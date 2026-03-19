using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        builder.Property(s => s.Title).HasMaxLength(300).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(2000);
        builder.Property(s => s.Status).HasConversion<int>().IsRequired();
        builder.HasMany(s => s.SurveyQuestions)
            .WithOne(sq => sq.Survey)
            .HasForeignKey(sq => sq.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(s => s.SurveyAssignments)
            .WithOne(sa => sa.Survey)
            .HasForeignKey(sa => sa.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(s => s.SurveyResponses)
            .WithOne(sr => sr.Survey)
            .HasForeignKey(sr => sr.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
