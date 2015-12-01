using System.Threading.Tasks;
using System.Web.Mvc;
using Identity;
using UrfIdentity.Models;
using UrfIdentity.Web.Models.RoleManager;


namespace UrfIdentity.Web.Controllers
{
    [Authorize]
    public class RoleManagerController
        : BaseIdentityController
    {
        protected readonly ApplicationRoleManager RoleManager;


        public RoleManagerController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;
        }


        public ActionResult Index()
        {
            return View();
        }



        #region Create Role
        public ActionResult CreateRole()
        {
            return View();
        }


        [ActionName("CreateRole"), HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole_POST(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await RoleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError("Role",
                    $"The role '{model.Role}' is already registered!");
            }
            else
            {
                await RoleManager.CreateAsync(new ApplicationRole(model.Role));
                TempData["Message"] = $"Role '{model.Role}' created successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion Create Role



        #region Delete Role
        public ActionResult DeleteRole()
        {
            return View();
        }


        [ActionName("DeleteRole"), HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole_POST(DeleteRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await RoleManager.FindByNameAsync(model.Role);

            if (role == null)
            {
                ModelState.AddModelError("Role",
                    $"The role '{model.Role}' does not exist!");
            }
            else
            {
                await RoleManager.DeleteAsync(role);
                TempData["Message"] = $"Role '{model.Role}' deleted successfully!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion Create Role



        #region Assign Role
        public ActionResult AssignRole()
        {
            return View();
        }


        [ActionName("AssignRole"), HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRole_POST(AssignUserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            var role = await RoleManager.FindByNameAsync(model.Role);
            if (user == null)
            {
                ModelState.AddModelError("Email",
                    $"No user found with the email address '{model.Email}'!");
            }
            else if (role == null)
            {
                ModelState.AddModelError("Role",
                    $"The role '{model.Role}' does not exist!");
            }
            else
            {
                await UserManager.AddToRoleAsync(user.Id, role.Name);
                TempData["Message"] = $"User '{user.Email}' has been assigned to the role '{role.Name}'!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion Assign Role



        #region Unassign Role
        public ActionResult UnassignRole()
        {
            return View();
        }


        [ActionName("UnassignRole"), HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UnassignRole_POST(UnassignUserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            var role = await RoleManager.FindByNameAsync(model.Role);
            if (user == null)
            {
                ModelState.AddModelError("Email",
                    $"No user found with the email address '{model.Email}'!");
            }
            else if (role == null)
            {
                ModelState.AddModelError("Role",
                    $"The role '{model.Role}' does not exist!");
            }
            else if (!await UserManager.IsInRoleAsync(user.Id, role.Name))
            {
                ModelState.AddModelError("Role",
                    $"User '{user.Email}' has not been assigned the role '{role.Name}'!");
            }
            else
            {
                await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                TempData["Message"] =
                    $"User '{user.Email}' has been unassigned from the role '{role.Name}'!";
                return RedirectToAction("Index");
            }

            return View(model);
        }
        #endregion Assign Role
    }
}