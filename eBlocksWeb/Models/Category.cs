using System.ComponentModel.DataAnnotations;

namespace eBlocksWeb.Models
{
    public class Category
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
