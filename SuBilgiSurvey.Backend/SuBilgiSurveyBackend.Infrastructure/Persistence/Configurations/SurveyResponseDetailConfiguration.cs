using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class SurveyResponseDetailConfiguration : IEntityTypeConfiguration<SurveyResponseDetail>
{
    public void Configure(EntityTypeBuilder<SurveyResponseDetail> builder)
    {
        builder.HasOne(d => d.SurveyResponse)
            .WithMany(sr => sr.Details)
            .HasForeignKey(d => d.SurveyResponseId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(d => d.Question)
            .WithMany()
            .HasForeignKey(d => d.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.AnswerTemplateOption)
            .WithMany()
            .HasForeignKey(d => d.AnswerTemplateOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
