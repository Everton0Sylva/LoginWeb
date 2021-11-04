using LoginWeb.Data;
using LoginWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWeb.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ClaimsController> _logger;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        // GET: ClaimsController
        public ClaimsController()
        {
        }

        public ClaimsController(ILogger<ClaimsController> logger,
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

        public ActionResult PostClaim(string claimName, string claimRule, ClaimType claimType)
        {
            var claim = new AuthClaim();
            claim.Name = claimName;
            claim.Rule = claimRule;
            claim.Type = claimType;

            _context.AuthClaim.Add(claim);
            _context.SaveChanges();

            return Ok(claim);
        }

        /*
         * 
        public ActionResult PostClaim(IdentityUser nUser, string claimType, string claimValue)
        {
            var userClaim = new IdentityUserClaim<string>();

            userClaim.UserId = nUser.Id;
            userClaim.ClaimType = claimType;
            userClaim.ClaimValue = claimValue;

            _context.UserClaims.Add(userClaim);
            _context.SaveChanges();

            return Ok(userClaim);
        }
        */

        public ActionResult PostRole(IdentityUser nUser, string roleName = null, string roleNName = null)
        {
            var role = new IdentityRole();
            role.Name = roleName;
            role.NormalizedName = roleNName;

            _context.Roles.Add(role);
            _context.SaveChanges();

            return Ok(role);
        }


        public ActionResult PostUserRole(IdentityUser nUser, IdentityRole nRole)
        {
            var userClaim = new IdentityUserRole<string>();

            userClaim.UserId = nUser.Id;
            userClaim.RoleId = nRole.Id;

            _context.UserRoles.Add(userClaim);
            _context.SaveChanges();

            return Ok(userClaim);
        }

        public ActionResult PostRoleClaim(IdentityRole nRole, AuthClaim authClaim)
        {
            var roleClaim = new RoleClaim();

            roleClaim.RoleId = nRole.Id;
            roleClaim.ClaimId = authClaim.Id;

            _context.RoleClaims.Add(roleClaim);
            _context.SaveChanges();

            return Ok(roleClaim);
        }

        public ActionResult PostUserClaim(IdentityUser nUser, AuthClaim authClaim)
        {
            var userClaim = new UserClaim();

            userClaim.ClaimId = authClaim.Id;
            userClaim.UserId = nUser.Id;

            _context.UserClaim.Add(userClaim);
            _context.SaveChanges();

            return Ok(userClaim);
        }

        // GET: ClaimsController
        public ActionResult<IdentityRole> GetRole(string roleName)
        {
            var elem = _context.Roles.FirstOrDefault(p => p.Name.IndexOf(roleName) >= 0);
            if (elem == null)
            {
                return NotFound();
            }
            return elem;
        }

        public ActionResult<AuthClaim> GetClaim(string roleName)
        {
            var elem = _context.AuthClaim.FirstOrDefault(p => p.Name.IndexOf(roleName) >= 0);
            if (elem == null)
            {
                return NotFound();
            }
            return elem;
        }

    }
}
