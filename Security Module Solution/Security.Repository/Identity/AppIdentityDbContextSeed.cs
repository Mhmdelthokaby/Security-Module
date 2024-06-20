using Microsoft.AspNetCore.Identity;
using Security.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Security.Repository.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Mohamed Emad",
                    Email = "mohamedemad@gmail.com",
                    UserName = "mhmdemad",
                    PhoneNumber = "1234567890"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
