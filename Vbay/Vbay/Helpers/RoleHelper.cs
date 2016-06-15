using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vbay.Models;

namespace Vbay.Helpers
{
    public class RoleHelper
    {
        internal static void SeedEntities(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(RoleNames.ROLE_ADMIN))
            {
                var roleResult = roleManager.Create(new IdentityRole(RoleNames.ROLE_ADMIN));
            }

            string userName = "gflynn@volusia.org";
            ApplicationUser user = userManager.FindByEmail(userName);
            var result = userManager.AddToRole(user.Id, RoleNames.ROLE_ADMIN);

        }
    }
}