using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UppgiftDatabas.Models
{
    internal class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public float SumCart { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<CartProduct> CartProduct { get; set; }
    }
}
