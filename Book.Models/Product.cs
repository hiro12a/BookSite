using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Book.Models
{
    public class Product
    {
        // Identifier
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }

        // Price
        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }
        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }
        [Required]
        [DisplayName("Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        // Navigational property to Category Model
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        [Required]
        public Category Category { get; set; }

        // Since it contains many image in one product, need to put list here
        [ValidateNever]
        public List<ImageManager> ImageManagers { get; set; }
    }
}
