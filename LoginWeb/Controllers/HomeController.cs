using LoginWeb.Data;
using LoginWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
        IConfiguration config
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var loged = _signInManager.IsSignedIn(User);
            if (loged)
            {
                var user = await _userManager.GetUserAsync(User);
                /*
                var role = await _roleManager.FindByNameAsync("vendasjunior");
                if (role == null)
                {
                    role = new IdentityRole("vendasjunior");
                    role.NormalizedName = "Vendas Junior";

                    await _roleManager.CreateAsync(role);

                    await _roleManager.AddClaimAsync(role, new Claim("Cliente", "client.view"));
                    await _roleManager.AddClaimAsync(role, new Claim("Cliente", "client.create"));
                    await _roleManager.AddClaimAsync(role, new Claim("Cliente", "client.update"));
                }
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                var userRole = await _userManager.GetRolesAsync(user);
                for (int i = 0; i < userRole.Count; i++)
                {
                    var r = await _roleManager.FindByNameAsync(userRole[i]);
                    var claim = await _roleManager.GetClaimsAsync(r);

                }
                */
                //       var provider = _config["Jwt:key"];

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

                object token;
                using (var httpClient = new HttpClient())
                {
                    var id = "84e3d066-73ae-461f-b163-18023c7d7be7";

                    var postrequest = new StringContent(user.Id.ToString());


                    using (var response = await httpClient.PostAsync("https://localhost:44380/api/token/", postrequest))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        token = JsonConvert.DeserializeObject(apiResponse);
                    }

                }
                return View(token);

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
