using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString CheckBoxListFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, object>> expression,    // The collection to be returned
            IEnumerable<SelectListItem> selectList,         // Available options
            object htmlAttributes = null)                   // Additional CSS classes
        {
            var ul = new TagBuilder("ul");

            if (htmlAttributes != null)
            {
                // Get the attributes
                IDictionary<string, object> attributes =
                    HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                // set the attributes
                ul.MergeAttributes(attributes);
            }

            ul.AddCssClass("check-box-list");


            var metaData = ModelMetadata.FromLambdaExpression(expression,
                htmlHelper.ViewData);


            if (selectList == null) return MvcHtmlString.Create(ul.ToString());

            foreach (var item in selectList)
            {
                //  < li class="checkbox">
                //      <label>
                //          <input type="checkbox" name="PropertyName" value="22" />
                //          Property Text
                //      </label>
                //  </li>

                var li = new TagBuilder("li");
                var label = new TagBuilder("label");
                var checkbox = new TagBuilder("input");

                li.AddCssClass("checkbox");
                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute("name", metaData.PropertyName);
                checkbox.MergeAttribute("value", item.Value);
                if (item.Selected) checkbox.MergeAttribute("checked", "checked");
                checkbox.GenerateId(metaData.PropertyName);
                label.InnerHtml = checkbox.ToString(TagRenderMode.SelfClosing);
                label.InnerHtml += " " + item.Text;
                li.InnerHtml = label.ToString();
                ul.InnerHtml += li.ToString();
            }

            return MvcHtmlString.Create(ul.ToString());
        }
    }
}