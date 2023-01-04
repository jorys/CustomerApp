Feature: Authentication

Scenario: Customer login
	Given a registered customer with password P@sSw0rD!
	When customer logins with password P@sSw0rD!
	Then successful response is returned
	And JWT token is generated
	
Scenario: Customer login invalid password
	Given a registered customer with password P@sSw0rD!
	When customer logins with password InvalidPassword
	Then fail response is returned

Scenario: Customer forgot password
	Given a registered customer with password P@sSw0rD!
	When call forgot password on generated email
	Then successful response is returned
	And an email with reset password token is sent
	When use email token to reset password to Passw0rd!
	Then successful response is returned
	When customer logins with password Passw0rd!
	Then successful response is returned
	And JWT token is generated
	
Scenario: Customer forgot password invalid token
	Given a registered customer with password P@sSw0rD!
	When call forgot password on generated email
	Then successful response is returned
	When use invalid token to reset password to Passw0rd!
	Then fail response is returned
	When customer logins with password Passw0rd!
	Then fail response is returned
