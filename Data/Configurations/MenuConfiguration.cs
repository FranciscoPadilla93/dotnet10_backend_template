using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LUPA.Api.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Route)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Icon)
            .HasMaxLength(100);

        builder.Property(x => x.IsVisible)
            .HasDefaultValue(true);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(x => x.Module)
            .WithMany(x => x.Menus)
            .HasForeignKey(x => x.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ParentMenu)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new Menu
            {
                Id = 1,
                ModuleId = 1,
                ParentMenuId = null,
                Code = "USERS",
                Name = "Usuarios",
                Route = "/users",
                Icon = "people",
                SortOrder = 1,
                IsVisible = true,
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1),
                IsDeleted = false
            }
        );
    }
}
