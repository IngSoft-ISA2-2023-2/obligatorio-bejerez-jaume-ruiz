using Microsoft.CodeAnalysis.CSharp.Syntax;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
    public class PurchaseDetailModelResponse
    {
        public int PurchaseId { get; set; }
        public int PurchaseDetailId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public PurchaseDetailModelResponse(int id, PurchaseDetail detail)
        {
            PurchaseId = id;
            PurchaseDetailId = detail.Id;
            Status = detail.Status;
            Price = detail.Price;
            Quantity = detail.Quantity;
            PharmacyId = detail.Pharmacy.Id;
            PharmacyName = detail.Pharmacy.Name;
            ItemCode = this.GetItemCode(detail);
            ItemName = this.GetItemName(detail);
        }

        private string? GetItemCode(PurchaseDetail detail)
        {
            if (detail.Drug != null)
            {
                return detail.Drug.Code;
            }
            else
            {
                return detail.Product?.Code;
            }
        }

        private string? GetItemName(PurchaseDetail detail)
        {
            if (detail.Drug != null)
            {
                return detail.Drug.Name;
            }
            else
            {
                return detail.Product?.Name;
            }
        }
    }
}
