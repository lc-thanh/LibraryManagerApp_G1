using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using LibraryManagerApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserService _userService;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userService = new UserService();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            if (login == null)
            {
                return BadRequest();
            }

            // Find user with provided Email
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(login.Email);

            if (user == null)
            {
                return NotFound("Cannot find user with provided Email!");
            }

            // Check password
            if (!_userService.VerifyPassword(user.Password, login.Password))
            {
                return Unauthorized();
            }

            // Check Role
            switch (user.Role)
            {
                case Data.Enum.RoleEnum.Admin:
                    {
                        var token = GenerateJwtToken(user.Email, "Admin");
                        return Ok(new { token });
                    }

                case Data.Enum.RoleEnum.Librarian:
                    {
                        var token = GenerateJwtToken(user.Email, "Librarian");
                        return Ok(new { token });
                    }

                case Data.Enum.RoleEnum.Member:
                    {
                        var token = GenerateJwtToken(user.Email, "Member");
                        return Ok(new { token });
                    }

                default:
                    return BadRequest("Something wrong while logging!");
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> signUp([FromBody] UserSignUp signUp)
        {
            if (signUp == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailcheck = await _unitOfWork.UserRepository.GetByEmailAsync(signUp.Email);

            if (emailcheck != null)
            {
                return Conflict(new { message = "Email already exists!" });
            }

            Member memberToCreate = new Member
            {
                FullName = signUp.FullName,
                Email = signUp.Email,
                Phone = signUp.Phone,
                Password = _userService.HashPassword(signUp.Password),
                Role = Data.Enum.RoleEnum.Member,
            };

            _unitOfWork.MemberRepository.Add(memberToCreate);

            var saved = await _unitOfWork.SaveChangesAsync();

            if (saved > 0)
            {
                return Ok(memberToCreate);
            }

            return BadRequest("Something wrong while signing up!");
        }

        private string GenerateJwtToken(string userEmail, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("group1VerySecretKey123!"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role) // Thêm role vào claims
            };

            var token = new JwtSecurityToken(
                issuer: "myIssuer",
                audience: "myAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
