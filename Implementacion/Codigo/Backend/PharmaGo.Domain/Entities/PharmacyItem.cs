using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.Entities
{
    public class PharmacyItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Pharmacy? Pharmacy { get; set; }
        public bool Deleted { get; set; }
    }
}
