using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(Category_Validation))]
    public partial class Category
    {

    }
    public class Category_Validation
    {
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "CategoryID must be 4 characters!")]
        public string CategoryID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string CategoryName { get; set; }

        [Required]
        public string ProductTypeID { get; set; }
        [Required]
        public virtual ProductType ProductType { get; set; }
    }

}