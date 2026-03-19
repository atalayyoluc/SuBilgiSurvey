using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
{
    public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.HasOne(sq => sq.Survey)
            .WithMany(s => s.SurveyQuestions)
            .HasForeignKey(sq => sq.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(sq => sq.Question)
            .WithMany()
            .HasForeignKey(sq => sq.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(sq => new { sq.SurveyId, sq.QuestionId }).IsUnique();
    }
}
