using Microsoft.AspNetCore.Mvc;
using MvcPractice.Models;
using MvcPractice.Repository;
using System.Diagnostics;

namespace MvcPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            this.userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpGet]
        public IActionResult UserForm()
        {
            ViewBag.Countries = new List<string> { "USA", "Canada", "UK" };
            return View(new UserViewModel());
        }

        [HttpPost]
        public IActionResult UserForm(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ProfilePhoto != null)
                {
                    // Validate file size (max 2 MB)
                    if (model.ProfilePhoto.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ProfilePhoto", "The file size must not exceed 2 MB.");
                        ViewBag.Countries = new List<string> { "USA", "Canada", "UK" };
                        return View(model);
                    }

                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(model.ProfilePhoto.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ProfilePhoto", "Only JPEG and PNG files are allowed.");
                        ViewBag.Countries = new List<string> { "USA", "Canada", "UK" };
                        return View(model);
                    }

                    // Save the file (optional)
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    var filePath = Path.Combine(uploadsFolder, model.ProfilePhoto.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.ProfilePhoto.CopyTo(stream);
                    }

                    ViewBag.Message = "Form submitted successfully!";
                    return RedirectToAction("Success");
                }
            }

            ViewBag.Countries = new List<string> { "USA", "Canada", "UK" };
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
