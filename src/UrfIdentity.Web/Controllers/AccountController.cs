using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Identity;
using Identity.Resources;
using Identity.UI.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Repository.Pattern.Infrastructure;
using UrfIdentity.DAL.Repository.Infrastructure;
using UrfIdentity.DAL.Service.Interfaces;
using UrfIdentity.Models;


namespace UrfIdentity.Web.Controllers
{
    // See http://www.asp.net/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset
    // For additional setup/configuration options

    [Authorize]
    public class AccountController : BaseIdentityController
    {
        private readonly IUrfIdentityUnitOfWorkAsync _unitOfWork;
        private readonly IApplicationUserService _applicationUserService;



        #region CTORs
        public AccountController(
            IUrfIdentityUnitOfWorkAsync uow,
            IApplicationUserService aus)
        {
            _unitOfWork = uow;
            _applicationUserService = aus;
        }


        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
            : base(userManager, signInManager)
        {
        }
        #endregion CTORs



        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(
            LoginViewModel model,
            string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    return View(
                        "ConfirmEmailBeforeLogin",
                        new ResendEmailConfirmationViewModel
                        {
                            Email = user.Email
                        });
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout,
            // change to shouldLockout: true
            var result =
                await
                    SignInManager.PasswordSignInAsync(
                        model.Email, model.Password, model.RememberMe,
                        shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var lastLogin = await SetLastLoginDateTime(user);
                    return (lastLogin == null)
                        ? RedirectToAction("FirstTimeLogin")
                        : RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction(
                        "SendCode",
                        new
                        {
                            ReturnUrl = returnUrl,
                            RememberMe = model.RememberMe
                        });
                default:
                    ModelState.AddModelError(
                        "", AccountResources.LoginCtrl_InvalidLoginAttempt);
                    return View(model);
            }
        }


        public async Task<ActionResult> FirstTimeLogin()
        {
            var user =
                await UserManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId()));

            if (user == null)
            {
                return View("Error");
            }

            var assignedLogins =
                await GetAssignedExternalLogins(Guid.Parse(User.Identity.GetUserId()));

            var unassignedLogins = GetUnassignedExternalLogins(assignedLogins);

            return View(
                "FirstTimeLogin", new FirstTimeLoginViewModel
                {
                    FirstName = user.UserDetails.FirstName,
                    ExternalLogins = unassignedLogins
                });
        }


        //
        // POST: /Account/ResendEmailConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResendEmailConfirmation(
            ResendEmailConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }


            // Require the user to have a confirmed email before they can log on.
            var user = await UserManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                // Send an email with this link
                await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                await
                    SendEmailConfirmationTokenAsync(user.Id,
                        AccountResources.RegisterEmailConfirmation_Subject);

                return View(
                    "ConfirmationEmailSent",
                    new ConfirmationEmailSentViewModel
                    {
                        Email = user.Email
                    });
            }

            ViewBag.ErrorMessage =
                AccountResources.UnableToResendEmailConfirmation;
            return View("Error");
        }


        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(
            string provider,
            string returnUrl,
            bool rememberMe)
        {
            // Require that the user has already logged in via
            // username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return
                View(
                    new VerifyCodeViewModel
                    {
                        Provider = provider,
                        ReturnUrl = returnUrl,
                        RememberMe = rememberMe
                    });
        }


        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the
            // two factor codes.  If a user enters incorrect codes for a 
            // specified amount of time then the user account  will be locked 
            // out for a specified amount of time. You can configure the 
            // account lockout settings in IdentityConfig
            var result =
                await
                    SignInManager.TwoFactorSignInAsync(
                        model.Provider, model.Code,
                        isPersistent: model.RememberMe,
                        rememberBrowser: model.RememberBrowser);


            switch (result)
            {
                case SignInStatus.Success:
                    var userId = await SignInManager.GetVerifiedUserIdAsync();
                    await SetLastLoginDateTime(userId);
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                // case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(
                        "", AccountResources.VerifyCodeCtrl_InvalidCode);
                    return View(model);
            }
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await CreateNewUser(model);

                if (result.Succeeded)
                {
                    return View(
                        "ConfirmationEmailSent",
                        new ConfirmationEmailSentViewModel
                        {
                            Email = model.Email
                        });
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(Guid userId, string code)
        {
            if (userId == Guid.Empty || code == null)
            {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(
            ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null ||
                    !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation 
                // and password reset please visit
                // http://go.microsoft.com/fwlink/?LinkID=320771
                //
                // Send an email with this link
                string code =
                    await UserManager.GeneratePasswordResetTokenAsync(user.Id);

                var callbackUrl = Url.Action(
                    "ResetPassword", "Account",
                    new {userId = user.Id, code = code},
                    protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(
                    user.Id,
                    AccountResources.ResetPassword,
                    String.Format(
                        AccountResources.ForgotPasswordCtrl_Message,
                        callbackUrl));

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }


        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(
            ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result =
                await
                    UserManager.ResetPasswordAsync(
                        user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);

            return View();
        }


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback", "Account",
                    new {ReturnUrl = returnUrl}));
        }


        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(
            string returnUrl,
            bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == Guid.Empty)
            {
                return View("Error");
            }
            var userFactors =
                await UserManager.GetValidTwoFactorProvidersAsync(userId);

            var factorOptions =
                userFactors.Select(
                    purpose =>
                        new SelectListItem {Text = purpose, Value = purpose})
                    .ToList();
            return
                View(
                    new SendCodeViewModel
                    {
                        Providers = factorOptions,
                        ReturnUrl = returnUrl,
                        RememberMe = rememberMe
                    });
        }


        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (
                !await
                    SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }

            return RedirectToAction(
                "VerifyCode",
                new
                {
                    Provider = model.SelectedProvider,
                    ReturnUrl = model.ReturnUrl,
                    RememberMe = model.RememberMe
                });
        }


        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo =
                await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result =
                await
                    SignInManager.ExternalSignInAsync(
                        loginInfo, isPersistent: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByNameAsync(loginInfo.Email);
                    await SetLastLoginDateTime(user);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction(
                        "SendCode",
                        new {ReturnUrl = returnUrl, RememberMe = false});
                // case SignInStatus.Failure:
                default:

                    bool forceInitialLocalAccount =
                        Convert.ToBoolean(
                            ConfigurationManager.AppSettings["ForceInitialLocalAccount"]);

                    if (forceInitialLocalAccount)
                    {
                        return View(
                            "UnassignedExternalLogin",
                            new UnassignedExternalLoginViewModel
                            {
                                Provider = loginInfo.Login.LoginProvider
                            });
                    }

                    // If the user does not have a local account, then prompt the 
                    // user to create one - or you could do it automatically
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

                    return View(
                        "ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel
                        {
                            Email = loginInfo.Email
                        });
            }
        }


        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(
            ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external
                // login provider
                var info =
                    await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    ObjectState = ObjectState.Added,
                    Created = DateTime.Now
                };

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result =
                        await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await
                            SignInManager.SignInAsync(
                                user, isPersistent: false,
                                rememberBrowser: false);


                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }



        #region Helpers
        private async Task<DateTime?> SetLastLoginDateTime(Guid userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            return await SetLastLoginDateTime(user);
        }


        private async Task<DateTime?> SetLastLoginDateTime(ApplicationUser user)
        {
            DateTime? lastLogin = user.LastLogin;
            user.LastLogin = DateTime.Now;
            await UserManager.UpdateAsync(user);
            return lastLogin;
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }


            public ChallengeResult(
                string provider,
                string redirectUri,
                string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }


            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }


            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = RedirectUri
                };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext()
                    .Authentication.Challenge(properties, LoginProvider);
            }
        }


        /*
        ** Added by Abilitation to support multiple registration points
        */


        private async Task<IdentityResult> CreateNewUser(
            RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Created = DateTime.Now, // Custom field initialiser
                UserDetails = new ApplicationUserDetails // Custom field initialiser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    JobTitle = model.JobTitle,
                    Company = model.Company,
                    Phone = model.Phone,
                    ObjectState = ObjectState.Added // Still necessary!
                }
            };

            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return result;

            // Comment the following line to prevent log in until the 
            // user is confirmed.await
            // SignInManager.SignInAsync(user,
            //    isPersistent: false, rememberBrowser: false);

            // For more information on how to enable account 
            // confirmation and password reset please visit
            // http://go.microsoft.com/fwlink/?LinkID=320771

            // Send an email with this link
            await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            await
                SendEmailConfirmationTokenAsync(user.Id,
                    AccountResources.RegisterEmailConfirmation_Subject);

            return result;
        }


        // See http://www.asp.net/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset
        private async Task<string> SendEmailConfirmationTokenAsync(
            Guid userId,
            string subject)
        {
            string code =
                await UserManager.GenerateEmailConfirmationTokenAsync(userId);

            var callbackUrl = Url.Action(
                "ConfirmEmail", "Account",
                new
                {
                    userId = userId,
                    code = code
                }, protocol: Request.Url.Scheme);

            await UserManager.SendEmailAsync(
                userId, subject,
                String.Format(
                    AccountResources.RegisterEmailConfirmation_Message,
                    callbackUrl));

            return callbackUrl;
        }
        #endregion
    }
}