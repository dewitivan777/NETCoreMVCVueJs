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
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage ="Maximum two Decimal Points allowed")]
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
