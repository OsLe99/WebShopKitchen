using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UppgiftDatabas.Models
{
    internal class CartProduct
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product Product { get; set; }
    }
}
