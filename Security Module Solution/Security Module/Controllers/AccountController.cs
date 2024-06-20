using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Entities.Identity;
using Security.Core.Services;
using Security_Module.DTOs;

namespace Security_Module.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase



    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        //register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var User = new AppUser()
            {
                Email = model.Email,
                DisplayName = model.DisplayName,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var Result = await _userManager.CreateAsync(User,model.Password);

            if (!Result.Succeeded)
            {
                return BadRequest(ModelState);
            }

            var RetutnedUser = new UserDto()
            {

                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            };
            return RetutnedUser;
        }

        //login

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }


    }
}
