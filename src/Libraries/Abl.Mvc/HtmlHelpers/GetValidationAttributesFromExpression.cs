using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        public static IEnumerable<ValidationAttribute>
            GetValidationAttributesFromExpression<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var propertyName = htmlHelper.GetPropertyNameFromExpression(expression);
            Type type = typeof (TModel);
            var prop = type.GetProperty(propertyName);
            return prop.GetCustomAttributes(true).OfType<ValidationAttribute>();
        }
    }
}