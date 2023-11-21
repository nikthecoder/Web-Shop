using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hakimslivs.Models
{
    [Keyless]
    public class ItemQuantity
    {
        public int ID { get; set; }
        public Item Item { get; set; }
        public Order Order { get; set; }
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public int Quantity { get; set; }
    }
}
