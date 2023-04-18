using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Dto;
using ShoppingApp.Repos;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepo _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(UserRepo userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;


        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto userDto)
        {
            var user = await _userRepo.Register(userDto);
            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _userRepo.Login(loginDto);
                return Ok(new { token });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully" });
        }









    }
}
