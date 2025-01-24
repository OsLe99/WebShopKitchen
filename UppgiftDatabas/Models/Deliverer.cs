using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UppgiftDatabas.Models
{
    internal class Deliverer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
