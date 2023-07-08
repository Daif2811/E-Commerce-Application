using BuyOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuyOnline.Startup))]
namespace BuyOnline
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRoleAndUsers();
        }

        public void CreateRoleAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            IdentityRole role;

            ApplicationUser user = new ApplicationUser();


            if (!roleManager.RoleExists("Admins"))
            {
                role = new IdentityRole();
                role.Name = "Admins";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Seller"))
            {
                role = new IdentityRole();
                role.Name = "Seller";
                roleManager.Create(role);
            }


            if (!roleManager.RoleExists("Customer"))
            {
                role = new IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);
            }





            user.UserName = "Daif";
            user.Email = "di.hamdan@gmail.com";
            user.UserType = "Admins";

            var check = userManager.Create(user, "@Qwe12");
            if (check.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admins");
            }



        }
    }
}
