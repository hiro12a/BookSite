using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models.ViewModels
{
    public class OrderVM
    {
       
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
