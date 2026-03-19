using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class SurveyResponseConfiguration : IEntityTypeConfiguration<SurveyResponse>
{
    public void Configure(EntityTypeBuilder<SurveyResponse> builder)
    {
        builder.HasOne(sr => sr.Survey)
            .WithMany(s => s.SurveyResponses)
            .HasForeignKey(sr => sr.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(sr => sr.User)
            .WithMany()
            .HasForeignKey(sr => sr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(sr => new { sr.SurveyId, sr.UserId }).IsUnique();
        builder.HasMany(sr => sr.Details)
            .WithOne(d => d.SurveyResponse)
            .HasForeignKey(d => d.SurveyResponseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
