using eBlocksWeb.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace eBlocksWeb.Models
{
    public class Supplier
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string ContactTitle { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
    }
}
