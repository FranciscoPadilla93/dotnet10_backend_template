using LUPA.Api.Common.Authorization;
using LUPA.Api.Configuration;
using LUPA.Api.Extensions;
using LUPA.Api.Middlewares;
using LUPA.Api.Services.Auth;
using LUPA.Api.Services.Interfaces;
using LUPA.Api.Services.Audit;
using LUPA.Api.Services.Email;
using LUPA.Api.Services.Menus;
using LUPA.Api.Services.Modules;
using LUPA.Api.Services.Permissions;
using LUPA.Api.Services.Reports;
using LUPA.Api.Services.Roles;
using LUPA.Api.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddControllers();
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection(EmailOptions.SectionName));

builder.Services.AddSingleton<EmailQueue>();
builder.Services.AddSingleton<IEmailService>(sp => sp.GetRequiredService<EmailQueue>());
builder.Services.AddHostedService<EmailBackgroundService>();

var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)),

            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine("================================");
                Console.WriteLine($"TOKEN: [{context.Token}]");
                Console.WriteLine($"AUTH HEADER: [{context.Request.Headers["Authorization"]}]");
                Console.WriteLine("================================");

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(context.Exception.ToString());
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("TOKEN OK");
                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                Console.WriteLine($"CHALLENGE = {context.Error} {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportExecutionService, ReportExecutionService>();

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
}

await app.SeedDatabaseAsync();

app.Run();