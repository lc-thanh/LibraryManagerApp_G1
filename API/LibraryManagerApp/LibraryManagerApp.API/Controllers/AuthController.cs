using LibraryManagerApp.Data.Dto;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using LibraryManagerApp.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagerApp.API.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _userService = new UserService();
        }

        //Tham khảo: https://www.c-sharpcorner.com/article/jwt-authentication-with-refresh-tokens-in-net-6-0/

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            if (login == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            JwtSecurityToken token;
            // Check Role
            switch (user.Role)
            {
                case Data.Enum.RoleEnum.Admin:
                    {
                        token = GenerateJwtToken(user.Email, "Admin");
                        break;
                    }

                case Data.Enum.RoleEnum.Librarian:
                    {
                        token = GenerateJwtToken(user.Email, "Librarian");
                        break;
                    }

                case Data.Enum.RoleEnum.Member:
                    {
                        token = GenerateJwtToken(user.Email, "Member");
                        break;
                    }

                default:
                    return BadRequest("Something wrong while logging!");
            }

            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            _unitOfWork.UserRepository.Update(user);
            var saved = await _unitOfWork.SaveChangesAsync();
            if (saved > 0)
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,
                    Role = user.Role
                });

            return BadRequest("Something wrong while logging!");
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
                return Ok(new { Result = "User registered successfully!" });
            }

            return BadRequest("Something wrong while signing up!");
        }

        private JwtSecurityToken GenerateJwtToken(string userEmail, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            _ = int.TryParse(_configuration["JWT:AccessTokenValidityInMinutes"], out int tokenValidityInMinutes);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role) // Thêm role vào claims
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                signingCredentials: credentials
            );

            return token;
        }

        private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            _ = int.TryParse(_configuration["JWT:AccessTokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                signingCredentials: credentials
            );

            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Cannot Get Principal From Expired Token");
            }

            string userEmail = principal.Identity.Name;

            var user = await _unitOfWork.UserRepository.GetByEmailAsync(userEmail);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = GenerateJwtToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
