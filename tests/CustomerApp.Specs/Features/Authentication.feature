Feature: Authentication

Scenario: Customer login
	Given a registered customer with random email
	When the customer logins
	Then a successful response is returned
	And a token is generated

Scenario: Customer forgot password
	Given a registered customer with random email
	When the customer calls forgot password feature
	Then a successful response is returned
	And an email with reset password token is sent
	When customer reset password
	Then a successful response is returned
	When the customer logins
	Then a successful response is returned
	And a token is generated
