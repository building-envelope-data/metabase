# Test the interaction of metabase and database

This tutorial explains how you can test the interaction of metabase and
database using the frontend as graphical user interface.

For convenience, the GraphQL queries and mutations are added at the beginning of each step. If something should fail in the frontend, use the GraphQL endpoint of the backend.

This tutorial can be used to validate your setup in a) `develop`, b) `staging` and c) `production`. If there is a difference between the three cases, it is described for a) , b) and c).

## Prepare for the tutorial

1.  Create the product data network with a `metabase` and a `database`. 
   - a) Follow the sections `Development>Getting Started` of [metabase](https://github.com/building-envelope-data/metabase?tab=readme-ov-file#getting-started)
   and [database](https://github.com/building-envelope-data/database?tab=readme-ov-file#getting-started). The sections use the repository 
   [machine](https://github.com/building-envelope-data/machine).
   - b) and c) Follow the sections `Deployment>Setting up a production machine` of [metabase](https://github.com/building-envelope-data/metabase?tab=readme-ov-file#getting-started)
   and [database](https://github.com/building-envelope-data/database?tab=readme-ov-file#getting-started). The sections use the repository 
   [machine](https://github.com/building-envelope-data/machine).
1. Within each project [machine](https://github.com/building-envelope-data/machine), 
   create a user for restricted areas like staging and email, for example with 
   `./deploy.mk user NAME=userRestrictedAreas`.

## Search `database` via `metabase`

This works if your `database` contains an optical dataset and is connected to the `metabase`.
1. Open the frontend of the `metabase`.
   - a) https://www.local.buildingenvelopedata.org:7001
   - b) https://staging.buildingenvelopedata.org
   - c) https://www.buildingenvelopedata.org
1. `allOpticalData`: Click on `Data>Optical Data`. If you find your optical
   dataset there, the connection between `metabase` and `database` works.
   - a) https://www.local.buildingenvelopedata.org:7001/data/optical
   - b) https://staging.buildingenvelopedata.org/data/optical
   - c) https://www.buildingenvelopedata.org/data/optical

## Create a dataset as admin

1. `loginUser`: Use the username `administrator@buildingenvelopedata.org` and
   the `BOOTSTRAP_USER_PASSWORD` which you find in your project `metabase` in `./.env` to sign in as admin at the metabase. Click on `Administrator`
   - a) https://www.local.buildingenvelopedata.org:7001/connect/client/login
   - b) https://staging.buildingenvelopedata.org/connect/client/login
   - c) https://www.buildingenvelopedata.org/connect/client/login
1. Click on `Administrator>Profile` and save the UUID of this user. You will 
   need it later as `creatorId`. 
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
1. `loginUser`: Switch to the database. Use the username 
   `administrator@buildingenvelopedata.org` and the `BOOTSTRAP_USER_PASSWORD` 
   which you find in your project `database` in ./.env` to sign in as admin at the database.
   - a) https://www.local.solarbuildingenvelopes.com:7001/connect/client/login
   - b) https://staging.solarbuildingenvelopes.com/connect/client/login
   - c) https://www.solarbuildingenvelopes.com/connect/client/login
1. Open the GraphQL endpoint of the `database`.
   - a) https://www.local.solarbuildingenvelopes.com:7001/graphql
   - b) https://staging.solarbuildingenvelopes.com/graphql
   - c) https://www.solarbuildingenvelopes.com/graphql
1. Copy the attached mutation into the field `Request`. Replace the UUIDs by 
   your saved `componentId`, `methodId`, `creatorId` and `dataFormatId`. Click
   on `Run`. You have successfully created the dataset if the response contains 
   `"errors": null`. If not, please follow the error messages.
   ```
   mutation {
      createOpticalData(
         input: {
            name: "Optical dataset for testing purposes"
            componentId: "7cedc6f7-89d4-4b1d-9215-743b283b2102"
            appliedMethod: {
            arguments: []
            methodId: "907e3366-08ea-478c-ae33-6086938508ba"
            sources: []
            }
            cielabColors: []
            colorRenderingIndices: []
            createdAt: "2026-05-12T13:45:30.000Z"
            creatorId: "5320d6fb-b96d-4aeb-a24c-eb7036d3437a"
            description: "This optical dataset is only used for testing purposes"
            infraredEmittances: []
            locale: "en-US"
            nearnormalHemisphericalSolarReflectances: [0.1,0.1]
            nearnormalHemisphericalSolarTransmittances: [0.7,0.7]
            nearnormalHemisphericalVisibleReflectances: [0.1,0.1]
            nearnormalHemisphericalVisibleTransmittances: [0.7,0.7]
            rootResource: {
            archivedFilesMetaInformation: []
            description: "BED-JSON"
            dataFormatId: "7e302757-0f43-4469-b058-b9c5a1f9f29a"
            }
            warnings: []
         }
      ) {
         opticalData {
            uuid
            resourceTree{
            root {
               value {
                  archivedFilesMetaInformation {
                  dataFormatId
                  path
                  }
                  description
                  dataFormatId
                  hashValue
                  id
                  locator
                  uuid
               }
               vertexId
            }
            }
         }
         errors {
            code
            message
            path
         }
      }
   }
   ```
1. Follow the instructions of the section [Search `database` via `metabase`](#search-database-via-metabase).


#########
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