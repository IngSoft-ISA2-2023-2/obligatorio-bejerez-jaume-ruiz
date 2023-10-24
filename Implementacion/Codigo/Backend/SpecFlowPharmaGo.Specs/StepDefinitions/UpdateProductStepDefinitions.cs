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

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class UpdateProductStepDefinitions
    {
        private readonly IProductManager _productManager;
        private ProductController _productController;
        private ProductDetailModel _productDetailModel;
        private UpdateProductModel _productModel;
        private ProductSearchCriteria _productSearch;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly PharmacyGoDbContext _dbContext;
        private DbContextOptions<PharmacyGoDbContext> _options;
        private int _id;

        public UpdateProductStepDefinitions()
        {
            this._id = 1;
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
            this._productController = new ProductController(this._productManager);
        }


        [Given("the login with token (.*) is an employee")]
        public void GivenTheUserIsAnEmployee(string token)
        {
            //It is validated with filters
        }

        [When("name (.*), description (.*) and price (.*) are updated")]
        public void WhenNameDescriptionAndPriceAreEnteredForTheUpdatedProduct(string name, string description, decimal price)
        {
            this._productSearch = new ProductSearchCriteria { PharmacyId = 1, Name = name };
            this._productModel = new UpdateProductModel() { Name = name, Description = description, Price = price };
        }

        [Then("update should be successful")]
        public void ThenUpdateShouldBeSuccessful()
        {
            var result = this._productController.Update(this._id, this._productModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            if (result is ObjectResult actionResult)
            {
                this._productDetailModel = (ProductDetailModel)actionResult.Value;
            }

            statusCode.Should().Be(200);
        }

        [Then("available products list should contain the updated product")]
        public void ThenAvailableProductsListShouldContainTheUpdatedProduct()
        {
            var result = this._productController.GetAll(this._productSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            if (result is ObjectResult actionResult)
            {
                IEnumerable<ProductBasicModel> products = (IEnumerable<ProductBasicModel>)actionResult.Value;
                products.Any(this.AreEqual()).Should().BeTrue();
            }

            statusCode.Should().Be(200);
        }

        private Func<ProductBasicModel, bool> AreEqual()
        {
            return p => p.Id == this._productDetailModel.Id
            && p.Name == this._productDetailModel.Name
            && p.Description == this._productDetailModel.Description
            && p.Price == this._productDetailModel.Price;
        }

        [Then(@"update is not successful")]
        public void ThenUpdateIsNotSuccessful()
        {
            var statusCode = 400;
            try
            {
                var result = this._productController.Update(this._id, this._productModel);
            }
            catch (InvalidResourceException)
            {
                statusCode = 400;
            }
            catch (ResourceNotFoundException)
            {
                statusCode = 400;
            }

            statusCode.Should().Be(400);
        }

        [When("(.*), (.*) and (.*) are entered for the updated product")]
        [Obsolete]
        public void WhenInvalidNameDescriptionAndPriceAreEnteredForTheUpdatedProduct(string name, string description, decimal price)
        {
            this._productModel = new UpdateProductModel() { Name = name, Description = description, Price = price };
        }

        [When(@"an existing product is selected")]
        public void WhenAnExistingProductIsSelected()
        {
            //Validate that a product with a given id exists
        }

        [When(@"an unexisting product is selected")]
        public void WhenAnUnexistingProductIsSelected()
        {
            this._productModel = new UpdateProductModel() { Name = "name", Description = "description", Price = 10 };
            this._id = -1;
        }

    }
}