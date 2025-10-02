using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Controllers
{
    public class TestAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TestAuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Simple()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TestForm()
        {
            return Json(new { 
                success = true, 
                message = "Form received successfully",
                timestamp = DateTime.Now
            });
        }

        [HttpPost]
        public async Task<IActionResult> TestRegister(string fullName, string email, string password)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Json(new { success = true, message = "User created successfully!" });
                }
                else
                {
                    return Json(new { success = false, errors = result.Errors.Select(e => e.Description) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
