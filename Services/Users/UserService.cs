using LUPA.Api.Common.Audit;
using LUPA.Api.Common.Email;
using LUPA.Api.Common.Excel;
using LUPA.Api.Common.Exceptions;
using LUPA.Api.Entities;
using LUPA.Api.Repositories.Contracts;
using LUPA.Api.Requests;
using LUPA.Api.Responses;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Auth;
using LUPA.Api.Services.Email;
using LUPA.Api.Services.Base;
using LUPA.Api.Services.Interfaces;

namespace LUPA.Api.Services.Users;

public class UserService
    : BaseService<User, UserResponse, CreateUserRequest, UpdateUserRequest>, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IAuditLogService auditLogService,
        IEmailService emailService)
        : base(userRepository, auditLogService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
    }

    protected override string NotFoundMessage => "Usuario no encontrado.";

    protected override async Task<UserResponse> MapToResponseAsync(User entity)
    {
        var roles = await _userRepository.GetRolesAsync(entity.Id);

        return UserMapper.ToResponse(entity, roles);
    }

    protected override Task<User> MapToEntityAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true
        };

        return Task.FromResult(user);
    }

    protected override Task ApplyUpdateAsync(User entity, UpdateUserRequest request)
    {
        entity.Email = request.Email;
        entity.FirstName = request.FirstName;
        entity.LastName = request.LastName;

        return Task.CompletedTask;
    }

    protected override async Task AfterCreateAsync(User entity, CreateUserRequest request)
    {
        if (request.RoleIds.Count > 0)
        {
            await _userRepository.SetRolesAsync(entity.Id, request.RoleIds);
        }

        await _emailService.QueueEmailAsync(new EmailMessage
        {
            To = entity.Email,
            Subject = "Bienvenido a LUPA",
            Body = $"""
                <p>Hola {entity.FirstName},</p>
                <p>Tu cuenta en LUPA fue creada correctamente.</p>
                <p>Tu usuario es: <strong>{entity.Username}</strong></p>
                """,
            IsHtml = true
        });
    }

    protected override async Task AfterUpdateAsync(User entity, UpdateUserRequest request)
    {
        await _userRepository.SetRolesAsync(entity.Id, request.RoleIds);
    }

    public async Task ActivateAsync(int id, bool isActive)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(NotFoundMessage);

        string? beforeJson = AuditSerializer.Serialize(user);

        user.IsActive = isActive;

        await _userRepository.UpdateAsync(user);

        string? afterJson = AuditSerializer.Serialize(user);

        await AuditLog.LogAsync(
            isActive ? "ACTIVATE" : "DEACTIVATE", EntityName, id.ToString(), beforeJson, afterJson);
    }

    public async Task ChangePasswordAsync(int id, ChangePasswordRequest request)
    {
        var user = await _userRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(NotFoundMessage);

        bool validCurrentPassword = _passwordHasher.Verify(request.CurrentPassword, user.PasswordHash);

        if (!validCurrentPassword)
        {
            throw new ValidationException("La contraseña actual no es correcta.");
        }

        user.PasswordHash = _passwordHasher.Hash(request.NewPassword);

        await _userRepository.UpdateAsync(user);

        // Nunca guardamos hashes de contraseña en el log, ni antes ni después: solo el evento.
        await AuditLog.LogAsync("CHANGE_PASSWORD", EntityName, id.ToString(), beforeJson: null, afterJson: null);
    }

    public async Task<ExcelImportResult> ImportFromExcelAsync(Stream fileStream)
    {
        var rows = ExcelImporter.ReadRows(fileStream);

        var result = new ExcelImportResult { TotalRows = rows.Count };

        for (int i = 0; i < rows.Count; i++)
        {
            var row = rows[i];
            int excelRowNumber = i + 2; // fila 1 = encabezados

            try
            {
                var request = new CreateUserRequest
                {
                    Username = row.GetValueOrDefault("Username", string.Empty),
                    Email = row.GetValueOrDefault("Email", string.Empty),
                    Password = row.GetValueOrDefault("Password", "Temporal123*"),
                    FirstName = row.GetValueOrDefault("FirstName", string.Empty),
                    LastName = row.GetValueOrDefault("LastName", string.Empty),
                    RoleIds = []
                };

                // Reutiliza el CreateAsync normal: valida, guarda y audita exactamente
                // igual que si lo hubieras creado a mano desde Postman.
                await CreateAsync(request);

                result.SuccessCount++;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Fila {excelRowNumber}: {ex.Message}");
            }
        }

        return result;
    }
}