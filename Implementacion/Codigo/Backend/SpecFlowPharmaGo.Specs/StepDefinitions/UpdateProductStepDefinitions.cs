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
    public sealed class UpdateProductStepDefinitions
    {
        private readonly Mock<IProductManager> _productManagerMock;
        private ProductController _productController;
        private Product _product;
        private UpdateProductModel _productModel;
        private ProductSearchCriteria _productSearch;
        private List<Product> _products;
        private int _id;

        public UpdateProductStepDefinitions()
        {
            this._productManagerMock = new Mock<IProductManager>(MockBehavior.Strict);
            this._products = new List<Product>();
            this._id = 1;
            this._productController = new ProductController(this._productManagerMock.Object);
        }


        [Given("the login with token (.*) is an employee")]
        public void GivenTheUserIsAnEmployee(string token)
        {
            //It is validated with filters
        }

        [When("name (.*), description (.*) and price (.*) are updated")]
        public void WhenNameDescriptionAndPriceAreEnteredForTheUpdatedProduct(string name, string description, decimal price)
        {
            this._product = new Product(name, description, price);
            this._products.Add(this._product);
            this._productSearch = new ProductSearchCriteria { PharmacyId = this._product.Id, Name = this._product.Name };
            this._product.Pharmacy = new Pharmacy();
            this._productModel = new UpdateProductModel() { Name = name, Description = description, Price = price };
        }

        [Then("update should be successful")]
        public void ThenUpdateShouldBeSuccessful()
        {
            this._productManagerMock.Setup(x => x.Update(this._id, It.IsAny<Product>())).Returns(this._product);
            var result = this._productController.Update(this._id, this._productModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            this._productManagerMock.VerifyAll();
            statusCode.Should().Be(200);
        }

        [Then("available products list should contain the updated product")]
        public void ThenAvailableProductsListShouldContainTheUpdatedProduct()
        {
            this._productManagerMock.Setup(x => x.GetAll(this._productSearch)).Returns(this._products);
            var result = this._productController.GetAll(this._productSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            this._productManagerMock.VerifyAll();
            statusCode.Should().Be(200);
        }

        [Then(@"update is not successful")]
        public void ThenUpdateIsNotSuccessful()
        {
            var statusCode = 400;
            try
            {
                var result = this._productController.Update(this._id, this._productModel);
                var objectResult = result as ObjectResult;
                statusCode = (int)objectResult.StatusCode;
            }
            catch (InvalidResourceException)
            {
                statusCode = 400;
            }
            catch (ResourceNotFoundException)
            {
                statusCode = 400;
            }

            this._productManagerMock.VerifyAll();

            statusCode.Should().Be(400);
        }

        [When("(.*), (.*) and (.*) are entered for the updated product")]
        [Obsolete]
        public void WhenInvalidNameDescriptionAndPriceAreEnteredForTheUpdatedProduct(string name, string description, decimal price)
        {
            this._productModel = new UpdateProductModel() { Name = name, Description = description, Price = price };
            this._productManagerMock.Setup(x => x.Update(this._id, It.IsAny<Product>())).Throws(new InvalidResourceException("Invalid Product"));
        }

        [When(@"an existing product is selected")]
        public void WhenAnExistingProductIsSelected()
        {
            //Validate that a product with a given id exists
        }

        [When(@"an unexisting product is selected")]
        public void WhenAnUnexistingProductIsSelected()
        {
            this._productModel = new UpdateProductModel();
            this._productManagerMock.Setup(x => x.Update(this._id, It.IsAny<Product>())).Throws(new ResourceNotFoundException("Invalid Product"));
        }

    }
}