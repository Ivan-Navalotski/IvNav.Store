using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace IvNav.Store.Infrastructure.Extensions;

internal static class EntityTypeBuilderExtensions
{
    internal static void AddQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder, Expression<Func<T, bool>> expression)
    {
        var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
        var expressionFilter = ReplacingExpressionVisitor.Replace(
            expression.Parameters.Single(), parameterType, expression.Body);

        AddQueryFilterCombined(entityTypeBuilder, expressionFilter, parameterType);
    }

    private static void AddQueryFilterCombined(EntityTypeBuilder entityTypeBuilder, Expression expressionFilter, ParameterExpression parameter)
    {
        var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
        if (currentQueryFilter != null)
        {
            var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                currentQueryFilter.Parameters.Single(), parameter, currentQueryFilter.Body);

            expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
        }

        var lambdaExpression = Expression.Lambda(expressionFilter, parameter);
        entityTypeBuilder.HasQueryFilter(lambdaExpression);
    }
}
