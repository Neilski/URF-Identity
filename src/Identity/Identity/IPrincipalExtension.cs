using System.Linq;
using System.Security.Principal;


namespace Identity
{
    public static class IPrincipalExtension
    {
        /// <summary>
        /// Checks if a User IS in one role and NOT in another role
        /// </summary>
        /// <param name="user">IPricipal object</param>
        /// <param name="isRole">Is in this role</param>
        /// <param name="notRole">Not in this role</param>
        /// <returns>True if the user satisfies the role assignments; otherwise false</returns>
        public static bool IsAndNot(this IPrincipal user, string isRole, string notRole)
        {
            return (user.IsInRole(isRole) && !user.IsInRole(notRole));
        }


        /// <summary>
        /// Checks if a User IS in all roles and NOT in another roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isRoles">User must be in these roles</param>
        /// <param name="notRoles">User must not be in these roles</param>
        /// <returns>True if the user satisfies the role assignments; otherwise false</returns>
        public static bool IsAndNot(this IPrincipal user, string [] isRoles, string [] notRoles)
        {
            if (isRoles != null)
            {
                if (isRoles.Any(role => !user.IsInRole(role)))
                {
                    return false;
                }
            }

            if (notRoles != null)
            {
                return notRoles.All(role => !user.IsInRole(role));
            }

            return true;
        }
    }
}