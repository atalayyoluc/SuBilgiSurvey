using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Api;
using SuBilgiSurveyBackend.Application;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Infrastructure;
using SuBilgiSurveyBackend.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    ErrorResponseOptions.SetInstanceBaseUrl(builder.Configuration["ErrorResponse:InstanceBaseUrl"]);
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApiServices(builder.Configuration);
}


var app = builder.Build();
{
    if (app.Configuration.GetValue<bool>("ApplyMigrationsOnStartup"))
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();
    }

    app.UseCors("signalrCORS");
    app.UseProblemDetails();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}