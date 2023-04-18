using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using ShoppingApp.Authentication;
using ShoppingApp.Dto;
using ShoppingApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingApp.Repos
{
    public interface UserRepo
    {
        Task<User> Register(RegisterDto userDto);
       Task<string> Login(LoginDto loginDto);
       Task Logout(HttpContext httpContext);

        //   Task Logout();
        //  Task<User> GetProfile(int userId);
        //   Task<int> GetUserId(string email);

    }

    public class UserRepository : UserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserRepository(ApplicationDbContext context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> Register(RegisterDto userDto)
        {
            // Check if user with same email already exists
            if (await _context.user.AnyAsync(u => u.Email == userDto.Email))
            {
                throw new ApplicationException("User with same email already exists");
            }

            if (userDto.Password != userDto.ConfirmPassword)
            {
                throw new ApplicationException("Password and Confirm Password do not match");
            }

            // Hash the password before saving to database
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);


            var userRole = await _context.userRoles.FirstOrDefaultAsync(r => r.RoleName == "User");

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = hashedPassword,
                UserRole = userRole, // Set the user's role to "User"
            };



            await _context.user.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }





        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _context.user.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new ApplicationException("Invalid email/password");
            }

            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("Jwt:Secret"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.UserRole.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }





        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync("Cookies");
            await httpContext.SignOutAsync("Bearer");
        }
       
        
    }
}
