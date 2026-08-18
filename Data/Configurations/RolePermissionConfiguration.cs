using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LUPA.Api.Data.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(x => new
        {
            x.RoleId,
            x.PermissionId
        });

        builder.HasOne(x => x.Role)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Permission)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rol Super Admin (Id=1) obtiene todos los permisos del módulo de Seguridad desde el arranque.
        builder.HasData(
            new RolePermission { RoleId = 1, PermissionId = 1 },  // USER_VIEW
            new RolePermission { RoleId = 1, PermissionId = 2 },  // USER_CREATE
            new RolePermission { RoleId = 1, PermissionId = 3 },  // USER_UPDATE
            new RolePermission { RoleId = 1, PermissionId = 4 },  // USER_DELETE
            new RolePermission { RoleId = 1, PermissionId = 5 },  // ROLE_VIEW
            new RolePermission { RoleId = 1, PermissionId = 6 },  // ROLE_CREATE
            new RolePermission { RoleId = 1, PermissionId = 7 },  // ROLE_UPDATE
            new RolePermission { RoleId = 1, PermissionId = 8 },  // ROLE_DELETE
            new RolePermission { RoleId = 1, PermissionId = 9 },  // MODULE_VIEW
            new RolePermission { RoleId = 1, PermissionId = 10 }, // MODULE_CREATE
            new RolePermission { RoleId = 1, PermissionId = 11 }, // MODULE_UPDATE
            new RolePermission { RoleId = 1, PermissionId = 12 }, // MODULE_DELETE
            new RolePermission { RoleId = 1, PermissionId = 13 }, // MENU_VIEW
            new RolePermission { RoleId = 1, PermissionId = 14 }, // MENU_CREATE
            new RolePermission { RoleId = 1, PermissionId = 15 }, // MENU_UPDATE
            new RolePermission { RoleId = 1, PermissionId = 16 }, // MENU_DELETE
            new RolePermission { RoleId = 1, PermissionId = 17 }, // PERMISSION_VIEW
            new RolePermission { RoleId = 1, PermissionId = 18 }, // PERMISSION_CREATE
            new RolePermission { RoleId = 1, PermissionId = 19 }, // PERMISSION_UPDATE
            new RolePermission { RoleId = 1, PermissionId = 20 }, // PERMISSION_DELETE
            new RolePermission { RoleId = 1, PermissionId = 22 }, // AUDIT_LOG_VIEW
            new RolePermission { RoleId = 1, PermissionId = 23 }, // REPORT_VIEW
            new RolePermission { RoleId = 1, PermissionId = 24 }, // REPORT_CREATE
            new RolePermission { RoleId = 1, PermissionId = 25 }, // REPORT_UPDATE
            new RolePermission { RoleId = 1, PermissionId = 26 }, // REPORT_DELETE
            new RolePermission { RoleId = 1, PermissionId = 27 }  // REPORT_EXECUTE
        );
    }
}