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

## Integration Tests for Books

## Pagination and Data Sorting    
  
Delete current books table and repopulate with less items so I can better test, especially last page. Maybe around 25 books is best (so can have full first page, full center page,    and not-full last page). Can also test what happens when new data is created by a different user when in the middle of searching.

Add keys to table so it can be sorted by title and/or author

Maybe look into pagination: It appears pagination token is just a DynamoDBObject with the ID of the the last object from the last page (exclusive)
  - Can this be trusted? If I change the API to just expect `/books/page/{id}` and then transform that `id` into a DynamoDBObject manually, will that work? Or will AWS change this up on me at some point ruining everything
  - Also does this change if sorting on a different key? If sorting by Title does it expect the title, or still use id, or maybe both?
  - Backwards pagination is not supported by dynamodb, so the frontend will just have to keep track of pagination tokens in a stack and pop them off when going backwards.
    - What does this mean for sorting by asc/desc? Is it impossible, or maybe it should be treated as an entirely different sort

Also add (created_at) and maybe (modified_at) support

At the end of this, I want to have a table on the front end that can be sorted by created_at (and maybe modified_at), title, and author, backwards or forwards. Data should be paginated and browsable.