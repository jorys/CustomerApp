Feature: Customer

Scenario: Get customer info
	Given a registered customer with random email
	When customer info is retrieved
	Then a successful response is returned
	And customer data contains correct email

Scenario: Update customer info
	Given a registered customer with random email
	When customer first name is updated to Jory
	Then a successful response is returned
	And customer data contains first name Jory
