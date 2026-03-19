using System.Diagnostics.CodeAnalysis;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Application.Common.Errors;

public static class ProblemDetailsThrower
{
    [DoesNotReturn]
    public static void Throw(ErrorMessage errorMessage)
    {
        throw new ProblemDetailsException(new ProblemDetails
        {
            Instance = $"{ErrorResponseOptions.InstanceBaseUrl.TrimEnd('/')}/{errorMessage.Instance}",
            Status = errorMessage.Status,
            Title = errorMessage.Title,
            Detail = errorMessage.Detail
        });
    }
}
