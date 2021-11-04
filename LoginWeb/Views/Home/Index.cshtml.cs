using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Project2.Models;
using System.Threading.Tasks;

namespace LoginWeb.Views.Home
{
    public class IndexModel : PageModel
    {
        public AuthToken authToken { get; set; }

        public IndexModel(AuthToken _authToken)
        {
            authToken = _authToken;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            var token = authToken;

          //  return RedirectToRoute("./Lockout");
        }

    }
}
