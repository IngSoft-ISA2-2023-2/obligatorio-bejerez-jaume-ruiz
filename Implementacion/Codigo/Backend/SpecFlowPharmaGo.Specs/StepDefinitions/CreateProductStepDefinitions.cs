using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;
using System.Collections;
using System.Xml.Linq;

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreateProductStepDefinitions
    {
        private readonly IProductManager _productManager;
        private ProductController _productController;
        private ProductDetailModel _productDetailModel;
        private ProductModel _productModel;
        private ProductSearchCriteria _productSearch;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly PharmacyGoDbContext _dbContext;
        private DbContextOptions<PharmacyGoDbContext> _options;

        public CreateProductStepDefinitions()
        {
            var connectionString = "Server=.\\;Database=PharmaGo;Trusted_Connection=True; MultipleActiveResultSets=True";
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyGoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            this._options = optionsBuilder.Options;
            this._dbContext = new PharmacyGoDbContext(this._options);
            this._pharmacyRepository = new PharmacyRepository(this._dbContext);
            this._sessionRepository = new SessionRepository(this._dbContext);
            this._userRepository = new UsersRepository(this._dbContext);
            this._productRepository = new ProductRepository(this._dbContext);
            this._productManager = new ProductManager(this._pharmacyRepository, this._sessionRepository, this._userRepository, this._productRepository);
        }


        [Given("the user logged with token (.*) is an employee")]
        public void GivenTheUserIsAnEmployee(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;
            this._productController = new ProductController(this._productManager)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
        }

        [When("name (.*), description (.*) and price (.*) are entered for the new product")]
        public void WhenNameDescriptionAndPriceAreEnteredForTheNewProduct(string name, string description, decimal price)
        {
            this._productSearch = new ProductSearchCriteria { PharmacyId = 1, Name = name };
            this._productModel = new ProductModel() { Name = name, Description = description, Price = price, PharmacyName = "" };
        }

        [Then("creation should be successful")]
        public void ThenCreationShouldBeSuccessful()
        {
            var result = this._productController.Create(this._productModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            if (result is ObjectResult actionResult)
            {
                this._productDetailModel = (ProductDetailModel)actionResult.Value;
            }

            statusCode.Should().Be(200);
        }

        [Then("available products list should contain the new product")]
        public void ThenAvailableProductsListShouldContainTheNewProduct()
        {
            var result = this._productController.GetAll(this._productSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            if (result is ObjectResult actionResult)
            {
                IEnumerable<ProductBasicModel> products = (IEnumerable<ProductBasicModel>)actionResult.Value;
                products.Any(p => p.Id == this._productDetailModel.Id).Should().BeTrue();
            }

            var productSaved = this._productRepository.GetOneByExpression(p => p.Id == this._productDetailModel.Id);
            this._productRepository.DeleteOne(productSaved);
            this._productRepository.Save();

            statusCode.Should().Be(200);
        }

        [Then(@"creation is not successful")]
        public void ThenCreationIsNotSuccessful()
        {
            var statusCode = 400;
            try
            {
                var result = this._productController.Create(this._productModel);
            }
            catch (InvalidResourceException)
            {
                statusCode = 400;
            }

            statusCode.Should().Be(400);
        }

        [When("(.*), (.*) and (.*) are entered")]
        [Obsolete]
        public void WhenInvalidNameDescriptionAndPriceAreEnteredForTheNewProduct(string name, string description, decimal price)
        {
            this._productModel = new ProductModel() { Name = name, Description = description, Price = price, PharmacyName = "" };
        }
    }
}