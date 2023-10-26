using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using PharmaGo.WebApi.Models.Out;

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class DeleteProductStepDefinitions
    {
        private readonly IProductManager _productManager;
        private ProductController _productController;
        private ProductSearchCriteria _productSearch;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly PharmacyGoDbContext _dbContext;
        private DbContextOptions<PharmacyGoDbContext> _options;
        private int _id;
        private Product _product;


        public DeleteProductStepDefinitions()
        {
            this._id = 800;
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
            this._productSearch = new ProductSearchCriteria();
            this.InitializeProduct();
        }

        private void InitializeProduct()
        {
            var pharmacySaved = this._pharmacyRepository.GetOneByExpression(p => p.Id == 1);
            this._product = new Product()
            {
                Name = "nombre",
                Code = "123456",
                Price = 200,
                Deleted = false,
                Description = "Description",
                Pharmacy = pharmacySaved
            };
            this._productRepository.InsertOne(this._product);
            this._productRepository.Save();
            this._product = this._productRepository.GetOneByExpression(p => p.Code == this._product.Code);
        }

        [Given("the user is an employee")]
        public void GivenTheUserIsAnEmployee()
        {
            //It is validated with filters
        }

        [Then("delete should be successful")]
        public void ThenDeleteShouldBeSuccessful()
        {
            var result = this._productController.Delete(this._id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            statusCode.Should().Be(200);
        }

        [Then("available products list should not contain the deleted product")]
        public void ThenAvailableProductsListShouldNotContainTheDeletedProduct()
        {
            var result = this._productController.GetAll(this._productSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            if (result is ObjectResult actionResult)
            {
                IEnumerable<ProductBasicModel> products = (IEnumerable<ProductBasicModel>)actionResult.Value;
                products.Any(this.AreEqual()).Should().BeFalse();
            }

            var productSaved = this._productRepository.GetOneByExpression(p => p.Id == this._product.Id);
            this._productRepository.DeleteOne(productSaved);
            this._productRepository.Save();

            statusCode.Should().Be(200);
        }

        private Func<ProductBasicModel, bool> AreEqual()
        {
            return p => p.Id == this._product.Id
            && p.Name == this._product.Name
            && p.Description == this._product.Description
            && p.Price == this._product.Price;
        }

        [Then(@"delete is not successful")]
        public void ThenUpdateIsNotSuccessful()
        {
            var statusCode = 400;
            try
            {
                var result = this._productController.Delete(this._id);
            }
            catch (ResourceNotFoundException)
            {
                statusCode = 400;
            }
            var productSaved = this._productRepository.GetOneByExpression(p => p.Id == this._product.Id);
            this._productRepository.DeleteOne(productSaved);
            this._productRepository.Save();

            statusCode.Should().Be(400);
        }

        [When(@"an existing product is selected to delete")]
        public void WhenAnExistingProductIsSelected()
        {
            this._id = this._product.Id;
        }

        [When(@"an unexisting product is selected to delete")]
        public void WhenAnUnexistingProductIsSelected()
        {
            this._id = -1;
        }

    }
}