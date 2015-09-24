using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UrfIdentity.Models;


namespace Identity.UI.Extensions
{
    public static class ApplicatiuonUserExtension
    {
        public static IEnumerable<SelectListItem> ToSelectItemList(
            this IEnumerable<ApplicationUser> users)
        {
            return users.Select(
                u => new SelectListItem
                {
                    Text = $"{u.UserDetails.LastName}, {u.UserDetails.FirstName}",
                    Value = u.Id.ToString()
                });
        }
    }
}