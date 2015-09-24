using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Identity;
using Identity.Resources;
using Identity.UI.ViewModels.Manage;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;


namespace UrfIdentity.Web.Controllers
{
    [Authorize]
    public class ManageController
        : BaseIdentityController
    {
        #region CTORs
        public ManageController()
        {
        }


        public ManageController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
            : base(userManager, signInManager)
        {
        }
        #endregion CTORs


        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess
                    ? AccountResources.ManageMessageId_ChangePasswordSuccess
                    : message == ManageMessageId.SetPasswordSuccess
                        ? AccountResources
                            .ManageMessageId_SetPasswordSuccess
                        : message == ManageMessageId.SetTwoFactorSuccess
                            ? AccountResources
                                .ManageMessageId_SetTwoFactorSuccess
                            : message == ManageMessageId.Error
                                ? AccountResources.ManageMessageId_Error
                                : message == ManageMessageId.AddPhoneSuccess
                                    ? AccountResources
                                        .ManageMessageId_AddPhoneSuccess
                                    : message ==
                                      ManageMessageId.RemovePhoneSuccess
                                        ? AccountResources
                                            .ManageMessageId_RemovePhoneSuccess
                                        : "";

            var userId = Guid.Parse(User.Identity.GetUserId());

            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered =
                    await
                        AuthenticationManager
                            .TwoFactorBrowserRememberedAsync(userId.ToString())
            };

            return View(model);
        }


        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(
            string loginProvider,
            string providerKey)
        {
            ManageMessageId? message;
            var userId = Guid.Parse(User.Identity.GetUserId());
            var result =
                await
                    UserManager.RemoveLoginAsync(userId,
                        new UserLoginInfo(loginProvider, providerKey));

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await
                        SignInManager.SignInAsync(
                            user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return RedirectToAction("ManageLogins", new {Message = message});
        }


        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }


        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(
            AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Guid.Parse(User.Identity.GetUserId());


            // Generate the token and send it
            var code =
                await
                    UserManager.GenerateChangePhoneNumberTokenAsync(userId, model.Number);

            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body =
                        String.Format(
                            AccountResources
                                .AddPhoneNumberCtrl_SecurityCodeMessage, code)
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction(
                "VerifyPhoneNumber", new {PhoneNumber = model.Number});
        }


        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            await
                UserManager.SetTwoFactorEnabledAsync(
                    userId, true);

            var user =
                await UserManager.FindByIdAsync(userId);

            if (user != null)
            {
                await
                    SignInManager.SignInAsync(
                        user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction(
                "Index", "Manage",
                new {message = ManageMessageId.SetTwoFactorSuccess});
        }


        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            await
                UserManager.SetTwoFactorEnabledAsync(userId, false);

            var user = await UserManager.FindByIdAsync(userId);

            if (user != null)
            {
                await
                    SignInManager.SignInAsync(
                        user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction(
                "Index", "Manage",
                new {message = ManageMessageId.SetTwoFactorSuccess});
        }


        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            await UserManager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber);

            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null
                ? View("Error")
                : View(
                    new VerifyPhoneNumberViewModel {PhoneNumber = phoneNumber});
        }


        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(
            VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Guid.Parse(User.Identity.GetUserId());

            var result =
                await
                    UserManager.ChangePhoneNumberAsync(userId, model.PhoneNumber,
                        model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    await
                        SignInManager.SignInAsync(
                            user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction(
                    "Index", new {Message = ManageMessageId.AddPhoneSuccess});
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(
                "",
                AccountResources.VerifyPhoneNumberCtrl_FailedToVerifyPhone);

            return View(model);
        }


        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());

            var result =
                await
                    UserManager.SetPhoneNumberAsync(userId, null);
            if (!result.Succeeded)
            {
                return RedirectToAction(
                    "Index", new {Message = ManageMessageId.Error});
            }

            var user =
                await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await
                    SignInManager.SignInAsync(
                        user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction(
                "Index", new {Message = ManageMessageId.RemovePhoneSuccess});
        }


        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }


        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(
            ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Guid.Parse(User.Identity.GetUserId());

            var result =
                await
                    UserManager.ChangePasswordAsync(userId, model.OldPassword,
                        model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);

                if (user != null)
                {
                    await
                        SignInManager.SignInAsync(
                            user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction(
                    "Index",
                    new {Message = ManageMessageId.ChangePasswordSuccess});
            }

            AddErrors(result);

            return View(model);
        }


        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }


        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(User.Identity.GetUserId());

                var result = await UserManager.AddPasswordAsync(userId, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        await
                            SignInManager.SignInAsync(
                                user, isPersistent: false,
                                rememberBrowser: false);
                    }
                    return RedirectToAction(
                        "Index",
                        new {Message = ManageMessageId.SetPasswordSuccess});
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess
                    ? AccountResources.ManageMessageId_RemoveLoginSuccess
                    : message == ManageMessageId.Error
                        ? AccountResources.ManageMessageId_Error
                        : "";

            var userId = Guid.Parse(User.Identity.GetUserId());

            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            var assignedLogins = await GetAssignedExternalLogins(userId);

            var unassignedLogins = GetUnassignedExternalLogins(assignedLogins);

            ViewBag.ShowRemoveButton = user.PasswordHash != null ||
                                       assignedLogins.Count > 1;

            return View(
                new ManageLoginsViewModel
                {
                    CurrentLogins = assignedLogins,
                    OtherLogins = unassignedLogins
                });
        }


        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(
                provider, Url.Action("LinkLoginCallback", "Manage"),
                User.Identity.GetUserId());
        }


        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo =
                await
                    AuthenticationManager.GetExternalLoginInfoAsync(
                        XsrfKey, User.Identity.GetUserId());

            if (loginInfo == null)
            {
                return RedirectToAction(
                    "ManageLogins", new {Message = ManageMessageId.Error});
            }

            var userId = Guid.Parse(User.Identity.GetUserId());

            var result =
                await
                    UserManager.AddLoginAsync(userId, loginInfo.Login);

            return result.Succeeded
                ? RedirectToAction("ManageLogins")
                : RedirectToAction(
                    "ManageLogins", new {Message = ManageMessageId.Error});
        }


        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private bool HasPassword()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var user = UserManager.FindById(userId);
            return user?.PasswordHash != null;
        }


        // private bool HasPhoneNumber()
        // {
        //     var userId = Guid.Parse(User.Identity.GetUserId());
        //     var user = UserManager.FindById(userId);
        //     return user?.PhoneNumber != null;
        // }


        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        #endregion
    }
}