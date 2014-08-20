using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(User_Validation))]
    public partial class User
    {

    }
    public class User_Validation
    {
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "UserNameID must be 4 characters!")]
        public string UserNameID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string FullName { get; set; }

        [Required]
        public string Birthday { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is is not valid.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Address { get; set; }

        [Required]
        [StringLength(10)]        
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Phone Number!")]
        public string Phone { get; set; }
    }

}