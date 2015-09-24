using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        // Requires local jquery.charactercounter support
        public static MvcHtmlString CharacterCounterFor<TModel, TValue>(
            this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression,
            object htmlAttributes = null)
        {
            var validationAttributes =
                helper.GetValidationAttributesFromExpression(expression);

            var stringLengthAttribute =
                validationAttributes.OfType<StringLengthAttribute>()
                    .FirstOrDefault();

            if (stringLengthAttribute == null)
            {
                var propertyName = helper.GetPropertyNameFromExpression(expression);
                throw new Exception(
                    string.Format(MvcResources.MissingStringLengthValidationAttribute,
                        propertyName));
            }

            var maxLength = stringLengthAttribute.MaximumLength;

            var container = new TagBuilder("div");

            if (htmlAttributes != null)
            {
                // Get the attributes
                IDictionary<string, object> attributes =
                    HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                // set the attributes
                container.MergeAttributes(attributes);
            }

            container.AddCssClass("character-counter");
            container.MergeAttribute("data-targetid", helper.IdFor(expression).ToString());
            container.MergeAttribute("data-maxlength", maxLength.ToString());

            container.InnerHtml =
                $"<span class='no-characters'>0</span>/{maxLength} " +
                $"{MvcResources.CharacterCount_Characters}";

            return MvcHtmlString.Create(container.ToString(TagRenderMode.Normal));
        }
    }
}