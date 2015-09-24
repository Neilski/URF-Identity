using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        public static string GetPropertyNameFromExpression<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Not a memberExpression");

            if (!(memberExpression.Member is PropertyInfo))
                throw new InvalidOperationException("Not a property");

            return memberExpression.Member.Name;
        }
    }
}