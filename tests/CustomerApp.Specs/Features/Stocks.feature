Feature: Stocks

Scenario: Add and remove items with JWT token
	Given a registered customer
	When use JWT token to add 11 books
	Then successful response is returned
	And stocks response contains 11 books
	When use JWT token to remove 2 books
	Then successful response is returned
	And stocks response contains 9 books
	When use JWT token to get stocks
	Then successful response is returned
	And stocks response contains 9 books

Scenario: get stock without JWT token
	Given a registered customer
	When get stocks without JWT token
	Then unauthorized response is returned
	
Scenario: add items without JWT token
	Given a registered customer
	When add 10 books without JWT token
	Then unauthorized response is returned
	
Scenario: remove items without JWT token
	Given a registered customer
	When remove 10 books without JWT token
	Then unauthorized response is returned
