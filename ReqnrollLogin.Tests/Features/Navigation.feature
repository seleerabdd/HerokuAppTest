@ui
Feature: Navigation and Assertions
  As a user
  I want to navigate across pages
  So that UI behavior and data assertions can be validated

  Background:
    Given I open the main page at "https://the-internet.herokuapp.com"

  Scenario: Navigate to Checkboxes page and validate checkbox state changes
    When I navigate to page link "Checkboxes"
    Then I should be on a page with header "Checkboxes"
    And checkbox 1 should be unchecked
    And checkbox 2 should be checked
    When I set checkbox 1 to checked
    Then checkbox 1 should be checked

  Scenario: Navigate to Dropdown page and assert selected option
    When I navigate to page link "Dropdown"
    Then I should be on a page with header "Dropdown List"
    When I select "Option 2" from dropdown
    Then selected dropdown option should be "Option 2"

  Scenario: Navigate to Inputs page and assert field value
    When I navigate to page link "Inputs"
    Then I should be on a page with header "Inputs"
    When I enter "2026" into the number input
    Then the number input value should be "2026"
