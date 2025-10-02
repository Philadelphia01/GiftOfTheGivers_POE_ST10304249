using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    public class ProgramsController : Controller
    {
        public IActionResult FoodSecurity()
        {
            return View();
        }

        public IActionResult Education()
        {
            return View();
        }

        public IActionResult Healthcare()
        {
            return View();
        }
    }
}


