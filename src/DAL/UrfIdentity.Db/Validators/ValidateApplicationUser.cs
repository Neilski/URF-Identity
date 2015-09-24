using System;
using System.Data.Entity.Validation;
using System.Linq;
using Identity.Models;
using Identity.Resources;
using Resources;


namespace UrfIdentity.Db.Validators
{
    public static partial class InsightDataContextExtension
    {
        public static void
            ValidateApplicationUser(
            this UrfIdentityDataContext dbContext,
            DbEntityValidationResult result)
        {
            var entity = result.Entry.Entity as ApplicationUser;
            if (entity == null)
            {
                return;
            }

            ApplicationUser temp = dbContext
                .ApplicationUsers.FirstOrDefault(x => x.UserName == entity.UserName);

            if ((temp != null) && (temp.Id != entity.Id))
            {
                result.ValidationErrors.Add(
                    new DbValidationError(
                        // A {0} with the {1} of '{2}' is already registered ({3})
                        "UserName",
                        String.Format(
                            ModelValidationResources.NonUniqueField_NoReference,
                            AccountResources.User_Account,
                            AccountResources.UserName, entity.UserName)));
            }
        }
    }
}