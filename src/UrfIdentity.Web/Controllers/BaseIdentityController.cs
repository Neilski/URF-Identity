using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abl;
using Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace UrfIdentity.Web.Controllers
{
    public abstract class BaseIdentityController
        : Controller
    {
        // Used for XSRF protection when adding external logins
        protected const string XsrfKey = "XsrfId";


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        #region CTORs
        protected BaseIdentityController()
        {
        }


        protected BaseIdentityController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager?.Dispose();
                _signInManager?.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion CTORs


        #region Properties
        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ??
                       HttpContext.GetOwinContext()
                           .Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                       HttpContext.GetOwinContext()
                           .GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }


        protected IAuthenticationManager AuthenticationManager
            => HttpContext.GetOwinContext().Authentication;
        #endregion Properties


        protected async Task<IList<UserLoginInfo>> GetAssignedExternalLogins(
            Guid userId)
        {
            return await UserManager.GetLoginsAsync(User.Identity.GetUserId().ToGuid());
        }


        protected IList<AuthenticationDescription> GetUnassignedExternalLogins(
            IList<UserLoginInfo> userLogins)
        {
            return
                AuthenticationManager.GetExternalAuthenticationTypes()
                    .Where(
                        auth =>
                            userLogins.All(
                                ul =>
                                    auth.AuthenticationType != ul.LoginProvider))
                    .ToList();
        }
    }
}