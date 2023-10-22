using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using System.Collections;
using System.Xml.Linq;

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreateProductStepDefinitions
    {
        private readonly Mock<IProductManager> _productManagerMock;
        private ProductController _productController;
        private string _token;
        private Product _product;
        private ProductModel _productModel;
        private ProductSearchCriteria _productSearch;
        private List<Product> _products;

        public CreateProductStepDefinitions()
        {
            this._productManagerMock = new Mock<IProductManager>(MockBehavior.Strict);
            this._products = new List<Product>();
        }


        [Given("the user logged with token (.*) is an employee")]
        public void GivenTheUserIsAnEmployee(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;
            this._productController = new ProductController(this._productManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            this._token = token;
        }

        [When("name (.*), description (.*) and price (.*) are entered for the new product")]
        public void WhenNameDescriptionAndPriceAreEnteredForTheNewProduct(string name, string description, decimal price)
        {
            this._product = new Product(name, description, price);
            this._products.Add(this._product);
            this._productSearch = new ProductSearchCriteria { PharmacyId = this._product.Id, Name = this._product.Name };
            this._product.Pharmacy = new Pharmacy();
            this._productModel = new ProductModel() { Name = name, Description = description, Price = price, PharmacyName = "" };
        }

        [Then("creation should be successful")]
        public void ThenCreationShouldBeSuccessful()
        {
            this._productManagerMock.Setup(x => x.Create(It.IsAny<Product>(), this._token)).Returns(this._product);
            var result = this._productController.Create(this._productModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            this._productManagerMock.VerifyAll();
            statusCode.Should().Be(200);
        }

        [Then("available products list should contain the new product")]
        public void ThenAvailableProductsListShouldContainTheNewProduct()
        {
            this._productManagerMock.Setup(x => x.GetAll(this._productSearch)).Returns(this._products);
            var result = this._productController.GetAll(this._productSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            this._productManagerMock.VerifyAll();
            statusCode.Should().Be(200);
        }

        [Then(@"creation is not successful")]
        public void ThenCreationIsNotSuccessful()
        {
            this._productManagerMock.Setup(x => x.Create(It.IsAny<Product>(), this._token)).Throws(new InvalidResourceException("Invalid Product"));
            var statusCode = 400;
            try
            {
                var result = this._productController.Create(this._productModel);
                var objectResult = result as ObjectResult;
                statusCode = (int)objectResult.StatusCode;
            }
            catch (InvalidResourceException)
            {
                statusCode = 400;
            }

            this._productManagerMock.VerifyAll();

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