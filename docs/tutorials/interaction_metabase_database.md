# Test the interaction of metabase and database

This tutorial explains how you can test the interaction of metabase and
database using the frontend as graphical user interface. For convenience,
the GraphQL queries and mutations are added at the beginning of each step. If something should
fail in the frontend, use the GraphQL endpoint of the backend.

This tutorial can be used to validate your setup in a) `develop`, b) `staging` and c) `production`. For example

## Prepare

1. Follow the sections `Getting Started` of [metabase](https://github.com/building-envelope-data/metabase?tab=readme-ov-file#getting-started)
   and [database](https://github.com/building-envelope-data/database?tab=readme-ov-file#getting-started). The sections use the repository 
   [machine](https://github.com/building-envelope-data/machine).
1. Within the project [machine](https://github.com/building-envelope-data/machine), 
   create a user for restricted areas like staging and email, for example with 
   `./deploy.mk user NAME=userRestrictedAreas`.

## Quick test

1. `loginUser`: Use the username `administrator@buildingenvelopedata.org` and
   the `BOOTSTRAP_USER_PASSWORD` which you find in `./.env` to sign in as admin at the metabase.
   - a) https://www.local.buildingenvelopedata.org:7001/connect/client/login
   - b) https://staging.buildingenvelopedata.org/connect/client/login
   - c) https://www.buildingenvelopedata.org/connect/client/login
1. `createInstitution`: Create a new institution.
   - a) https://www.local.buildingenvelopedata.org:7001/institutions
   - b) https://staging.buildingenvelopedata.org/institutions
   - c) https://www.buildingenvelopedata.org/institutions
1. Save the `institutionId` of the new institution and open its page. Apply the
   next steps on that page.
   - a) https://www.local.buildingenvelopedata.org:7001/institutions/${instituionId}
   - b) https://staging.buildingenvelopedata.org/institutions/${instituionId}
   - c) https://www.buildingenvelopedata.org/institutions/${instituionId}
1. `createComponent`: Create a new component which is manufactured by your new
   institution. Save the `componentId`.
1. `createMethod`: Create a new method which is managed by your new institution.
   Save the `methodId`.
1. `createDataFormat`: Create a new data format which is managed by your new 
   institution. Save the `dataFormatId`.

######Create dataset. Find dataset.

## Detailed test

1. `registerUser`: Register as new user at the metabase.
   a) https://www.local.buildingenvelopedata.org:7001/users/register
   b) https://staging.buildingenvelopedata.org/users/register
   c) https://www.buildingenvelopedata.org/users/register
1. Use your account for restricted areas to open 
   https://www.local.buildingenvelopedata.org:7001/email/ . Open the registration email and use the link to confirm your metabase user account.
1. [Login in](https://www.local.buildingenvelopedata.org:7001/users/login) with 
   your metabase account. 
1. [Create an institution](https://www.local.buildingenvelopedata.org:7001/institutions/create).
   Use the UUID of the institution to verify the pending institution with
   ```

   ```
1. Add a component, a method, a data format and a database to this institution. The database must include the URL of the GraphQL endpoint e.g. https://www.local.solarbuildingenvelopes.com:7501/graphql/ . Copy the UUIDs of the database and of your institution and paste it into the .env file of your `database` project. In the folder of your database, run `make down && make build up`. Sign in on https://www.local.solarbuildingenvelopes.com:7501/ with your metabase account. Create an optical dataset on https://www.local.solarbuildingenvelopes.com:7501/graphql/ using the mutation `createOpticalData`. List all pending optical datasets with the query `allPendingOpticalData`. Publish your optical dataset with the mutation `publishData`. Search your database for your optical dataset e.g. with the query `allOpticalData`. Search the metabase for your optical dataset e.g. with the query `databases{edges{node{allOpticalData`.