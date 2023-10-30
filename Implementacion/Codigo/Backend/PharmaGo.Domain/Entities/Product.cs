using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.Entities
{
    public class Product
    {
        public Product()
        {
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Deleted { get; set; }
        public string Description { get; set; }
        public Pharmacy? Pharmacy { get; set; }
    }
}
