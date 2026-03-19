using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<AnswerTemplate> AnswerTemplates { get; }
    DbSet<AnswerTemplateOption> AnswerTemplateOptions { get; }
    DbSet<Question> Questions { get; }
    DbSet<Survey> Surveys { get; }
    DbSet<SurveyQuestion> SurveyQuestions { get; }
    DbSet<SurveyAssignment> SurveyAssignments { get; }
    DbSet<SurveyResponse> SurveyResponses { get; }
    DbSet<SurveyResponseDetail> SurveyResponseDetails { get; }
    int SaveChanges();
    int SaveChanges(bool acceptAllChangesOnSuccess);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
}