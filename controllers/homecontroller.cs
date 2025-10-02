using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Data;
using Microsoft.Extensions.Logging;

namespace DisasterAlleviationFoundation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"User authenticated: {User.Identity.IsAuthenticated}");
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Redirecting to register");
                return RedirectToPage("/Account/Register", new { area = "Identity" });
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TestDb()
        {
            try
            {
                using (var scope = HttpContext.RequestServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var canConnect = dbContext.Database.CanConnect();
                    return Json(new { 
                        success = true, 
                        canConnect = canConnect,
                        connectionString = "Connected to database successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { 
                    success = false, 
                    error = ex.Message,
                    connectionString = "Failed to connect to database"
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
