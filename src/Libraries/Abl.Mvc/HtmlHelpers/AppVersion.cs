using System;
using System.Web.Mvc;


namespace Abl.Mvc.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {

        /// <summary>
        /// Returns the AssemblyInfo application version
        /// </summary>
        /// <param name="helper">This Html Helper</param>
        /// <param name="mvcAssembly">The assembly type</param>
        /// <returns>The AssemblyInfo define application version string</returns>
        /// <example>
        /// Version: @Html.AppVersion(typeof(Inbox.WebAdmin.MvcApplication))
        /// </example>
        public static MvcHtmlString AppVersion(
            this HtmlHelper helper,
            Type mvcAssembly)
        {
            string version = mvcAssembly.Assembly.GetName().Version.ToString();

#if DEBUG
         version += " [debug]";
#endif

            return MvcHtmlString.Create(version);
        }

    }
}