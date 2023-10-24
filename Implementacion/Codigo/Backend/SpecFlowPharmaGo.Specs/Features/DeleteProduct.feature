Feature: DeleteProduct

@mytag
Scenario: Delete Product Successfully

	Given the user is an employee
	When an existing product is selected to delete
	Then delete should be successful
	And available products list should not contain the deleted product

@mytag
Scenario: Delete an Unexisting Product

	Given the user is an employee
	When an unexisting product is selected to delete
	Then delete is not successful
