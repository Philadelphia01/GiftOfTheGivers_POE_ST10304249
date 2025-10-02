using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult HelpCenter() => View("Page", model: "Help Center");
        public IActionResult WaysToGive() => View("Page", model: "Ways to Give");
        public IActionResult PhilanthropicGifts() => View("Page", model: "Philanthropic Gifts");
        public IActionResult DonateInHonor() => View("Page", model: "Donate in Honor");
        public IActionResult StartFundraiser() => View("Page", model: "Start a Fundraiser");

        public IActionResult StartApplication() => View("Page", model: "Start an Application");
        public IActionResult WhyJoin() => View("Page", model: "Why Join?");
        public IActionResult Handbook() => View("Page", model: "Handbook");
        public IActionResult PricingFees() => View("Page", model: "Pricing & Fees");

        public IActionResult OurServices() => View("Page", model: "Our Services");
        public IActionResult OurPartners() => View("Page", model: "Our Partners");
        public IActionResult CorporateGiftCards() => View("Page", model: "Corporate Gift Cards");
        public IActionResult GlobalGivingAtlas() => View("Page", model: "GlobalGiving Atlas");

        public IActionResult OurTeam() => View("Page", model: "Our Team");
        public IActionResult Jobs() => View("Page", model: "Jobs");
        public IActionResult FAQs() => View("Page", model: "FAQs");

        public IActionResult NonprofitResources() => View("Page", model: "Nonprofit Resources");
        public IActionResult CorporateGivingResources() => View("Page", model: "Corporate Giving Resources");
        public IActionResult DonorResources() => View("Page", model: "Donor Resources");
        public IActionResult SuccessStories() => View("Page", model: "Success Stories");
    }
}


