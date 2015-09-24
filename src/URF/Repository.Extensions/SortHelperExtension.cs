using System;
using Abl.Data;
using System.Linq;
using System.Linq.Expressions;


namespace Repository.Extensions
{
    /// <summary>
    /// See http://genericunitofworkandrepositories.codeplex.com/discussions/561126
    /// </summary>
    /// <example>
    /// Use in Services like example below that:
    /// <![CDATA[
    /// 
    ///     string orderBy = "Column1";
    ///     base.Query().OrderBy(q => q.OrderBy(orderBy, SortDirection.Descending))
    ///         .SelectPage(page, pageSize, out totalCount);
    ///
    /// ]]>
    /// </example>
    public static class SortHelperExtension
    {
        #region Generic Methods
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string propertyName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return OrderingHelper(source, propertyName, sortDirection, false);
        }


        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string propertyName,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return OrderingHelper(source, propertyName, sortDirection, true);
        }
        #endregion Generic Methods


        #region SortExpression/PagingInfo Methods
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            SortExpression sortExpression)
        {
            return OrderingHelper(
                source, sortExpression.Expression, sortExpression.Direction, false);
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IQueryable<T> source,
            SortExpression sortExpression)
        {
            return OrderingHelper(
                source, sortExpression.Expression, sortExpression.Direction, true);
        }

        public static IOrderedQueryable<T> Sort<T>(
            this IQueryable<T> source,
            SortExpressionCollection sortExpressions,
            string defaultSortExpression)
        {
            if ((sortExpressions == null) || sortExpressions.Count < 1)
            {
                if (String.IsNullOrWhiteSpace(defaultSortExpression))
                {
                    throw new ArgumentException(
                        "A SortExpressionCollection or a default sort expression must be specified!",
                        nameof(defaultSortExpression));
                }

                return source.OrderBy(
                    defaultSortExpression, SortDirection.Ascending);
            }

            var sortedSource = source.OrderBy(sortExpressions[0]);

            if (sortExpressions.Count > 1)
            {
                for (var i = 1; i < sortExpressions.Count; i++)
                {
                    sortedSource = sortedSource.ThenBy(sortExpressions[i]);
                }
            }

            return sortedSource;
        }
        #endregion SortExpression/PagingInfo Methods


        #region Helper Functions
        private static IOrderedQueryable<T> OrderingHelper<T>(
            IQueryable<T> source,
            string propertyName,
            SortDirection sortDirection,
            bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(
                typeof (T), string.Empty); // I don't care about some naming
            MemberExpression property = Expression.PropertyOrField(
                param, propertyName);
            LambdaExpression sort = Expression.Lambda(property, param);

            MethodCallExpression call = Expression.Call(
                typeof (Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") +
                ((sortDirection == SortDirection.Descending)
                    ? "Descending"
                    : string.Empty),
                new[] {typeof (T), property.Type},
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>) source.Provider.CreateQuery<T>(call);
        }
        #endregion Helper Functions


    }
}