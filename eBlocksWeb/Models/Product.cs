using System.ComponentModel.DataAnnotations;

namespace eBlocksWeb.Models
{
    public class Product
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string CategoryId { get; set; }
        [Required]
        public int QuantityPerUnit { get; set; } = 0;
        [Required]
        public decimal UnitPrice { get; set; } = 0;
        [Required]
        public int UnitsInStock { get; set; } = 0;
        [Required]
        public int ReorderLevel { get; set; } = 0;
        public bool Discontinued { get; set; }
    }
}
