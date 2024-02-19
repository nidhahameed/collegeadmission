using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CollegeAdmission.Models
{
    public class UserProfile
    {
        public int UserID { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; }
        
        public string Gender { get; set; }

        public string FatherName { get; set; }
        
        public string MotherName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
        
        public string Address { get; set; }

        public string State { get; set; }

        public string District { get; set; }

        public string School { get; set; }

        public float Marks { get; set; }

        public string Course { get; set; }

        public string Department { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }


    }
}