Feature: CreatePurchase

@mytag
Scenario: Create Purchase Successfully

	Given I am an anonymous user and select products
	When I buy existing products
	Then the purchase is successful

@mytag
Scenario: Create Purchase Without Item Codes

	Given I am an anonymous user and select products
	When I buy unexisting products
	Then the purchase is not successful
