using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }
        public int ProdId { get; set; }
        [ForeignKey(nameof(ProdId))]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1,1000, ErrorMessage = "Count must be between 1-1000")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped] // WIll not create the column in database
        public double Price { get; set; }
    }
}
