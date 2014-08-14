using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(ProductType_Validation))]
    public partial class ProductType
    {

    }
    public class ProductType_Validation
    {
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "ProductTypeID must be 4 characters!")]
        public string ProducTypeID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string ProductTypeName { get; set; }
    }

}