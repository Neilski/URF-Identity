using System.Web.Helpers;
using System.Web.Mvc;


namespace UrfIdentity.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureViewEngines();
            ConfigureAntiForgeryTokens();
        }


        /// <summary>
        /// Configures the view engines. By default, Asp.Net MVC includes the Web Forms (WebFormsViewEngine) and 
        /// Razor (RazorViewEngine) view engines. You can remove view engines you are not using here for better
        /// performance.
        /// </summary>
        private static void ConfigureViewEngines()
        {
            // Only use the RazorViewEngine.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }


        /// <summary>
        /// Configures the anti-forgery tokens. See 
        /// http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages
        /// </summary>
        private static void ConfigureAntiForgeryTokens()
        {
            // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f". This adds a little security 
            // through obscurity and also saves sending a few characters over the wire. Sadly there is no way to change 
            // the form input name which is hard coded in the @Html.AntiForgeryToken helper and the 
            // ValidationAntiforgeryTokenAttribute to  __RequestVerificationToken.
            // <input name="__RequestVerificationToken" type="hidden" value="..." />
            AntiForgeryConfig.CookieName = "f";

            // If you have enabled SSL. Uncomment this line to ensure that the Anti-Forgery 
            // cookie requires SSL to be sent across the wire. 
            // AntiForgeryConfig.RequireSsl = true;
        }
    }
}