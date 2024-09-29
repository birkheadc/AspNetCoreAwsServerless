# Infrastructure

Currently in the middle of refactoring everything so changes are easier to push from one environment to another.

Most of the infrastructure code is held in `common`. Each environment is simply a shell around calling the `common` module tree for its own environment.

It is important to keep infrastructure changes in sync with the environment branch that they apply to. Whenever a push is made to production, production will apply any changes to any modules in `common` in the current branch to the production infrastructure, etc.

## Applying TF changes locally

Github actions are in place to update infrastructure when pushing to `development`, `staging`, or `production`. Github holds repository secrets needed to access the S3 backend. In order to sync TF to this backend locally, these environment variables must be provided. Do so by exporting `AWS_ACCESS_KEY_ID` and `AWS_SECRET_ACCESS_KEY` into the terminal before performing terraform commands. I haven't come up with a good solution for doing this automatically, so it must be done every time a new terminal is opened. Most of the time, terraform updates should be performed by simply pushing to the relevant branch.