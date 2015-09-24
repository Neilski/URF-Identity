using Abl.Data;
using System.Collections.Generic;
using System.Web.Mvc;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {

        public static MvcHtmlString SortExpressionLink(
            this HtmlHelper helper,
            string title,
            string sortExpression,
            SortDirection direction = SortDirection.Ascending,
            object htmlAttributes = null)
        {
            var a = new TagBuilder("a");

            if (htmlAttributes != null)
            {
                // get the attributes
                IDictionary<string, object> attributes =
                    HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)
                        as IDictionary<string, object>;

                // set the attributes
                a.MergeAttributes(attributes);
            }

            var i = new TagBuilder("i");
            i.MergeAttribute("class", "indicator");

            a.AddCssClass("sort-expression-link");

            a.MergeAttribute("title", title);
            a.MergeAttribute("href", "#" + sortExpression);
            a.MergeAttribute("data-sort-expression", sortExpression);
            a.MergeAttribute("data-sort-direction", direction.ToString());
            a.InnerHtml = title + i.ToString(TagRenderMode.Normal);

            return
                MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }

    }
}