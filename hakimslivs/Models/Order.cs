using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hakimslivs.Models
{
    public class Order
    {
        public int ID { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime OrderDate { get; set; }

        [ForeignKey("AspNetUserId")]
        [Display(Name = "CustomerID")]
        public ApplicationUser User { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool PaymentOk { get; set; }
    }
}
