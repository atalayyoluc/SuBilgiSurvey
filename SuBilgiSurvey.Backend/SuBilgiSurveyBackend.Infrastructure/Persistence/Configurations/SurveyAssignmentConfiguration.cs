using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Configurations;

public class SurveyAssignmentConfiguration : IEntityTypeConfiguration<SurveyAssignment>
{
    public void Configure(EntityTypeBuilder<SurveyAssignment> builder)
    {
        builder.HasOne(sa => sa.Survey)
            .WithMany(s => s.SurveyAssignments)
            .HasForeignKey(sa => sa.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(sa => sa.User)
            .WithMany()
            .HasForeignKey(sa => sa.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(sa => new { sa.SurveyId, sa.UserId }).IsUnique();
    }
}
