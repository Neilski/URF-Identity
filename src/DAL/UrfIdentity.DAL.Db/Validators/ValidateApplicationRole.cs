using System;
using System.Data.Entity.Validation;
using System.Linq;
using Identity.Resources;
using Resources;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Db.Validators
{
    public static partial class UrfIdentityDataContextExtension
    {
        public static void
            ValidateApplicationRole(
            this UrfIdentityDataContext dbContext,
            DbEntityValidationResult result)
        {
            var entity = result.Entry.Entity as ApplicationRole;
            if (entity == null)
            {
                return;
            }

            ApplicationRole temp = dbContext
                .ApplicationRoles.FirstOrDefault(x => x.Name == entity.Name);

            if ((temp != null) && (temp.Id != entity.Id))
            {
                result.ValidationErrors.Add(
                    new DbValidationError(
                        // A {0} with the {1} of '{2}' is already registered ({3})
                        "Name",
                        String.Format(
                            ModelValidationResources.NonUniqueField_NoReference,
                            AccountResources.User_Role,
                            AccountResources.Name, entity.Name)));
            }
        }
    }
}