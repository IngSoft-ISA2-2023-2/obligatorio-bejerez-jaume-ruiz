﻿namespace PharmaGo.Domain.Entities
{
    public class PurchaseDetail
    {

        public int Id { get; set; }
        public PharmacyItem Item { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public string Status { get; set; }
    }
}