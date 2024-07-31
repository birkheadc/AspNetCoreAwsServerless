# Todo
## Create 3 branches that auto deploy to different environments:
  - `master`, the environment that outside users use
  - `staging`, where changes are aggragated in an environment that is as similar to Production as possible, so that changes can be pushed from there to Production as releases
  - `development`, where individual changes to code can be pushed to see how they react to the serverless environment. Resources in Development are also used when developing Locally.

Probably and github actions to configure automatic deployment.

The created resource should either have a static url (maybe using area 53) or somehow export the new URL in a way that another application (usually the frontend) can automatically update.

## Tests must all pass before merging into Staging or Production?

## Create account type or permission system

## Implement 0Auth

## Get AWS config working (region, profile, etc)

Figure out how to get terraform working in github actions. Something to do with state not being saved, so terraform tries to create everything from scratch every time. Should state be saved in src control, or store those artifacts somewhere else?

Also, now need to somehow destroy all the aws resources that the apply made without saving state. Uhhh...

Also double check that `actions/setup-dotnet@v4` is valid. I used `v1` before and it worked. `v4` appears to be most recent.

Also check if I need to declare which version of dotnet. Should be fine to just use latest...