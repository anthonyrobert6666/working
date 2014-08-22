﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(Product_Validation))]
    public partial class Product
    {

    }
    public class Product_Validation
    {
        [Required]
        [Display(Name = "ID")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "UserNameID must be 4 characters!")]
        public string ProductID { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string CategoryID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        [StringLength(50, MinimumLength = 4)]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Branch")]
        public string BranchID { get; set; }
        [StringLength(200, MinimumLength = 10)]
        public string Description { get; set; }
        [Required]
        public Nullable<double> Price { get; set; }
        [Required]
        public Nullable<int> Quantity { get; set; }
    }

}