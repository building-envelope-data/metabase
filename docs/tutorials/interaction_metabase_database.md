# Test the interaction of metabase and database

This tutorial explains how you can test the interaction of metabase and
database using the frontend as graphical user interface.

After [preparation](#prepare-for-the-tutorial), the tutorial starts with the [simplest test](#search-database-via-metabase). Each following test adds an additional feature. The [most detailed test](#set-the-access-rights-of-a-dataset-in-database) therefore covers the widest range of features. 

For convenience, the GraphQL queries and mutations are added at the beginning of each step. If something should fail in the frontend, try the GraphQL endpoint of the backend.

This tutorial can be used to validate your setup in a) `develop`, b) `staging` and c) `production`. If there is a difference between the three cases, it is described for a) , b) and c).

## Contents

[Prepare for the tutorial](#prepare-for-the-tutorial)

[Search `database` via `metabase`](#search-database-via-metabase) - the simplest test, covering one feature

[Create a dataset as admin](#create-a-dataset-as-admin)

[Register as new user and create an institution](#register-as-new-user-and-create-an-institution)

[Add a new database](#add-a-new-database)

[Set the access rights of a dataset in `database`](#set-the-access-rights-of-a-dataset-in-database) - the most detailed test, covering the widest range of features

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
1. When you are not sure about the meaning of a term, consult the [API 
   specification of the metabase](https://github.com/building-envelope-data/api/blob/develop/apis/metabase.graphql) and the [API specification of product data servers](https://github.com/building-envelope-data/api/blob/develop/apis/database.graphql) which contain all details.

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
   the `BOOTSTRAP_USER_PASSWORD` which you find in your project `metabase` in `./.env` to sign in as admin at the metabase.
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
1. `createOpticalData`: Copy the attached mutation into the field `Request`. 
   Replace the UUIDs by your saved `componentId`, `methodId`, `creatorId` and 
   `dataFormatId`. Click on `Run`. You have successfully created the dataset if 
   the response contains `"errors": null`. If not, please follow the error 
   messages.
   ```graphql
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
                     description
                     dataFormatId
                     hashValue
                     id
                     locator
                     uuid
                  }
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
1. The response should be similar to the attached response. Save `uuid` of the 
   dataset `data/createOpticalData/opticalData/uuid` and the `uuid` of the resource `data/createOpticalData/opticalData/resourceTree/root/value/uuid`.
   ```json
   {
      "data": {
         "createOpticalData": {
            "opticalData": {
               "uuid": "e8783902-9895-4b94-bf14-a7f48de1dcb6",
               "resourceTree": {
                  "root": {
                     "value": {
                        "description": "BED-JSON",
                        "dataFormatId": "7e302757-0f43-4469-b058-b9c5a1f9f29a",
                        "hashValue": "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855",
                        "id": "R2V0SHR0cHNSZXNvdXJjZTp+YTL727lDQLixD+GDQw4y",
                        "locator": "https://www.solar-in-the-facade.com/api/resources/fb32617e-b9db-4043-b8b1-0fe183430e32",
                        "uuid": "fb32617e-b9db-4043-b8b1-0fe183430e32"
                     }
                  }
               }
            },
            "errors": null
         }
      }
   }
   ```
1. `uploadFile`: In the frontend of the database, click on your user and 
   `Upload File`. Enter the uuid of the resource from the previous step and 
   upload the dataset, for example a JSON file.
   - a) https://www.local.solarbuildingenvelopes.com:7501/upload-file
   - b) https://staging.solarbuildingenvelopes.com/upload-file
   - c) https://www.solarbuildingenvelopes.com/upload-file
1. `publishData`: Open a new document in the GraphQL endpoint and copy the 
   attached mutation into the field `Request`. Replace the uuid of the `dataId` 
   by the `uuid` of the dataset. Click on `Run` to publish you pending dataset. 
   If the response contains `"errors": null`, you have been successful. If not, 
   please follow the error messages.
   ```graphql
   mutation {
      publishData(
         input: {
            dataId: "4d9a00cd-6146-4f2b-8dbc-dc6b01a37037"
            dataKind: OPTICAL_DATA
         }
      ) {
         errors {
            code
            message
            path
         }
      }
   }
   ```
1. Follow the instructions of the section [Search `database` via `metabase`](#search-database-via-metabase).

## Register as new user and create an institution

1. `registerUser`: Register as new user at the metabase.
   - a) https://www.local.buildingenvelopedata.org:7001/users/register
   - b) https://staging.buildingenvelopedata.org/users/register
   - c) https://www.buildingenvelopedata.org/users/register
1. Confirm your account by opening a link in a browser.
   - a) Use your account for restricted areas to open 
      https://www.local.buildingenvelopedata.org:7001/email/ . Open the registration email and use the link to confirm your account.
   - b) Use your account for restricted areas to open 
      https://staging.buildingenvelopedata.org/email/ . Open the registration 
      email and use the link to confirm your metabase user account.
   - c) Open the registration email in your mailbox and use the link to confirm
      your account.
1. Use your account to follow the instructions of the section [Create a dataset 
   as admin](#create-a-dataset-as-admin), but with the following change:
      - `verifyInstitution`: After the step `createInstitution`, log out of 
      your account and login as `administrator@buildingenvelopedata.org` with 
      the `BOOTSTRAP_USER_PASSWORD` which you find in your project `metabase` in
      `./.env`. Verify the newly created institution. Then log out and log in with your account.

## Add a new database

1. `loginUser`: After [registering as a new user](#register-as-new-user), sign 
   in at the metabase with your account.  
   - a) https://www.local.buildingenvelopedata.org:7001/connect/client/login
   - b) https://staging.buildingenvelopedata.org/connect/client/login
   - c) https://www.buildingenvelopedata.org/connect/client/login
1. `createDatabase`: Go to your institution - either with the tab 
   `Institutions` or your user `Profile`. Switch to the tab 
   `Operated Databases` and click on `New Database`. Enter all required fields including the `Locator` which is the URL of the GraphQL endpoint of your database. Click on `Create`.
1. If you want to add a product data server that can add and update components
   and institutions in the metabase, then stay being logged-in and open the
   [endpoint of the metabase](https://www.buildingenvelopedata.org/graphql/).
   Send a mutation like
   ```graphql
   mutation {
      createOpenIdConnectApplication(
         input: {
            clientId: "${YOUR_INSTITUTION_NAME}"
            consentType: EXPLICIT
            displayName: "${YOUR_INSTITUTION_NAME}"
            endpoints: [AUTHORIZATION, PUSHED_AUTHORIZATION, INTROSPECTION,
               END_SESSION, REVOCATION, TOKEN]
            grantTypes: [AUTHORIZATION_CODE, REFRESH_TOKEN]
            institutionId: "${UUID_OF_YOUR_INSTITUTION}"
            postLogoutRedirectUri: "https://${HOST_OF_YOUR_PRODUCT_DATA_SERVER}/connect/
               callback/logout/metabase"
            redirectUri: "https://${HOST_OF_YOUR_PRODUCT_DATA_SERVER}/connect/
               callback/login/metabase"
            responseTypes: [CODE]
            scopes: [PROFILE, READ_API, MANAGE_DATABASE_API, WRITE_API]
         }
      ) {
         clientSecret
         errors {
            code
            message
            path
         }
      }
   }
   ```
   to the endpoint. Make sure that you exchange the variables (`${...}`) 
   according to your institution. For `${UUID_OF_YOUR_INSTITUTION}` please use 
   the UUID which your institution has received when it was created. For 
   `https://${HOST_OF_YOUR_PRODUCT_DATA_SERVER}` you need to enter the URL of 
   your product data server. The redirect URIs define how the metabase returns 
   to your product data server when signing in. 
1. Stay in the tab `Operated Databases` and click on your pending database.   
   Take the verification code and update your database so that it returns this 
   verification code when receiving the GraphQL `query { verificationCode }`. 
   This proves that you control the new database. Then, press the “Verify” button.

## Set the access rights of a dataset in `database`

1. The [API specification of the `database`](https://github.com/
   building-envelope-data/database/blob/develop/frontend/type-defs.graphqls) includes the following description of setting the data access rights:
   <details>
   <summary>Detailed General Description</summary>
   A data access policy decides who can access data, meaning which data shows up
   in GraphQL queries and which associated resources can be downloaded. The
   decision is made based on the authenticated user or institution, the
   institutions represented by a user, and/or the communicating OpenID Connect
   application. The access token issued by the metabase and associated with
   same-site logins or given in the HTTP Authorization header as Bearer, tells
   through the 'Subject' claim which user or institution is authenticated and
   through the 'Client ID' or 'Authorized Party' claim which OpenID Connect
   application is communicating. In the case of a user, the metabase informs us
   about the institutions he*she represents.

   How user, institution(s), and application are allowed/restricted is decided 
   by user, institution, and application access policies associated with the 
   data access policy and specific users, institutions, and applications 
   through their IDs or users and institutions and client IDs for applications. 
   Each such policy can allow access to a user, institution, or application and 
   can limit the number of API accesses totally or within a time span, which is 
   shifted to the current moment once it has passed. For example, an 
   institution policy without a limit just allows access for any user 
   representing that institution and for any application owned by the 
   institution itself, and it disallows access for all other users and 
   institutions. If an additional limit without duration is given,
   than exactly that number of GraphQL or REST API accesses are allowed. And if 
   an additional duration is given, from the time of the first access until the
   duration passed, the given number of accesses are allowed; the start time and
   the access count are reset on the first access after the duration passed (a
   sliding window of time).

   The individual decisions based on user, institution, and OpenID Connect
   application, can be combined conjunctively ('and' or 'all' need to be 
   positive) or disjunctively ('or' or 'any one'/'at least one' needs to be 
   positive). This is configured through the combinator and the mutation
   `ConfigureDataAccessPolicyMutation`. If there are no user access
   policies at all, then, in the 'all' case, no restrictions based on the user
   itself are imposed, and in the 'any' case, no allowances based on the user
   itself are given; and analogously for institution and application policies. 
   Put another way, an empty list of user access policies is `true` in the 
   'and' case and `false` in the 'or' case, and analogously for institution and 
   application access policies.

   In particular, a data access policy with the combinator 'all' and empty user,
   institution, and application policies allows access to anyone, also anonymous
   access. And one with the combinator 'or' and empty policies allows access to
   no-one, no matter if authenticated or not.

   A data access policy is either the one-and-only global one or associated 
   with a specific data entry, see the field `Data`. It is
   global if this field is `null`. The global and individual policies are 
   combined conjunctively, meaning that for access both need to allow access.
   </details>
1. `loginUser`: Use the your user account from section [Register as a new user]
   (#register-as-new-user-and-create-an-institution) to sign in at the your 
   product data server. When you want to change the data access policies, your 
   user must be part of the institution which operates this product data server.
   - a) https://www.local.solarbuildingenvelopes.com:7001/connect/client/login
   - b) https://staging.solarbuildingenvelopes.com/connect/client/login
   - c) https://www.solarbuildingenvelopes.com/connect/client/login
1. Open the GraphQL endpoint of the `database`.
   - a) https://www.local.solarbuildingenvelopes.com:7001/graphql
   - b) https://staging.solarbuildingenvelopes.com/graphql
   - c) https://www.solarbuildingenvelopes.com/graphql
1. Get an overview about the current access rights in your product data server 
   with
   <details>
   <summary>Detailed Query</summary>
   ```graphql
   query {
      dataAccessPolicies {
         totalCount
         pageInfo {
            hasNextPage
         }
         edges {
            node {
               combinator
               isGlobal
               data {
                  uuid
                  kind
               }
               isEveryoneAllowed
               isAccessAllowed(
                  userId: null
                  institutionIds: ["5320d6fb-b96d-4aeb-a24c-eb7036d3437a"]
                  openIdConnectClientId: null
               )
               institutionAccessPolicies {
                  edges {
                     node {
                        institutionId
                        isAlwaysAllowed
                        isAccessAllowed(institutionIds: ["5320d6fb-b96d-4aeb-a24c-eb7036d3437a"])
                        isWithinAccessLimitInTimeSpan
                        isWithinTimeSpan
                        upperAccessLimitPerTimeDuration {
                           upperLimit
                           duration
                        }
                        accessCountSinceStartTime {
                           accessCount
                           startTime
                        }
                     }
                  }
               }
               userAccessPolicies {
                  edges {
                     node {
                        userId
                        isAlwaysAllowed
                        isAccessAllowed(userId: null)
                        isWithinAccessLimitInTimeSpan
                        isWithinTimeSpan
                        upperAccessLimitPerTimeDuration {
                           upperLimit
                           duration
                        }
                        accessCountSinceStartTime {
                           accessCount
                           startTime
                        }
                     }
                  }
               }
               openIdConnectApplicationAccessPolicies {
                  edges {
                     node {
                        clientId
                        isAlwaysAllowed
                        isAccessAllowed(openIdConnectClientId: null)
                        isWithinAccessLimitInTimeSpan
                        isWithinTimeSpan
                        upperAccessLimitPerTimeDuration {
                           upperLimit
                           duration
                        }
                        accessCountSinceStartTime {
                           accessCount
                           startTime
                        }
                     }
                  }
               }
            }
         }
      }
   }
   ```
   </details>
   Each dataset has a access policy. For example,
   ```json
   "edges": [
      {
         "node": {
         "combinator": "ALL",
         "isGlobal": false,
         "data": {
            "uuid": "019ed4e9-edfb-7fcb-b8c0-4d57da0adf10",
            "kind": "OPTICAL_DATA"
         },
         "isEveryoneAllowed": true,
         "isAccessAllowed": true,
         "institutionAccessPolicies": [],
         "userAccessPolicies": [],
         "openIdConnectApplicationAccessPolicies": []
         }
      },
   ```
   means that the dataset with the UUID 019ed4e9-edfb-7fcb-b8c0-4d57da0adf10 
   does neither have an `institutionAccessPolicy` nor a `userAccessPolicy` nor 
   an `openIdConnectApplicationAccessPolicy`. When the combinator is defined as 
   `ALL`, undefined accessPolicies default to `true` and they are combined with 
   `AND`. Therefore, the dataAccessPolicy of this dataset evaluates to `true` 
   and the data access is allowed to everyone. 

   If the combinator is `SOME`, then undefined accessPolicies default to `false` and they are combined with `OR`. In this case, no-one would have access to the dataset 019ed4e9-edfb-7fcb-b8c0-4d57da0adf10.
1. With
   ```graphql
   isAccessAllowed(
      userId: null
      institutionIds: ["5320d6fb-b96d-4aeb-a24c-eb7036d3437a"]
      openIdConnectClientId: null
   )
   ```
   you can check if the institution with the UUID 5320d6fb-b96d-4aeb-a24c-eb7036d3437a has access to the dataset.
1. You can also check the data access rights with your user. If you are not 
   allowed to access the dataset, a query for the dataset will provide no 
   result. But as a member of the institution which operates the product data server, you can always change the data access policies to give you access to the datasets.
1. Set the access rights of the optical dataset   
   e068d8f9-9e2c-4695-b5fc-16992041040f so that institution 
   a11b2f32-a270-4caf-8eae-1d47ebba3274 can access it up to 10 times per 
   minute when they use the "LambdaWork" with its OpenIdConnectApplication. The 
   duration of of LambdaWork is set to null which means that there is no   
   limitation in time, but an upper limit of 1000. If the upper limit would be 
   `null`, then all users of the institution 
   a11b2f32-a270-4caf-8eae-1d47ebba3274 would have no limit to access this 
   dataset when they use LambdaWork.
   ```graphql
   mutation {
      setInstitutionAccessPolicy(
         input: {
            institutionId: "a11b2f32-a270-4caf-8eae-1d47ebba3274"
            data: {
            dataId: "e068d8f9-9e2c-4695-b5fc-16992041040f"
            dataKind: OPTICAL_DATA
            }
            upperAccessLimitPerTimeDuration: { duration: "PT1M", upperLimit: 10 }
         }
      ) {
         errors {
            message
            path
            code
         }
      }
      setOpenIdConnectApplicationAccessPolicy(
         input: {
            clientId: "LambdaWork"
            data: {
            dataId: "e068d8f9-9e2c-4695-b5fc-16992041040f"
            dataKind: OPTICAL_DATA
            }
            upperAccessLimitPerTimeDuration: { duration: null, upperLimit: 1000 }
         }
      ) {
         errors {
            code
            message
            path
         }
      }
   }
   ```
1. Check the resulting dataAccessPolicy with
   ```graphql
   query {
      dataAccessPolicy(dataId: "e068d8f9-9e2c-4695-b5fc-16992041040f") {
         edges {
            node {
               combinator
               id
               institutionAccessPolicies {
                  institutionId
                  upperAccessLimitPerTimeDuration {
                  duration
                  upperLimit
                  }
               }
               openIdConnectApplicationAccessPolicies {
                  clientId
                  upperAccessLimitPerTimeDuration {
                  duration
                  upperLimit
                  }
               }
               userAccessPolicies {
                  upperAccessLimitPerTimeDuration {
                  duration
                  upperLimit
                  }
                  userId
               }               
            }
         }
      }
   }
   ```
   The combinator `AND` means that the institution   
   a11b2f32-a270-4caf-8eae-1d47ebba3274 looses access to the dataset after 1000 
   accesses. Then, for example, a separate license is needed.
1. Change the combinator of the policies to `SOME`.
   ```graphql
   mutation {
      configureDataAccessPolicy(
         input: {
            combinator: SOME
            data: { dataId: "e068d8f9-9e2c-4695-b5fc-16992041040f", dataKind: OPTICAL_DATA }
         }
      ) {
         errors {
            message
            code
            path
         }
         dataAccessPolicy {
            edges {
               node {
                  combinator                  
               }
            }
         }
      }
   }
   ```
   Now, users which belong to the institution   
   a11b2f32-a270-4caf-8eae-1d47ebba3274 will always have access to the dataset 
   with an upper limit of 10 per minute. In addition, every user of every 
   institution has access to the dataset when using the software LambdaWork 
   until LambdaWork has accessed the dataset in total 1000 times.
1. Use `unsetInstitutionAccessPolicy` to delete a specific 
   InstitutionAccessPolicy. Use `clearInstitutionAccessPolicies` to delete all 
   InstituionAccessPolicies of a dataset. Use `resetDataAccessPolicy` to delete 
   all data access policies of a dataset and set them to the default. With 
   `resetDataAccessPolicies`, the data access policies of several datasets can 
   deleted and set to default.
1. In addition to data access policies of each dataset, you can also set a 
   global data access policy. It is conjunctively connected with the data 
   access policies of the datasets. You can either set the `dataId` of the 
   input to `null` or you do not use the `data` input at all to define a global 
   data access policy which gives only user 
   019d06e8-bbd5-799d-963c-71f1c11ba7e6 access to the datasets.
   ```graphql
   mutation {
      setUserAccessPolicy(
         input: {
            userId: "019d06e8-bbd5-799d-963c-71f1c11ba7e6"
         }
      ) {
         errors {
            code
            message
            path
         }
      }
   }
   ```  
1. Let's reset the data access policies of all datasets and keep only the 
   global data access policy:
   ```graphql
   mutation {
      resetDataAccessPolicies(where: { dataId: { notEqualTo: null } }) {
         errors {
            code
            message
            path
         }
      }
   }
   ```
   Now, user 019d06e8-bbd5-799d-963c-71f1c11ba7e6 has unlimited access to all 
   datasets of this product data server and no other user has access. 
