using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

namespace CollegeAdmission.Models
{
    public class UserClass
    {
        [Key]
        public string UserID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage ="Enter first name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name should be min 3 and max 20 length")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter last name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Last name should be min 1 and max 20 length")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pick a date")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }

        [Required(ErrorMessage = "Select your gender")]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your father's name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Father name should be min 4 and max 20 length")]
        public string FatherName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter your mother's name")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Mother name should be min 4 and max 20 length")]
        public string MotherName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter phone number")]
        [StringLength(10, ErrorMessage = "Phone number must contains 10 characters", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter email address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please provide valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your address")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Select your state")]
        public string State { get; set; }

        [Required(ErrorMessage = "Select your district")]
        public string District { get; set; }

        [Required(ErrorMessage = "Enter your previous school")]
        [StringLength(200)]
        public string School { get; set; }

        [Required(ErrorMessage = "Enter your last academic percentage")]
        [RegularExpression("^(100\\.00|100\\.0|100)|([0-9]{1,2}){0,1}(\\.[0-9]{1,2}){0,1}$", ErrorMessage = "Please Provide Percentage")]
        public float Marks { get; set; }

        [Required(ErrorMessage = "Select a course")]
        public string Course { get; set; }

        [Required(ErrorMessage = "Select a department")]
        public string Department { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter username")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please provide valid username")]
        [Compare("Email", ErrorMessage = "Email and username must me same")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]       
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Enter password")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirma password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Image { get; set; }

        public string EncryptedPassword { get; set; }
        public string DecryptedPassword { get; set; }

        public string Status { get; set; }
    }
}