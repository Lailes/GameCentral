using System;
using System.Threading.Tasks;
using GameCentral.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using GameCentral.Shared;
using Microsoft.AspNetCore.Authorization;

namespace GameCentral.API.Controllers {
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase {

        private UserManager<ApiUser> userManager;

        public AuthController(UserManager<ApiUser> userManager) {
            this.userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentials credentials) {
            var user = await userManager.FindByNameAsync(credentials.UserName);
            if (user == null || !await userManager.CheckPasswordAsync(user, credentials.Password)) return Forbid();
            
            var authClaims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SECRET_KEY));

            var token = new JwtSecurityToken(
                issuer: "http://dotnetdetail.net",
                audience: "http://dotnetdetail.net",
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        
        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] UserCredentials userCredentials) {
            var user = await userManager.FindByNameAsync(userCredentials.UserName);

            if (user != null) {
                return Problem(statusCode: 400, detail: "User already exists");
            }
            
            var apiUser = new ApiUser {
                UserName = userCredentials.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            var result = await userManager.CreateAsync(apiUser, userCredentials.Password);
            
            if (result.Succeeded) return CreatedAtAction("Register", userCredentials.UserName);
            
            var builder = new StringBuilder();
            foreach (var identityError in result.Errors) {
                builder.AppendLine(identityError.Description);
            }

            return Problem(statusCode: 400, detail: builder.ToString());
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] UserCredentials credentials) {
            var user = await userManager.FindByNameAsync(credentials.UserName);
            if (user == null) {
                return NotFound();
            }

            if (! await userManager.CheckPasswordAsync(user, credentials.Password)) {
                return Forbid();
            }

            if (userManager.Users.Count() == 1) {
                return Problem(statusCode: 400, detail: "One admin must exists");
            }
            
            await userManager.DeleteAsync(user);
            return Ok(new {name = credentials.UserName});
        }
    }
}