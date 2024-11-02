# Todo
## Create 3 branches that auto deploy to different environments:
  - `master`, the environment that outside users use
  - `staging`, where changes are aggragated in an environment that is as similar to Production as possible, so that changes can be pushed from there to Production as releases
  - `development`, where individual changes to code can be pushed to see how they react to the serverless environment. Resources in Development are also used when developing Locally.

The created resource should either have a static url (maybe using area 53) or somehow export the new URL in a way that another application (usually the frontend) can automatically update.

## Create account type or permission system

After figuring out how to get Cognito working, Cognito infrastructure should be included in API infra. Different user pool per environment

## Testing environment

Probably going to use nothing for unit tests, staging environment for integration tests

## Pagination and Data Sorting    

Add keys to table so it can be sorted by title and/or author

Also add (created_at) and maybe (modified_at) support

At the end of this, I want to have a table on the front end that can be sorted by created_at (and maybe modified_at), title, and author, backwards or forwards. Data should be paginated and browsable.

## Session

Need to develop a relogin workflow when cookies are present.

Need to create logout function (front end currently just logs out memory)
  - Invalidate cookies
  - Remove cookies from browsers

## Failed integration tests are hanging the test runner... but only the test runner in vscode, CLI is fine

This seems to be only a problem on my laptop ¯\_(ツ)_/¯

## User Profile

Split user data that the user should be able to change from user data that is effectively immutable (username, roles)

Allow users to change the former, but not the latter

## Cookies

Two things with cookies:
  - Cookies are not being set properly in the frontend
    - Something about the api gateway?
  - Cookies probably won't work great with lambda atm because the next time the lambda restarts the API will not recognize the cookies from last session
    - Something about DataProtection?