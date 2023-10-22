Feature: UpdateProduct

@mytag
Scenario Outline: Update Product Successfully

	Given the login with token Token123 is an employee
	When an existing product is selected
	And name <name>, description <description> and price <price> are updated
	Then update should be successful
	And available products list should contain the updated product

Examples:
	| name                 | description                                    | price |
	| Shampoo Sedal 200 ml | Dale vida a tu pelo con el nuevo shampoo Sedal | 75.5  |
	| Loreal               | Shampu.                                        | 20    |
	| Jabon                | Jabon jabon                                    | 30    |

@mytag
Scenario: Update an Unexisting Product

	Given the login with token Token123 is an employee
	When an unexisting product is selected
	Then update is not successful

@mytag
Scenario Outline: Required fields are missing

	Given the login with token Token123 is an employee
	When an existing product is selected
	And <name>, <description> and <price> are entered for the updated product
	Then update is not successful

Examples:
	| name   | description | price |
	|        | desc        | 75.5  |
	| Loreal |             | 75.5  |


@mytag
Scenario Outline: Name has more than 30 characters

	Given the login with token Token123 is an employee
	When an existing product is selected
	And <name>, <description> and <price> are entered for the updated product
	Then update is not successful

Examples:
	| name                                    | description | price |
	| nombre de producto de muchos caracteres | desc        | 75.5  |
	| nombre de producto de caractere         | desc        | 75.5  |
	| nombre de producto de caracteres        | desc        | 75.5  |


@mytag
Scenario Outline: Description has more than 70 characters

	Given the login with token Token123 is an employee
	When an existing product is selected
	And <name>, <description> and <price> are entered for the updated product
	Then update is not successful

Examples:
	| name   | description                                                                        | price |
	| Loreal | DescripcionLargaDescripcionLargaDescripcionLargaDescripcionLargaAAAAAAA            | 75.5  |
	| Loreal | DescripcionLarga DescripcionLarga DescripcionLargaDescripcionLargaAAAAAAsfdsnslfdA | 75.5  |


@mytag
Scenario Outline: Price is Less than or Equal to Zero

	Given the login with token Token123 is an employee
	When an existing product is selected
	And <name>, <description> and <price> are entered for the updated product
	Then update is not successful

Examples:
	| name   | description | price  |
	| Loreal | desc        | 0      |
	| Loreal | desc        | -1     |
	| Loreal | desc        | -56465 |