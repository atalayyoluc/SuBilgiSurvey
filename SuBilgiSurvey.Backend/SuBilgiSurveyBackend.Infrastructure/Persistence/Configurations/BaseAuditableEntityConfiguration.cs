using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public static class BaseAuditableEntityConfiguration
{
    static void Configure(EntityTypeBuilder<IBaseAuditableEntity> modelBuilder)
    {
        modelBuilder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        modelBuilder.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
    }

    public static ModelBuilder ApplyBaseAuditableEntityConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (var et in modelBuilder.Model.GetEntityTypes())
        {
            if (et.ClrType.IsAssignableTo(typeof(IBaseAuditableEntity)))
            { // IsSubclassOf
                Configure(new EntityTypeBuilder<IBaseAuditableEntity>(et));
            }
        }

        return modelBuilder;
    }
}