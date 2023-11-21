using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hakimslivs.Models
{
    public class Item
    {
        public int ID { get; set; }
        [Display(Name = "Kategori"), Column("Category")]
        public Category Category { get; set; }
        [MaxLength(50), Display(Name = "Produkt"), Required]
        public string Product { get; set; }
        [Column(TypeName = "decimal(7, 2)"), Display(Name = "Pris")]
        public decimal Price { get; set; }
        [Display(Name = "Lagersaldo")]
        public int Stock { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "URL till Bild"), Required]
        public string ImageURL { get; set; }

    }



}
