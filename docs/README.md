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