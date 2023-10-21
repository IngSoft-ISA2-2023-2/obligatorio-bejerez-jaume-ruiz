Feature: CreateProduct

@mytag
Scenario: Create Product Successfully
	Given the user with Id 1 is an employee
	And the user works in the pharmacy with Id 1
	When name Shampoo Sedal 200 ml, description Dale vida a tu pelo con el nuevo shampoo Sedal and price 75 are entered for the new product
	Then creation should be successful
	And available products list should contain the new product