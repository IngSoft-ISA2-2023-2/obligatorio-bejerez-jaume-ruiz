﻿using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class CreateProduct : IProductManager
    {
        public int UserId { get; set; }
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;
        Random random = new Random();

        public CreateProduct(IRepository<Pharmacy> pharmacyRepository,
                           IRepository<Session> sessionRespository,
                           IRepository<User> userRespository,
                           IRepository<Product> productRepository)
        {
            this._pharmacyRepository = pharmacyRepository;
            this._sessionRepository = sessionRespository;
            this._userRepository = userRespository;
            this._productRepository = productRepository;
        }

        public Product Create(Product product, string token)
        {
            User user = this.GetUserByToken(token);
            Pharmacy pharmacyOfProduct = this._pharmacyRepository.GetOneByExpression(p => p.Name == user.Pharmacy.Name);

            this.ValidateFieldsAreValid(product);

            this.ValidateProductPharmacy(pharmacyOfProduct);
            product.Pharmacy.Id = pharmacyOfProduct.Id;

            product.Code = this.GenerateProductCode(product);

            this._productRepository.InsertOne(product);
            this._productRepository.Save();

            return product;
        }

        private void ValidateProductPharmacy(Pharmacy pharmacyOfProduct)
        {
            if (pharmacyOfProduct == null)
            {
                throw new ResourceNotFoundException("The pharmacy of the product does not exist.");
            }
        }

        private User GetUserByToken(string token)
        {
            var guidToken = new Guid(token);
            Session session = this._sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = this._userRepository.GetOneDetailByExpression(u => u.Id == userId);
            return user;
        }

        private string GenerateProductCode(Product product)
        {
            string code = this.random.Next(10000, 99999) + "";
            while (this.CodeAlreadyExists(product))
            {
                code = this.random.Next(10000, 99999) + "";
            }
            return code;
        }

        private bool CodeAlreadyExists(Product product)
        {
            if (this._productRepository.Exists(p => p.Code == product.Code && p.Pharmacy.Name == product.Pharmacy.Name))
            {
                return true;
            }
            return false;
        }

        private void ValidateFieldsAreValid(Product product)
        {
            if (product.Name == null || product.Name == "" || product.Description == null || product.Description == "")
            {
                throw new InvalidResourceException("Fields are required");
            }
            if (product.Name.Length > 30)
            {
                throw new InvalidResourceException("Name can not have more than 30 characters");
            }
            if (product.Description.Length > 70)
            {
                throw new InvalidResourceException("Description can not have more than 70 characters");
            }
            if (product.Price <= 0)
            {
                throw new InvalidResourceException("Price must be greater than zero");
            }
        }

        public IEnumerable<Product> GetAll(ProductSearchCriteria productSearchCriteria)
        {
            Product productToSearch = new Product();
            if (productSearchCriteria.PharmacyId == null)
            {
                productToSearch.Name = productSearchCriteria.Name;
            }
            else
            {
                Pharmacy pharmacySaved = this._pharmacyRepository.GetOneByExpression(p => p.Id == productSearchCriteria.PharmacyId);
                if (pharmacySaved != null)
                {
                    productToSearch.Name = productSearchCriteria.Name;
                    productToSearch.Pharmacy = pharmacySaved;
                }
                else
                {
                    throw new ResourceNotFoundException("The pharmacy to get products of does not exist.");
                }
            }
            return this._productRepository.GetAllByExpression(productSearchCriteria.Criteria(productToSearch));
        }
    }
}