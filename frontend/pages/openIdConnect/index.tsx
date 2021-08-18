import Layout from "../../components/Layout";
import { Table, message, Typography } from "antd";
import { useOpenIdConnectQuery } from "../../queries/openIdConnect.graphql";
import { useEffect } from "react";

// TODO Load and display scopes.

function Page() {
  const { loading, error, data } = useOpenIdConnectQuery();

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Title>Applications</Typography.Title>
      <Table
        loading={loading}
        columns={[
          {
            title: "clientId",
            dataPage: "clientId",
            key: "clientId",
          },
          {
            title: "clientSecret",
            dataPage: "clientSecret",
            key: "clientSecret",
          },
          {
            title: "concurrencyToken",
            dataPage: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "consentType",
            dataPage: "consentType",
            key: "consentType",
          },
          {
            title: "displayName",
            dataPage: "displayName",
            key: "displayName",
          },
          {
            title: "displayNames",
            dataPage: "displayNames",
            key: "displayNames",
          },
          {
            title: "id",
            dataPage: "id",
            key: "id",
          },
          {
            title: "permissions",
            dataPage: "permissions",
            key: "permissions",
          },
          {
            title: "postLogoutRedirectUris",
            dataPage: "postLogoutRedirectUris",
            key: "postLogoutRedirectUris",
          },
          {
            title: "properties",
            dataPage: "properties",
            key: "properties",
          },
          {
            title: "redirectUris",
            dataPage: "redirectUris",
            key: "redirectUris",
          },
          {
            title: "requirements",
            dataPage: "requirements",
            key: "requirements",
          },
          {
            title: "type",
            dataPage: "type",
            key: "type",
          },
          //   {
          //     title: "authorizations",
          //     dataPage: "authorizations",
          //     key: "authorizations",
          //     render: (_value, record, _index) => (
          //     ),
          //   },
        ]}
        dataSource={data?.openIdConnectApplications || []}
      />
      <Typography.Title>Authorizations</Typography.Title>
      <Table
        loading={loading}
        columns={[
          {
            title: "concurrencyToken",
            dataPage: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "creationDate",
            dataPage: "creationDate",
            key: "creationDate",
          },
          {
            title: "id",
            dataPage: "id",
            key: "id",
          },
          {
            title: "properties",
            dataPage: "properties",
            key: "properties",
          },
          {
            title: "scopes",
            dataPage: "scopes",
            key: "scopes",
          },
          {
            title: "status",
            dataPage: "status",
            key: "status",
          },
          {
            title: "subject",
            dataPage: "subject",
            key: "subject",
          },
          {
            title: "type",
            dataPage: "type",
            key: "type",
          },
          //   {
          //     title: "tokens",
          //     dataPage: "tokens",
          //     key: "tokens",
          //     render: (_value, record, _index) => (
          //     ),
          //   },
        ]}
        dataSource={data?.openIdConnectAuthorizations || []}
      />
      <Typography.Title>Tokens</Typography.Title>
      <Table
        loading={loading}
        columns={[
          {
            title: "concurrencyToken",
            dataPage: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "creationDate",
            dataPage: "creationDate",
            key: "creationDate",
          },
          {
            title: "expirationDate",
            dataPage: "expirationDate",
            key: "expirationDate",
          },
          {
            title: "id",
            dataPage: "id",
            key: "id",
          },
          {
            title: "payload",
            dataPage: "payload",
            key: "payload",
          },
          {
            title: "properties",
            dataPage: "properties",
            key: "properties",
          },
          {
            title: "redemptionDate",
            dataPage: "redemptionDate",
            key: "redemptionDate",
          },
          {
            title: "referenceId",
            dataPage: "referenceId",
            key: "referenceId",
          },
          {
            title: "status",
            dataPage: "status",
            key: "status",
          },
          {
            title: "subject",
            dataPage: "subject",
            key: "subject",
          },
          {
            title: "type",
            dataPage: "type",
            key: "type",
          },
        ]}
        dataSource={data?.openIdConnectTokens || []}
      />
      <Typography.Title>Scopes</Typography.Title>
      <Table
        loading={loading}
        columns={[
          {
            title: "concurrencyToken",
            dataPage: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "description",
            dataPage: "description",
            key: "description",
          },
          {
            title: "descriptions",
            dataPage: "descriptions",
            key: "descriptions",
          },
          {
            title: "displayName",
            dataPage: "displayName",
            key: "displayName",
          },
          {
            title: "displayNames",
            dataPage: "displayNames",
            key: "displayNames",
          },
          {
            title: "id",
            dataPage: "id",
            key: "id",
          },
          {
            title: "name",
            dataPage: "name",
            key: "name",
          },
          {
            title: "properties",
            dataPage: "properties",
            key: "properties",
          },
          {
            title: "resources",
            dataPage: "resources",
            key: "resources",
          },
        ]}
        dataSource={data?.openIdConnectScopes || []}
      />
    </Layout>
  );
}

export default Page;
