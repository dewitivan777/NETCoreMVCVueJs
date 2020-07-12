using eBlocksWeb.Models.Attributes;
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
        [DecimalPrecision(2)]
        [Range(0, 9999999999999999.99, ErrorMessage ="Invalid Input")]
        public decimal UnitPrice { get; set; } = 0;
        [Required]
        [Range(0, 9999999999999999, ErrorMessage = "Invalid Input")]
        public int UnitsInStock { get; set; } = 0;
        [Required]
        [Range(0, 9999999999999999, ErrorMessage = "Invalid Input")]
        public int ReorderLevel { get; set; } = 0;
        public bool Discontinued { get; set; }
    }
}
