using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(OrderDetail_Validation))]
    public partial class OrderDetail
    {

    }
    public class OrderDetail_Validation
    {
        [Required]
        [Display(Name = "ID")]
        public string OrderDetailID { get; set; }
        [Required]
        [Display(Name = "Order")]
        public string OrderID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductID { get; set; }
        [Required]
        public Nullable<int> Quantity { get; set; }
        [Required]
        [Display(Name = "Price")]
        public Nullable<double> UnitPrice { get; set; }
        [Required]
        public virtual Order Order { get; set; }
        [Required]
        public virtual Product Product { get; set; }
    }

}