using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.DataAccess;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using System.Collections;
using System.Xml.Linq;
using PharmaGo.WebApi.Models.Out;
using static PharmaGo.WebApi.Models.In.PurchaseModelRequest;

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreatePurchaseStepDefinitions
    {
        private readonly IPurchasesManager _purchasesManager;
        private PurchasesController _purchasesController;

        private readonly IRepository<Purchase> _purchasesRepository;
        private readonly IRepository<Pharmacy> _pharmacysRepository;
        private readonly IRepository<Drug> _drugsRepository;
        private readonly IRepository<PurchaseDetail> _purchaseDetailRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;

        private readonly PharmacyGoDbContext _dbContext;
        private DbContextOptions<PharmacyGoDbContext> _options;
        private PurchaseModelRequest _newPurchaseRequest;
        private PurchaseDetailModelRequest _newProductDetail;
        private PurchaseModelResponse? _purchaseModelResponse;
        private Product _product;

        public CreatePurchaseStepDefinitions()
        {
            var connectionString = "Server=.\\;Database=PharmaGo;Trusted_Connection=True; MultipleActiveResultSets=True";
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyGoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            this._options = optionsBuilder.Options;
            this._dbContext = new PharmacyGoDbContext(this._options);

            this._purchasesRepository = new PurchasesRepository(this._dbContext);
            this._pharmacysRepository = new PharmacyRepository(this._dbContext);
            this._drugsRepository = new DrugRepository(this._dbContext);
            this._purchaseDetailRepository = new PurchasesDetailRepository(this._dbContext);
            this._sessionRepository = new SessionRepository(this._dbContext);
            this._userRepository = new UsersRepository(this._dbContext);
            this._productRepository = new ProductRepository(this._dbContext);

            this._purchasesManager = new PurchasesManager(this._purchasesRepository, this._pharmacysRepository, this._drugsRepository, this._purchaseDetailRepository, this._sessionRepository, this._userRepository);
            this._purchasesController = new PurchasesController(this._purchasesManager);
        }

        [Given(@"I am an anonymous user and select products")]
        public void GivenIAmAnAnonymousUserAndSelectProducts()
        {
            this._newPurchaseRequest = new PurchaseModelRequest()
            {
                BuyerEmail = "anonymous@gmail.com",
                PurchaseDate = DateTime.Now,
                Details = new List<PurchaseDetailModelRequest>(),
            };
        }

        [When(@"I buy existing products")]
        public void WhenIBuyExistingProducts()
        {
            this.InitializeProduct();
            this._newProductDetail = new PurchaseDetailModelRequest()
            {
                DrugCode = "",
                PharmacyId = 1,
                ProductCode = "1234567",
                Quantity = 1,
            };
            this._newPurchaseRequest.Details.Add(this._newProductDetail);
        }

        private void InitializeProduct()
        {
            var pharmacySaved = this._pharmacysRepository.GetOneByExpression(p => p.Id == 1);
            this._product = new Product()
            {
                Name = "nombre",
                Code = "1234567",
                Price = 200,
                Deleted = false,
                Description = "Description",
                Pharmacy = pharmacySaved
            };
            this._productRepository.InsertOne(this._product);
            this._productRepository.Save();
            this._product = this._productRepository.GetOneByExpression(p => p.Code == this._product.Code);
        }

        [Then(@"the purchase is successful")]
        public void ThenThePurchaseIsSuccessful()
        {
            var result = this._purchasesController.CreatePurchase(this._newPurchaseRequest);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            if (result is ObjectResult actionResult)
            {
                this._purchaseModelResponse = (PurchaseModelResponse)actionResult.Value;
                bool isInPurchase = this._purchaseModelResponse.Details.Any(d => d.Code.Equals(this._newProductDetail.ProductCode));
                isInPurchase.Should().BeTrue();

                foreach (var purchaseDetail in this._purchaseModelResponse.Details)
                {
                    var purchaseDetailSaved = this._purchaseDetailRepository.GetOneByExpression(p => p.Id == purchaseDetail.Id);
                    this._purchaseDetailRepository.DeleteOne(purchaseDetailSaved);
                    this._purchaseDetailRepository.Save();
                }
                var purchaseSaved = this._purchasesRepository.GetOneByExpression(p => p.Id == this._purchaseModelResponse.Id);
                this._purchasesRepository.DeleteOne(purchaseSaved);
                this._purchasesRepository.Save();
            }

            var productSaved = this._productRepository.GetOneByExpression(p => p.Id == this._product.Id);
            this._productRepository.DeleteOne(productSaved);
            this._productRepository.Save();

            statusCode.Should().Be(200);
        }

        [When(@"I buy unexisting products")]
        public void WhenIBuyUnexistingProducts()
        {
            this._newProductDetail = new PurchaseDetailModelRequest()
            {
                DrugCode = "",
                PharmacyId = 1,
                ProductCode = "-1",
                Quantity = 1,
            };
            this._newPurchaseRequest.Details.Add(this._newProductDetail);
        }

        [Then(@"the purchase is not successful")]
        public void ThenThePurchaseIsNotSuccessful()
        {
            var statusCode = 200;
            try
            {
                var result = this._purchasesController.CreatePurchase(this._newPurchaseRequest);
            }
            catch (ResourceNotFoundException)
            {
                statusCode = 400;
            }

            statusCode.Should().Be(400);
        }

    }
}