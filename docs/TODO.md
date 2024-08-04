# Todo
## Create 3 branches that auto deploy to different environments:
  - `master`, the environment that outside users use
  - `staging`, where changes are aggragated in an environment that is as similar to Production as possible, so that changes can be pushed from there to Production as releases
  - `development`, where individual changes to code can be pushed to see how they react to the serverless environment. Resources in Development are also used when developing Locally.

The created resource should either have a static url (maybe using area 53) or somehow export the new URL in a way that another application (usually the frontend) can automatically update.

## Create account type or permission system

## Implement

## Testing environment

Need to create a 4th environment that can be deployed, reset to 0, then used for integration and/or e2e tests, then taken back down.