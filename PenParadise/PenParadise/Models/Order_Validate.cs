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
        [StringLength(4, MinimumLength = 4, ErrorMessage = "UserNameID must be 4 characters!")]
        public string OrderID { get; set; }

        [Required]
        public string UserNameID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public string OrderDate { get; set; }

        [Required]
        public Nullable<int> Total { get; set; }
        [Required]
        public virtual User User { get; set; }
    }

}