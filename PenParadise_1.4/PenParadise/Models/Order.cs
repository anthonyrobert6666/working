
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace PenParadise.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Order
{

    public Order()
    {

        this.OrderDetails = new HashSet<OrderDetail>();

    }


    public int OrderID { get; set; }

    public string UserNameID { get; set; }

    public Nullable<System.DateTime> OrderDate { get; set; }

    public Nullable<double> Total { get; set; }

    public string DeliveryAddress { get; set; }

    public string PhoneContact { get; set; }



    public virtual User User { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; }

}

}
