using System.Collections.Generic;
using System.Web.Mvc;
using Abl.Security;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        public static MvcHtmlString Gravatar(
            this HtmlHelper htmlHelper,
            string email,
            int size = 80,
            string defaultImage = "mm",
            object htmlAttributes = null)
        {
            var img = new TagBuilder("img");

            if (htmlAttributes != null)
            {
                // Get the attributes
                IDictionary<string, object> attributes =
                    HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

                // set the attributes
                img.MergeAttributes(attributes);
            }

            img.AddCssClass("gravatar");

            var hash = MD5Utils.GetMd5Hash(email.ToLower());
            var encodedSize = htmlHelper.Encode(size);
            var encodedDefaultImage = htmlHelper.Encode(defaultImage);
            var url = $"//gravatar.com/avatar/{hash}.jpg?s={encodedSize}&d={encodedDefaultImage}";
            img.MergeAttribute("src", url);

            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}
