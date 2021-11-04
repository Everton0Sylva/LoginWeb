using LoginWeb.Data;
using LoginWeb.Models;
using LoginWeb.Views.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Project2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly TokenController tokenController;
        private readonly ClaimsController claimsController;

        public HomeController(ILogger<HomeController> logger,
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
            tokenController = new TokenController(null, signInManager, userManager, context, config);
            claimsController = new ClaimsController(null, signInManager, userManager, context, config);
        }

        public async Task<IActionResult> IndexAsync()
        {
            var loged = _signInManager.IsSignedIn(User);
            if (loged)
            {
                var user = await _userManager.GetUserAsync(User);
                var provider = Guid.NewGuid().ToString();
                var token = tokenController.GetToken(user, provider, "access_token");
             /*   if (token == null)
                {*/
                    var newToken = await tokenController.PostToken(user, "Microsoft", "access_token");
             //   }
                /*
              //  var claim = claimsController.PostClaim("vendasread", "Vendas I", ClaimType.Read);
                // var role = claimsController.PostRole(user, "vendaswriter", "Vendas II");
                var claim = claimsController.GetClaim("vendas").Value;
                var role = claimsController.GetRole("vendas").Value;
            //  var role = roles.FirstOrDefault(p => p == "vendaswriter");            
                var postUserRole = claimsController.PostUserRole(user, role);
                var postRoleClaim = claimsController.PostRoleClaim(role, claim);
                var postUserClaim = claimsController.PostUserClaim(user, claim);
            */

                return View();
            }
            return Error();
        }

        private IActionResult Page()
        {
            throw new NotImplementedException();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
