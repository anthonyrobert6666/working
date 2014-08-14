using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(Branch_Validation))]
    public partial class Branch
    {

    }
    public class Branch_Validation
    {
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "UserNameID must be 4 characters!")]
        public string BranchID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string BranchName { get; set; }
    }
}