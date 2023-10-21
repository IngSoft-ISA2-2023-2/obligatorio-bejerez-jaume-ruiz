namespace SpecFlowPharmaGo.Specs.StepDefinitions
{
    [Binding]
    public sealed class CreateProductStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        [Given("the user with Id (.*) is an employee")]
        public void GivenTheUserIsAnEmployee(int userId)
        {
            // Implementar la lógica para establecer que el usuario está autenticado como empleado.
            // Tengo que guardar el id
        }

        [Given("the user works in the pharmacy with Id (.*)")]
        public void GivenTheUserWorksInThePharmacy(int pharmacyId)
        {
            // Implementar la lógica para establecer que el usuario trabaja en dicha farmacia.
            // Tengo que guardar el id
        }

        [When("name (.*), description (.*) and price (.*) are entered for the new product")]
        public void WhenNameDescriptionAndPriceAreEnteredForTheNewProduct(string name, string description, decimal price)
        {
            // Implementar la lógica para validar el 'name', 'description' y 'price' del nuevo producto.
            // Dar de alta al nuevo producto
            // Guardar el producto devuelto al crear
        }

        [Then("creation should be successful")]
        public void ThenCreationShouldBeSuccessful()
        {
            // Implementar la lógica para verificar que la creación del producto fue exitosa.
            // EN LA ANTERIOR SE HACE EL LLAMADO, SE GUARDA EL PRODUCTO
            // ACA SE TOMA EL PRODUCTO Y SE VALIDA QUE SEA DISTINTO DE NULL (SINO DARIA ERROR ANTES IGUAL)
            // ME GUARDO EL CODIGO
        }

        [Then("available products list should contain the new product")]
        public void ThenAvailableProductsListShouldContainTheNewProduct()
        {
            // Implementar la lógica para verificar que la lista de productos disponibles ahora contiene el nuevo producto.
            // ACA TOMO LA LISTA DE PRODUCTOS Y ME FIJO QUE ESTE EL DE CODIGO GUARDADO EN EL ANTERIOR
        }
    }
}