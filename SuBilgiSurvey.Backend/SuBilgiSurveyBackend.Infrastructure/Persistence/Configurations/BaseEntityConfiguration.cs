using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public static class BaseEntityConfiguration
{
    static void Configure(EntityTypeBuilder<BaseEntity> modelBuilder)
    {
    }

    public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (var et in modelBuilder.Model.GetEntityTypes())
        {
            // Console.WriteLine($"et.ClrType: {et.ClrType} IsAssignableTo IBaseAuditableEntity: {et.ClrType.IsAssignableTo(typeof(IBaseAuditableEntity))}");
            if (et.ClrType.IsAssignableTo(typeof(BaseEntity)))
            {
                Configure(new EntityTypeBuilder<BaseEntity>(et));
            }
        }

        modelBuilder.ApplyBaseAuditableEntityConfiguration();

        return modelBuilder;
    }
}