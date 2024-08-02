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

## How to create a new project with this template

- Copy or clone the entire repository
- Make the following changes with the correct information for you and your project:
  - `infra/backend.conf`
    - Change the bucket name to the name of the bucket you use for storing terraform state
    - Change the key name the the name of the project
    - Change the dynamodb table name to the name of the table you use for storing terraform state locks
  - TODO: Find other places that need configured...