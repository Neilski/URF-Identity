using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UrfIdentity.Models.ViewModels;


namespace Identity.UI.Extensions
{
    public static class ApplicatiuonUserViewModelExtension
    {
        public static IEnumerable<SelectListItem> ToSelectItemList(
            this IEnumerable<ApplicationUserViewModel> users)
        {
            return users.Select(
                u => new SelectListItem
                {
                    Text = $"{u.LastName}, {u.FirstName}",
                    Value = u.UserId.ToString()
                });
        }
    }
}