# AspNetCore Aws Serverless API Template

A basic template app for getting off the ground.

The app acts as a repository of fake book meta data. Anonymous users can Get books. Users can register and sign in through OAuth2. Registered users can create and manage a list of their favorites that other users cannot access. Admins can create, edit, and delete books entirely.

## Deployment

Currently working on deploying automatically into 3 environments on AWS: (Development, Staging, Production) using Terraform and GitHub Actions

Terraform stuff is in the `infra` folder

- Change to the `infra` directory

- Initialize terraform

  `terraform init`

- Apply changes

  `terraform apply`

### Secrets

Secrets related to deployment (access keys for aws) are kept as GitHub repository secrets

### Continuous Integration and Continuous Delivery (CI/CD)

Deployment is done automatically when pushing to certain branches. In order to do this, Terraform needs a place to store (sometimes sensitive) information on state. This is stored and received via AWS, but must be set up prior to creating the app. I use a shared S3 bucket and DynamoDB for all of my deployments.

- Create a bucket
  - ACLs disabled
  - Block all public access
  - Bucket Versioning: Enable
  - Encryption Type: SSE-S3
  - Bucket Key: Enable
  - Object Lock: Disable
  
- Create a DynamoDB Table
  - Partition Key: LockID (string)
  - Default settings

## Auth

Authentication / Authorization uses AWS Cognito.

## Validation

Validation in this context refers to basic validation of requests, for example:
  - Is the request body of the correct shape for the request
  - Are the values valid:
    - Id must be a valid Guid
    - String cannot be empty
    - Password must be at least 8 characters
    - Number must be greater than 0 but less than 100

This basic validation is done with FluentValidation. FluentValidation no longer supports automatic validation out of the box, so I've created `Filters/FluentValidationFilter` to automatic check all incoming requests for validatable arguments, validate them, and reject the request with the proper errors.

## ApiResult

This application makes extensive use of the Result pattern, rather than throwing exceptions. I created my own simple implementation of it, focused around `Utils/Result/ApiResult`. Instead of functions that expect a certain Type to be returned, and throw when that type cannot be returned, functions expect a `ApiResult<Type>` to be returned, and must check the result for success or failure and handle each situation. `ApiResult` can also include details on why the failure occurred. It also has a method `GetActionResult` for quickly converting a result to an object that can be returned from the controller. `FluentValidationFilter` also makes use of `ApiResult`, helping ensure uniformity in the shape of outgoing error messages.

## How to create a new project with this template

- Copy or clone the entire repository
- Make the following changes with the correct information for you and your project:
  - `infra/backend.conf`
    - Change the bucket name to the name of the bucket you use for storing terraform state
    - Change the key name the the name of the project
    - Change the dynamodb table name to the name of the table you use for storing terraform state locks
  - TODO: Find other places that need configured...