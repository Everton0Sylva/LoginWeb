using LoginWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
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

        public AuthToken Get(IdentityUser nUser)
        {
            var token = _context.AuthToken.FirstOrDefault(p => p.UserId == nUser.Id);
            return token;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<AuthToken> Post([FromBody] StringContent nUser)
        {
            IdentityUser user = new IdentityUser();
            var token = await setTokenAsync(user, "access_token");
            return token;
        }

        public async Task<IActionResult> GetToken(IdentityUser nUser, string tokenProvider, string purpose)
        {
            var token = await _userManager.GetAuthenticationTokenAsync(nUser, tokenProvider, purpose);

            return Ok(token);
        }



        public async Task<IdentityResult> PostToken(IdentityUser nUser, string tokenProvider, string purpose)
        {
            var newToken = Guid.NewGuid().ToString();
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
            var expires = DateTime.UtcNow.Subtract(new TimeSpan(expirein));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["TokenConfiguration:Issuer"],
                audience: _config["TokenConfiguration:Audience"],
                claims: claims, expires: expires,
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
