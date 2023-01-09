Feature: Customer

Scenario: Get customer info
	Given a registered customer Jorys
	When use JWT token to get info
	Then successful response is returned
	And customer response contains first name Jorys

Scenario: Get customer info without JWT token
	Given a registered customer Jorys
	When get info without JWT token
	Then unauthorized response is returned

Scenario: Update customer info
	Given a registered customer Jorys
	When use JWT token to update first name to Jory
	Then successful response is returned
	And customer response contains first name Jory

Scenario: Update customer info without JWT token
	Given a registered customer Jorys
	When update first name to Jory without JWT token
	Then unauthorized response is returned

Scenario: Update customer password
	Given a registered customer with password P@sSw0rD!
	When use JWT token to update password to Passw0rd!
	Then successful response is returned
	When customer logins with password Passw0rd!
	Then successful response is returned

Scenario: Update customer password without JWT token
	Given a registered customer with password P@sSw0rD!
	When update password to Passw0rd! without JWT token
	Then unauthorized response is returned
