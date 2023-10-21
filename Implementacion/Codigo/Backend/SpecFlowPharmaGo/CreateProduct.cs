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
            if (name == null || name == "" || description == null || description == "")
            {
                throw new InvalidResourceException("Fields are required");
            }
            if (name.Length > 30)
            {
                throw new InvalidResourceException("Name can not have more than 30 characters");
            }
            if (description.Length > 70)
            {
                throw new InvalidResourceException("Description can not have more than 70 characters");
            }
            if (price <= 0)
            {
                throw new InvalidResourceException("Price must be greater than zero");
            }
            return true;
        }
    }
}