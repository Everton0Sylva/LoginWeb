using LoginWeb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginWeb.Controllers
{
    public class TokenController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(ILogger<TokenController> logger,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IConfiguration config
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            _context = context;
        }

        public async Task<ActionResult> Post(IdentityUser nUser)
        {
            var loged = _signInManager.IsSignedIn(User);
            if (loged)
            {
                var token = await setTokenAsync(nUser, "access_token");
                return Ok(token);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Erro no Login...");
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> GetToken(IdentityUser nUser, string tokenProvider, string purpose)
        {
            var token = await _userManager.GetAuthenticationTokenAsync(nUser, tokenProvider, purpose);
            return Ok(token);            
        }



        public async Task<IdentityResult> PostToken(IdentityUser nUser, string tokenProvider, string purpose)
        {
            var newToken = await _userManager.GenerateUserTokenAsync(nUser, tokenProvider, purpose);
            //Set the new token for the user 
            return await _userManager.SetAuthenticationTokenAsync(nUser, tokenProvider, purpose, newToken);
        }

        public async Task<AuthToken> setTokenAsync(IdentityUser nUser, string tkType)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, nUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expire = _config["TokenConfiguration:Expire"];
            var expirein = short.Parse(expire);
            //var date = DateTime.Now()

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["TokenConfiguration:Issuer"],
                audience: _config["TokenConfiguration:Audience"],
                claims: claims, expires: DateTime.UtcNow.AddHours(expirein),
                signingCredentials: credenciais);

            var authtoken = new AuthToken()
            {
                UserId = nUser.Id,
                TokenType = tkType,
                TokenValue = new JwtSecurityTokenHandler().WriteToken(token),
                Expire = expirein
            };

            await _context.AuthToken.AddAsync(authtoken);
            await _context.SaveChangesAsync();
            return authtoken;
        }

    }
}
