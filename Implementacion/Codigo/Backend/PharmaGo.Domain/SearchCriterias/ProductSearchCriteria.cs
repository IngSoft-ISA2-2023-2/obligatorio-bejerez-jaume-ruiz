using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.SearchCriterias
{
    public class ProductSearchCriteria
    {
        public string? Name { get; set; }
        public int? PharmacyId { get; set; }

        public Expression<Func<Product, bool>> Criteria(Product product)
        {
            if (!string.IsNullOrEmpty(this.Name) && this.PharmacyId != null)
            {
                return p => p.Name == product.Name && !p.Deleted && p.Pharmacy == product.Pharmacy;
            }
            else if (string.IsNullOrEmpty(this.Name) && this.PharmacyId != null)
            {
                return p => p.Pharmacy == product.Pharmacy && !p.Deleted;
            }
            else if (!string.IsNullOrEmpty(this.Name) && this.PharmacyId == null)
            {
                return p => p.Name == product.Name && !p.Deleted;
            }
            else
            {
                return p => p.Name == p.Name && !p.Deleted;
            }
        }
    }
}
