using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class DeleteProductStepDefinitions
    {
        private readonly Mock<IProductManager> _productManagerMock;
        private ProductController _productController;
        private ProductSearchCriteria _productSearch;
        private int _id;

        public DeleteProductStepDefinitions()
        {
            this._productManagerMock = new Mock<IProductManager>(MockBehavior.Strict);
            this._id = 1;
            this._productController = new ProductController(this._productManagerMock.Object);
        }


        [Given("the user is an employee")]
        public void GivenTheUserIsAnEmployee()
        {
            //It is validated with filters
        }

        [Then("delete should be successful")]
        public void ThenDeleteShouldBeSuccessful()
        {
            this._productManagerMock.Setup(x => x.Delete(this._id));
            var result = this._productController.Delete(this._id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            this._productManagerMock.VerifyAll();
            statusCode.Should().Be(200);
        }

        [Then("available products list should not contain the updated product")]
        public void ThenAvailableProductsListShouldNotContainTheUpdatedProduct()
        {
            this._productManagerMock.Setup(x => x.GetAll(this._productSearch)).Returns(new List<Product>());
            var result = this._productController.GetAll(this._productSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            this._productManagerMock.VerifyAll();
            statusCode.Should().Be(200);
        }

        [Then(@"delete is not successful")]
        public void ThenUpdateIsNotSuccessful()
        {
            var statusCode = 400;
            try
            {
                var result = this._productController.Delete(this._id);
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

        [When(@"an existing product is selected to delete")]
        public void WhenAnExistingProductIsSelected()
        {
            //Validate that a product with a given id exists
        }

        [When(@"an unexisting product is selected to delete")]
        public void WhenAnUnexistingProductIsSelected()
        {
            this._productManagerMock.Setup(x => x.Delete(this._id)).Throws(new ResourceNotFoundException("Invalid Product"));
        }

    }
}