namespace Identity.Db
{
    public class IdentityDbInitializer
        : CreateDatabaseIfNotExists<UrfIdentityDbContext>
    {
        protected override void Seed(UrfIdentityDbContext context)
        {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }


        public static void InitializeIdentityForEf(UrfIdentityDbContext db)
        {
            // var store = new UserStore<ApplicationUser>(db);
            // var userManager = new UserManager<ApplicationUser>(store);
            // var roleManager =
            //     new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            // const string name = "neil.martin@abilitation.com";
            // const string email = "neil.martin@abilitation.com";
            // const string password = "bcFCsYnbOX02";
            // const string roleName = "SysAdmin";
            // 
            // //Create Role Admin if it does not exist
            // var role = roleManager.FindByName(roleName);
            // if (role == null)
            // {
            //     role = new IdentityRole(roleName);
            //     roleManager.Create(new IdentityRole(roleName));
            // }
            // 
            // var user = userManager.FindByName(name);
            // if (user == null)
            // {
            //     user = new ApplicationUser
            //     {
            //         UserName = name,
            //         Email = email,
            //         UserDetails = new ApplicationUserDetails
            //         {
            //             FirstName = "Neil",
            //             LastName = "Martin",
            //             Company = "Abilitation Limited",
            //             JobTitle = "Developer",
            //             Phone = "+441689370890"
            //         }
            //     };
            //     userManager.Create(user, password);
            // }
            // 
            // // Add user admin to Role Admin if not already added
            // var rolesForUser = userManager.GetRoles(user.Id);
            // if (!rolesForUser.Contains(role.Name))
            // {
            //     userManager.AddToRole(user.Id, role.Name);
            // }
        }
    }
}