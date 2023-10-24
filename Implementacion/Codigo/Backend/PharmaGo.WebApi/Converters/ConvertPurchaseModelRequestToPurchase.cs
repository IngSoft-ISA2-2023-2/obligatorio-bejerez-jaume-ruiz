using PharmaGo.Domain.Entities;
using PharmaGo.WebApi.Models.In;

namespace PharmaGo.WebApi.Converters
{
    public class PurchaseModelRequestToPurchaseConverter
    {

        public Purchase Convert(PurchaseModelRequest model)
        {

            var purchase = new Purchase();
            purchase.PurchaseDate = model.PurchaseDate;
            purchase.BuyerEmail = model.BuyerEmail;
            purchase.details = new List<PurchaseDetail>();
            foreach (var detail in model.Details)
            {
                if (!string.IsNullOrEmpty(detail.DrugCode))
                {
                    purchase.details
                    .Add(new PurchaseDetail
                    {
                        Quantity = detail.Quantity,
                        Drug = new Drug() { Code = detail.DrugCode },
                        Pharmacy = new()
                        {
                            Id = detail.PharmacyId
                        }
                    });
                }
                else
                {
                    purchase.details
                    .Add(new PurchaseDetail
                    {
                        Quantity = detail.Quantity,
                        Product = new Product() { Code = detail.ProductCode },
                        Pharmacy = new()
                        {
                            Id = detail.PharmacyId
                        }
                    });
                }

            }

            return purchase;
        }

    }
}
