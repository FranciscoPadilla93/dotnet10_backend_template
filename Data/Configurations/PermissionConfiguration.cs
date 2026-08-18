using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LUPA.Api.Data.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne(x => x.Module)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Permission
            {
                Id = 1,
                ModuleId = 1,
                Code = "USER_VIEW",
                Name = "Ver Usuarios",
                Description = "Permite consultar usuarios",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 2,
                ModuleId = 1,
                Code = "USER_CREATE",
                Name = "Crear Usuarios",
                Description = "Permite crear usuarios",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 3,
                ModuleId = 1,
                Code = "USER_UPDATE",
                Name = "Editar Usuarios",
                Description = "Permite editar usuarios",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 4,
                ModuleId = 1,
                Code = "USER_DELETE",
                Name = "Eliminar Usuarios",
                Description = "Permite eliminar (soft delete) usuarios",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 5,
                ModuleId = 1,
                Code = "ROLE_VIEW",
                Name = "Ver Roles",
                Description = "Permite consultar roles",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 6,
                ModuleId = 1,
                Code = "ROLE_CREATE",
                Name = "Crear Roles",
                Description = "Permite crear roles",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 7,
                ModuleId = 1,
                Code = "ROLE_UPDATE",
                Name = "Editar Roles",
                Description = "Permite editar roles",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 8,
                ModuleId = 1,
                Code = "ROLE_DELETE",
                Name = "Eliminar Roles",
                Description = "Permite eliminar (soft delete) roles",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 9,
                ModuleId = 1,
                Code = "MODULE_VIEW",
                Name = "Ver Módulos",
                Description = "Permite consultar módulos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 10,
                ModuleId = 1,
                Code = "MODULE_CREATE",
                Name = "Crear Módulos",
                Description = "Permite crear módulos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 11,
                ModuleId = 1,
                Code = "MODULE_UPDATE",
                Name = "Editar Módulos",
                Description = "Permite editar módulos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 12,
                ModuleId = 1,
                Code = "MODULE_DELETE",
                Name = "Eliminar Módulos",
                Description = "Permite eliminar (soft delete) módulos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 13,
                ModuleId = 1,
                Code = "MENU_VIEW",
                Name = "Ver Menús",
                Description = "Permite consultar menús",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 14,
                ModuleId = 1,
                Code = "MENU_CREATE",
                Name = "Crear Menús",
                Description = "Permite crear menús",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 15,
                ModuleId = 1,
                Code = "MENU_UPDATE",
                Name = "Editar Menús",
                Description = "Permite editar menús",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 16,
                ModuleId = 1,
                Code = "MENU_DELETE",
                Name = "Eliminar Menús",
                Description = "Permite eliminar (soft delete) menús",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 17,
                ModuleId = 1,
                Code = "PERMISSION_VIEW",
                Name = "Ver Permisos",
                Description = "Permite consultar permisos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 18,
                ModuleId = 1,
                Code = "PERMISSION_CREATE",
                Name = "Crear Permisos",
                Description = "Permite crear permisos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 19,
                ModuleId = 1,
                Code = "PERMISSION_UPDATE",
                Name = "Editar Permisos",
                Description = "Permite editar permisos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 20,
                ModuleId = 1,
                Code = "PERMISSION_DELETE",
                Name = "Eliminar Permisos",
                Description = "Permite eliminar (soft delete) permisos",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                Id = 22,
                ModuleId = 1,
                Code = "AUDIT_LOG_VIEW",
                Name = "Ver Auditoría",
                Description = "Permite consultar el log de auditoría",
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission { Id = 23, ModuleId = 1, Code = "REPORT_VIEW", Name = "Ver Reportes", Description = "Permite consultar el catálogo de reportes", CreatedAt = new DateTime(2026, 1, 1), IsDeleted = false },
            new Permission { Id = 24, ModuleId = 1, Code = "REPORT_CREATE", Name = "Crear Reportes", Description = "Permite registrar nuevos reportes", CreatedAt = new DateTime(2026, 1, 1), IsDeleted = false },
            new Permission { Id = 25, ModuleId = 1, Code = "REPORT_UPDATE", Name = "Editar Reportes", Description = "Permite editar reportes", CreatedAt = new DateTime(2026, 1, 1), IsDeleted = false },
            new Permission { Id = 26, ModuleId = 1, Code = "REPORT_DELETE", Name = "Eliminar Reportes", Description = "Permite eliminar (soft delete) reportes", CreatedAt = new DateTime(2026, 1, 1), IsDeleted = false },
            new Permission { Id = 27, ModuleId = 1, Code = "REPORT_EXECUTE", Name = "Ejecutar Reportes", Description = "Permite ejecutar el stored procedure de un reporte", CreatedAt = new DateTime(2026, 1, 1), IsDeleted = false }
        );
    }
}