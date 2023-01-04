Feature: Customer

Scenario: Get customer info
	Given a registered customer Jorys
	When use JWT token to get info
	Then successful response is returned
	And customer response contains first name Jorys

Scenario: Update customer info
	Given a registered customer Jorys
	When use JWT token to update first name to Jory
	Then successful response is returned
	And customer response contains first name Jory

Scenario: Update customer password
	Given a registered customer with password P@sSw0rD!
	When use JWT token to update password to Passw0rd!
	Then successful response is returned
	When customer logins with password Passw0rd!
	Then successful response is returned

