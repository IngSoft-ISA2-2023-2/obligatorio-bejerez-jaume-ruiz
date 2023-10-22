using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.Entities
{
    public class Product : PharmacyItem
    {
        public Product()
        {
        }

        public Product(string name, string description, decimal price)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
        }
        public string Description { get; set; }
    }
}
