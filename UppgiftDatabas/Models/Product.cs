using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UppgiftDatabas.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public int DeliveryId { get; set; }
        public int CategoryId { get; set; }
        public bool Chosen { get; set; }

        public virtual Deliverer Deliverer { get; set; }
        public virtual Category Category { get; set; }
    }
}
