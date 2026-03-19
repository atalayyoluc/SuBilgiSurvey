using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class AnswerTemplateOptionConfiguration : IEntityTypeConfiguration<AnswerTemplateOption>
{
    public void Configure(EntityTypeBuilder<AnswerTemplateOption> builder)
    {
        builder.Property(o => o.OptionText).HasMaxLength(500).IsRequired();
        builder.HasOne(o => o.AnswerTemplate)
            .WithMany(a => a.Options)
            .HasForeignKey(o => o.AnswerTemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
