Feature: CreateProduct

@mytag
Scenario: Create Product Successfully
	Given the user with Id 1 is an employee
	When name Shampoo Sedal 200 ml, description Dale vida a tu pelo con el nuevo shampoo Sedal and price 75.5 are entered for the new product
	Then creation should be successful
	And available products list should contain the new product

@mytag
Scenario: Required Fields are missing
	Given the user with Id 1 is an employee
	When only description Dale vida a tu pelo con el nuevo shampoo Sedal and price 75.5 are entered for the new product
	Then creation is not successful