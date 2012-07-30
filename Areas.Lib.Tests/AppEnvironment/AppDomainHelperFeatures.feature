Feature: Search all assemblies in app domain
	I want to search types in the whole app domain 
	for different critareas

Scenario: Find by attribute type
	Given I am running any application
	When I provide an attribute type to find all matching types
	Then the result should be list of all classes which has the that attribute applied
