using Microsoft.AspNetCore.Mvc;

namespace JOBProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        IConfiguration configuration;
        public AdminController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
