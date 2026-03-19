using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Common;
using SuBilgiSurveyBackend.Infrastructure.Persistence.Extensions;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public static class BaseDeletableEntityConfiguration
{
    static void Configure(EntityTypeBuilder<IBaseDeletableEntity> modelBuilder)
    {
        // https://learn.microsoft.com/en-us/ef/core/querying/filters#accessing-entity-with-query-filter-using-required-navigation
        // https://www.gencayyildiz.com/blog/entity-framework-core-global-query-filters/

        // modelBuilder.HasQueryFilter(u => !u.IsDeleted);

        // real one
        modelBuilder.AppendQueryFilter(u => !u.IsDeleted);
    }

    public static ModelBuilder ApplyBaseDeletableEntityConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (var et in modelBuilder.Model.GetEntityTypes())
        {
            if (et.ClrType.IsAssignableTo(typeof(IBaseDeletableEntity)))
            { // IsSubclassOf
                Configure(new EntityTypeBuilder<IBaseDeletableEntity>(et));
            }
        }

        return modelBuilder;
    }
}