using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        public static IHtmlString MetaAcceptedLanguage(this HtmlHelper html)
        {
            var acceptedLanguage =
                HttpUtility.HtmlAttributeEncode(
                    Thread.CurrentThread.CurrentUICulture.ToString());

            var meta = new TagBuilder("meta");
            meta.MergeAttribute("name", "accepted-language");
            meta.MergeAttribute("content", acceptedLanguage);
            return MvcHtmlString.Create(meta.ToString(TagRenderMode.StartTag));

        }
    }
}