using Microsoft.AspNetCore.Identity;
using Security.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> userManager);
    }
}
