using System;
using System.Configuration;
using Identity;
using Identity.Extensions;
using Identity.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.LinkedIn;
using UrfIdentity.DAL.Db;
using UrfIdentity.Models;
using UrfIdentity.Web.Services.Identity;


namespace UrfIdentity.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(UrfIdentityDataContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(
                CreateApplicationUserManager);
            app.CreatePerOwinContext<ApplicationSignInManager>(
                CreateApplicationSignInManager);
            app.CreatePerOwinContext<ApplicationRoleManager>(
                CreateApplicationRoleManager);


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {
                    AuthenticationType =
                        DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                    Provider = new CookieAuthenticationProvider
                    {
                        // Enables the application to validate the security stamp when the user logs in.
                        // This is a security feature which is used when you change a password or add an external login to your account.  
                        OnValidateIdentity =
                            SecurityStampValidator
                                .OnValidateIdentity
                                <ApplicationUserManager, ApplicationUser, Guid>(
                                    TimeSpan.FromMinutes(30),
                                    regenerateIdentityCallback: (manager, user) =>
                                        user.GenerateUserIdentityAsync(manager),
                                    getUserIdCallback: id => Guid.Parse(id.GetUserId()))
                    }
                });

            app.UseExternalSignInCookie(
                DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(
                DefaultAuthenticationTypes.TwoFactorCookie,
                TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(
                DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            // Uncomment the following lines to enable logging in with third party login providers

            // http://www.codeproject.com/Articles/874207/LinkedIn-Authentication-in-ASP-NET-MVC
            // https://developer.linkedin.com/
            app.UseLinkedInAuthentication(
                ConfigurationManager.AppSettings["LinkedInAPIKey"],
                ConfigurationManager.AppSettings["LinkedInAPISecret"]
                );

            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }


        public static ApplicationUserManager CreateApplicationUserManager(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager =
                new ApplicationUserManager(
                    new ApplicationUserStore(context.Get<UrfIdentityDataContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application 
            // uses Phone and Emails as a step of receiving a code for 
            // verifying the user You can write your own provider and plug it 
            // in here.
            manager.RegisterTwoFactorProvider(
                "SMS Code", new PhoneNumberTokenProvider<ApplicationUser, Guid>
                {
                    MessageFormat = AccountResources.OAuth_SMS_Body
                });

            manager.RegisterTwoFactorProvider(
                "Email Code", new EmailTokenProvider<ApplicationUser, Guid>
                {
                    Subject = AccountResources.OAuth_Email_Subject,
                    BodyFormat = AccountResources.OAuth_Email_Body
                });

            manager.SmsService = new SmsMessagingService();
            manager.EmailService = new EmailMessagingService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser, Guid>(
                        dataProtectionProvider.Create("UrfIdentity"));
            }
            return manager;
        }


        public static ApplicationSignInManager CreateApplicationSignInManager(
            IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return
                new ApplicationSignInManager(
                    context.GetUserManager<ApplicationUserManager>(),
                    context.Authentication);
        }


        // See http://bitoftech.net/2015/03/11/asp-net-identity-2-1-roles-based-authorization-authentication-asp-net-web-api/
        public static ApplicationRoleManager CreateApplicationRoleManager(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var appRoleManager =
                new ApplicationRoleManager(
                    new ApplicationRoleStore(
                        context.Get<UrfIdentityDataContext>()));

            return appRoleManager;
        }
    }
}