# Todo
## Create 3 branches that auto deploy to different environments:
  - `master`, the environment that outside users use
  - `staging`, where changes are aggragated in an environment that is as similar to Production as possible, so that changes can be pushed from there to Production as releases
  - `development`, where individual changes to code can be pushed to see how they react to the serverless environment. Resources in Development are also used when developing Locally.

The created resource should either have a static url (maybe using area 53) or somehow export the new URL in a way that another application (usually the frontend) can automatically update.

## Create account type or permission system

After figuring out how to get Cognito working, Cognito infrastructure should be included in API infra. Different user pool per environment

## Testing environment

Need to create a 4th environment that can be deployed, reset to 0, then used for integration and/or e2e tests, then taken back down.

## Integration Tests for Books

## Pagination and Data Sorting    

Add keys to table so it can be sorted by title and/or author

Also add (created_at) and maybe (modified_at) support

At the end of this, I want to have a table on the front end that can be sorted by created_at (and maybe modified_at), title, and author, backwards or forwards. Data should be paginated and browsable.

## MeController

Finish Users resource
 - Create Converter
 - Create Repository
 - Hook Controller, Converter, Service, and Repo up
 - Create DynamoDBTable (Terraform)

 ## Cognito

 Once Auth is working, delete the resource from aws and rebuild IaC via Terraform
   - Update staging infra to reflect recent changes to development infra
   - Add Cognito stuff to development and staging infra
   - This workflow sucks... What did I even change in development?

## Infra

Make sure that cognito module actual adds localhost callback to list of allowed urls when deploying (that flatten function)