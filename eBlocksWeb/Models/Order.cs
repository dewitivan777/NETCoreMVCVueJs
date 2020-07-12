using eBlocksWeb.Models.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace eBlocksWeb.Models
{
    public class Order
    {
        public string Id { get; set; }
        [Required]
        public string ProductId { get; set; }
        [DecimalPrecision(2)]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Input")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Range(1, 9999999999999999, ErrorMessage = "Invalid Input")]
        public int Quantity { get; set; }
        [DecimalPrecision(2)]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Input")]
        public decimal Discount { get; set; }
        [DecimalPrecision(2)]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Input")]
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public DateTime UpdateDate { get; set; } = DateTime.MinValue;
        public string State { get; set; }
    }
}
