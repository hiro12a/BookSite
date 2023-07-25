using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Book.Models.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOder { get; set; }
    }
}
