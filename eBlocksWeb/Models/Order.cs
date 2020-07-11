using System;
using System.ComponentModel.DataAnnotations;

namespace eBlocksWeb.Models
{
    public class Order
    {
        public string Id { get; set; }
        [Required]
        public string ProductId { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Maximum two Decimal Points allowed")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Input")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Range(0, 9999999999999999, ErrorMessage = "Invalid Input")]
        public int Quantity { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Maximum two Decimal Points allowed")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Input")]
        public decimal Discount { get; set; }
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Maximum two Decimal Points allowed")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Input")]
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string State { get; set; }
    }
}
