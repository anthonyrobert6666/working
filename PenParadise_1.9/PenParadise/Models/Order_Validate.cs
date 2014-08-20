using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PenParadise.Models
{
    [MetadataType(typeof(Order_Validation))]
    public partial class Order
    {

    }
    public class Order_Validation
    {
        [Required]
        public string OrderID { get; set; }

        [Required]
        public string UserNameID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public string OrderDate { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid Phone Number!")]
        public string PhoneContact { get; set; }

        [Required]
        public Nullable<int> Total { get; set; }

        [Required]
        public virtual User User { get; set; }
    }

}