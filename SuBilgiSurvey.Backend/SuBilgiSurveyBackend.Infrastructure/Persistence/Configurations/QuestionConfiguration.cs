using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(q => q.Text).HasMaxLength(1000).IsRequired();
        builder.HasOne(q => q.AnswerTemplate)
            .WithMany()
            .HasForeignKey(q => q.AnswerTemplateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
