using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;

namespace SpecFlowPharmaGo
{
    public class CreateProduct
    {
        public int UserId { get; set; }

        Random random = new Random();
        public Product CreateNewProduct(string name, string description, decimal price)
        {
            Product? product = null;
            if (this.IsUserEmployee(this.UserId))
            {
                if (this.AreFieldsValid(name, description, price))
                {
                    product = new Product(name, description, price);
                    product.Code = this.GenerateProductCode();
                }
            }

            return product;
        }

        private bool IsUserEmployee(int userId)
        {
            return true;
        }

        private string GenerateProductCode()
        {
            string code = this.random.Next(10000, 99999) + "";
            while (this.CodeAlreadyExists(code))
            {
                code = this.random.Next(10000, 99999) + "";
            }
            return code;
        }

        private bool CodeAlreadyExists(string code)
        {
            return false;
        }

        private bool AreFieldsValid(string name, string description, decimal price)
        {
            return true;
        }
    }
}