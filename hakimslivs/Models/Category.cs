using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace hakimslivs.Models
{
    public class Category
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
        public string Icon { get; set; }
    }
}
