Feature: CreateProduct

@mytag
Scenario Outline: Create Product Successfully

	Given the user logged with token Token123 is an employee
	When name <name>, description <description> and price <price> are entered for the new product
	Then creation should be successful
	And available products list should contain the new product

Examples:
	| name                 | description                                    | price |
	| Shampoo Sedal 200 ml | Dale vida a tu pelo con el nuevo shampoo Sedal | 75.5  |
	| Loreal               | Shampu.                                        | 20    |
	| Jabon                | Jabon jabon                                    | 30    |

@mytag
Scenario Outline: Required fields are missing

	Given the user logged with token Token123 is an employee
	When <name>, <description> and <price> are entered
	Then creation is not successful

Examples:
	| name   | description | price |
	|        | desc        | 75.5  |
	| Loreal |             | 75.5  |


@mytag
Scenario Outline: Name has more than 30 characters

	Given the user logged with token Token123 is an employee
	When <name>, <description> and <price> are entered
	Then creation is not successful

Examples:
	| name                                    | description | price |
	| nombre de producto de muchos caracteres | desc        | 75.5  |
	| nombre de producto de caractere         | desc        | 75.5  |
	| nombre de producto de caracteres        | desc        | 75.5  |


@mytag
Scenario Outline: Description has more than 70 characters

	Given the user logged with token Token123 is an employee
	When <name>, <description> and <price> are entered
	Then creation is not successful

Examples:
	| name   | description                                                                        | price |
	| Loreal | DescripcionLargaDescripcionLargaDescripcionLargaDescripcionLargaAAAAAAA            | 75.5  |
	| Loreal | DescripcionLarga DescripcionLarga DescripcionLargaDescripcionLargaAAAAAAsfdsnslfdA | 75.5  |


@mytag
Scenario Outline: Price is Less than or Equal to Zero

	Given the user logged with token Token123 is an employee
	When <name>, <description> and <price> are entered
	Then creation is not successful

Examples:
	| name   | description | price  |
	| Loreal | desc        | 0      |
	| Loreal | desc        | -1     |
	| Loreal | desc        | -56465 |