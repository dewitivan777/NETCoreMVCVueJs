using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        [RegularExpression("^http(s)?://([\\w-]+.)+[\\w-]+(/[\\w- ./?%&=])?$",ErrorMessage ="Invalid format")]
        public string Website { get; set; }
    }
}
