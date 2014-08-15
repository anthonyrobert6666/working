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
        [StringLength(4, MinimumLength = 4, ErrorMessage = "UserNameID must be 4 characters!")]
        public string OrderDetailID { get; set; }
        [Required]
        public string OrderID { get; set; }
        [Required]
        public string ProductID { get; set; }
        [Required]
        public Nullable<int> Quantity { get; set; }
        [Required]
        public Nullable<double> UnitPrice { get; set; }
        [Required]
        public virtual Order Order { get; set; }
        [Required]
        public virtual Product Product { get; set; }
    }

}