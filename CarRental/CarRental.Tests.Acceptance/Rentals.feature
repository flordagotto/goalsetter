Feature: rent vehicles

Background:
	Given The Client 'Ivan' 'Martinez' is registered

Scenario: A Client creates a rental with an available vehicle
	Given There is a vehicle 'VW Golf' with Price 25.00
	When The client creates a rental from '2021/04/01' to '2021/04/10' With vehicle 1
	Then The client has a rental for 225

