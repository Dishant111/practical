using System.ComponentModel.DataAnnotations;

namespace MvcPractice.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }

        [Range(18, 99, ErrorMessage = "Age must be between 18 and 99")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        public bool SubscribeToNewsletter { get; set; }

        [Url(ErrorMessage = "Invalid URL")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Comments are required")]
        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Profile photo is required")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only JPEG and PNG files are allowed.")]
        public IFormFile ProfilePhoto { get; set; }
    }


}
