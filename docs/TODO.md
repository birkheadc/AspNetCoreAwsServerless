# Todo

## Testing environment

Probably going to use nothing for unit tests, staging environment for integration tests

## Failed integration tests are hanging the test runner... but only the test runner in vscode, CLI is fine

This seems to be only a problem on my laptop ¯\_(ツ)_/¯

## Cookies

One thing left:
  - Cookies probably won't work great with lambda atm because the next time the lambda restarts the API will not recognize the cookies from last session
    - Something about DataProtection?