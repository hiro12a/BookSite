using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models
{
    public class ImageManager
    {
        public int Id { get; set; }

        [Required] 
        public string ImageUrl { get; set; }


        // One to many relation, single product has many image 
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
    }
}
