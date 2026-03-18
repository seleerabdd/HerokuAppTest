
# TODO: Write login test scenarios here
  # 
  # Requirements:
  # 1. Test successful login with valid credentials
  #    - Username: tomsmith
  #    - Password: SuperSecretPassword!
  #    - Verify success message, secure page access, and page data
  #    - Include logout flow
  # 
  # 2. Test failed login with invalid credentials
  #    - Use invalid username/password
  #    - Verify error message is displayed

  # Test site: https://the-internet.herokuapp.com/login

  @ui
Feature: Login
  As a user
  I want to log in
  So that I can access secured pages

  Scenario: Successful login with valid credentials
    Given I open the login page at "https://the-internet.herokuapp.com/login"
    When I log in with username "tomsmith" and password "SuperSecretPassword!"
    Then I should see a success message containing "You logged into a secure area!"
    And I should be on the secure area page
    And I should see the following secure area data
      | Field        | Value                                              |
      | Title        | Secure Area                                        |
      | Message      | Welcome to the Secure Area. When you are done click logout below. |
    When I log out from the secure area
    Then I should see a success message containing "You logged out of the secure area!"
    And I should be redirected to the login page

  Scenario: Failed login with invalid credentials
    Given I open the login page at "https://the-internet.herokuapp.com/login"
    When I log in with username "invaliduser" and password "wrongpassword"
    Then I should see an error message containing "Your username is invalid!"


