using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Range(0, 1000) ]
        public int DisplayOrder { get; set; }
        

    }
}
