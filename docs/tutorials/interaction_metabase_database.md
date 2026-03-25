# Test the interaction of metabase and database

1. Follow the sections `Getting Started` of [metabase](https://github.com/building-envelope-data/metabase?tab=readme-ov-file#getting-started)
   and [database](https://github.com/building-envelope-data/database?tab=readme-ov-file#getting-started). The sections use the repository 
   [machine](https://github.com/building-envelope-data/machine).
1. Within the project [machine](https://github.com/building-envelope-data/machine), 
   create a user for restricted areas like staging and email, for example with 
   `./deploy.mk user NAME=userRestrictedAreas`.
1. Register as metabase user
   https://www.local.buildingenvelopedata.org:7001.
1. Use your account for restricted areas to open 
   https://www.local.buildingenvelopedata.org:7001/email/ . Open the registration email and use the link to confirm your metabase user account.
1. [Login in](https://www.local.buildingenvelopedata.org:7001/users/login) with 
   your metabase account. 
1. [Create an institution](https://www.local.buildingenvelopedata.org:7001/institutions/create).
   Use the UUID of the institution to verify the pending institution with
   ```

   ```
1. Add a component, a method, a data format and a database to this institution. The database must include the URL of the GraphQL endpoint e.g. https://www.local.solarbuildingenvelopes.com:7501/graphql/ . Copy the UUIDs of the database and of your institution and paste it into the .env file of your `database` project. In the folder of your database, run `make down && make build up`. Sign in on https://www.local.solarbuildingenvelopes.com:7501/ with your metabase account. Create an optical dataset on https://www.local.solarbuildingenvelopes.com:7501/graphql/ using the mutation `createOpticalData`. List all pending optical datasets with the query `allPendingOpticalData`. Publish your optical dataset with the mutation `publishData`. Search your database for your optical dataset e.g. with the query `allOpticalData`. Search the metabase for your optical dataset e.g. with the query `databases{edges{node{allOpticalData`.