using LUPA.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LUPA.Api.Data.Configurations;

public class ReportParameterConfiguration : IEntityTypeConfiguration<ReportParameter>
{
    public void Configure(EntityTypeBuilder<ReportParameter> builder)
    {
        builder.ToTable("ReportParameters");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Label).HasMaxLength(150).IsRequired();
        builder.Property(x => x.DefaultValue).HasMaxLength(500);

        builder.Property(x => x.DataType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasIndex(x => new { x.ReportId, x.Name }).IsUnique();
    }
}