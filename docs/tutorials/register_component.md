# Register a component at the metabase as a new user

When you have a component and you want to create a unique identifier at the existing 
metabase, this is your tutorial.

If you want to test this feature, please raise an issue that `@christoph-maurer`
shall give you access to the staging environment. Please be aware that the staging
environment can always change due to software development. Follow the option
a) `staging` in this tutorial.

If you want to register a component to add product data for an unlimited time range,
you can follow the option b) `production` in this tutorial.

## Contents

## Register as new user and create an institution

1. `registerUser`: Register as new user at the metabase.
   - a) https://staging.buildingenvelopedata.org/users/register
   - b) https://www.buildingenvelopedata.org/users/register
1. Confirm your account by opening a link in a browser.
   - a) Use your account for restricted areas to open 
      https://staging.buildingenvelopedata.org/email/ . Open the registration 
      email and use the link to confirm your metabase user account.
   - b) Open the registration email in your mailbox and use the link to confirm
      your account.

## Create a new institution

1. Sign in at the metabase.
   - a) https://staging.buildingenvelopedata.org/connect/client/login
   - b) https://www.buildingenvelopedata.org/connect/client/login
1. `createInstitution`: Create a new institution.
   - a) https://staging.buildingenvelopedata.org/institutions
   - b) https://www.buildingenvelopedata.org/institutions
1. Save the `institutionId` of the new institution.
1. Wait until an administrator verifies the new institution. If you don't know an 
   administrator personally, please raise an issue and explain to `@christoph-maurer` 
   how he can verify that you represent the institution.

## Create a new component

1. Sign in at the metabase.
   - a) https://staging.buildingenvelopedata.org/connect/client/login
   - b) https://www.buildingenvelopedata.org/connect/client/login
1. Open the page of your institution.
   - a) https://staging.buildingenvelopedata.org/institutions/${instituionId}
   - b) https://www.buildingenvelopedata.org/institutions/${instituionId}
1. `createComponent`: Create a new component which is manufactured by your new
   institution. Save the `componentId`.

## Create a method and a data format

If you want to create datasets for your new component in a product data server, then you need to specify the method how this dataset was generated and the data format of the dataset.

If there such a method and such a data format already exist in the metabase, you can use their identifiers. Else you can create a new method and a new data format

1. Sign in at the metabase.
   - a) https://staging.buildingenvelopedata.org/connect/client/login
   - b) https://www.buildingenvelopedata.org/connect/client/login
1. Open the page of your institution.
   - a) https://staging.buildingenvelopedata.org/institutions/${instituionId}
   - b) https://www.buildingenvelopedata.org/institutions/${instituionId}
1. `createMethod`: Create a new method which is managed by your new institution.
   Save the `methodId`.
1. `createDataFormat`: Create a new data format which is managed by your new 
   institution. Save the `dataFormatId`.
