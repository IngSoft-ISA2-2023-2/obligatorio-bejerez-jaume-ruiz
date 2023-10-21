using Microsoft.VisualStudio.TestTools.UnitTesting;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using System.Collections;
using System.Xml.Linq;

namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreateProductStepDefinitions
    {
        private readonly CreateProduct _createProduct = new CreateProduct();
        private Product? _product = null;
        private List<Product> _products = new List<Product>();


        [Given("the user with Id (.*) is an employee")]
        public void GivenTheUserIsAnEmployee(int userId)
        {
            this._createProduct.UserId = userId;
        }

        [When("name (.*), description (.*) and price (.*) are entered for the new product")]
        public void WhenNameDescriptionAndPriceAreEnteredForTheNewProduct(string name, string description, decimal price)
        {
            this._product = this._createProduct.CreateNewProduct(name, description, price);
            this._products.Add(this._product);
        }

        [Then("creation should be successful")]
        public void ThenCreationShouldBeSuccessful()
        {
            this._product.Should().NotBeNull();
        }

        [Then("available products list should contain the new product")]
        public void ThenAvailableProductsListShouldContainTheNewProduct()
        {
            this._products.Should().Contain(this._product);
        }

        [Then(@"creation is not successful")]
        public void ThenCreationIsNotSuccessful()
        {
            this._product.Should().BeNull();
        }

        [When("(.*), (.*) and (.*) are entered")]
        [Obsolete]
        public void WhenInvalidNameDescriptionAndPriceAreEnteredForTheNewProduct(string name, string description, decimal price)
        {
            string? exceptionMessage = null;
            try
            {
                this._product = this._createProduct.CreateNewProduct(name, description, price);
                this._products.Add(this._product);
            }
            catch (InvalidResourceException e)
            {
                exceptionMessage = e.Message;
            }

            exceptionMessage.Should().NotBeNull();
        }

    }
}