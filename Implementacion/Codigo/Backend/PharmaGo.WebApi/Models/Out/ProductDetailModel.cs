using PharmaGo.Domain.Entities;
using PharmaGo.Domain.Enums;

namespace PharmaGo.WebApi.Models.Out
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public PharmacyBasicModel Pharmacy { get; set; }

        public ProductDetailModel(Product product)
        {
            this.Id = product.Id;
            this.Code = product.Code;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Price = product.Price;
            this.Pharmacy = new PharmacyBasicModel(product.Pharmacy);
        }
    }
}
