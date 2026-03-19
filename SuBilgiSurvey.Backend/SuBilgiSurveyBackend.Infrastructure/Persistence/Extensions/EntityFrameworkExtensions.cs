using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace SuBilgiSurveyBackend.Infrastructure.Persistence.Extensions;

public static class EntityFrameworkExtensions
{
    // https://github.com/dotnet/efcore/issues/10275
    // https://github.com/dotnet/efcore/issues/10275#issuecomment-785916356
    public static void AppendQueryFilter<T>(
        this EntityTypeBuilder<T> entityTypeBuilder, Expression<Func<T, bool>> expression)
        where T : class
    {
        var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);

        var expressionFilter = ReplacingExpressionVisitor.Replace(
            expression.Parameters.Single(), parameterType, expression.Body);

        if (entityTypeBuilder.Metadata.GetQueryFilter() is { } currentQueryFilter)
        {
            var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
            expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
        }

        var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
        entityTypeBuilder.HasQueryFilter(lambdaExpression);
    }
}