using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class AnswerTemplateConfiguration : IEntityTypeConfiguration<AnswerTemplate>
{
    public void Configure(EntityTypeBuilder<AnswerTemplate> builder)
    {
        builder.Property(a => a.Name).HasMaxLength(200).IsRequired();
        builder.HasMany(a => a.Options)
            .WithOne(o => o.AnswerTemplate)
            .HasForeignKey(o => o.AnswerTemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
