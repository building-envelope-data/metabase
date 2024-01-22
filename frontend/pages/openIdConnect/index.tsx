import { messageApolloError } from "../../lib/apollo";
import Layout from "../../components/Layout";
import { Table, Typography } from "antd";
import { useOpenIdConnectQuery } from "../../queries/openIdConnect.graphql";
import { useEffect } from "react";

// TODO Load and display scopes.

function Page() {
  const { loading, error, data } = useOpenIdConnectQuery();

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  return (
    <Layout>
      <Typography.Title>Applications</Typography.Title>
      <Table
        loading={loading}
        columns={[
          {
            title: "applicationType",
            dataIndex: "applicationType",
            key: "applicationType",
          },
          {
            title: "clientId",
            dataIndex: "clientId",
            key: "clientId",
          },
          {
            title: "clientSecret",
            dataIndex: "clientSecret",
            key: "clientSecret",
          },
          {
            title: "clientType",
            dataIndex: "clientType",
            key: "clientType",
          },
          {
            title: "concurrencyToken",
            dataIndex: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "consentType",
            dataIndex: "consentType",
            key: "consentType",
          },
          {
            title: "displayName",
            dataIndex: "displayName",
            key: "displayName",
          },
          {
            title: "displayNames",
            dataIndex: "displayNames",
            key: "displayNames",
          },
          {
            title: "id",
            dataIndex: "id",
            key: "id",
          },
          {
            title: "jsonWebKeySet",
            dataIndex: "jsonWebKeySet",
            key: "jsonWebKeySet",
          },
          {
            title: "permissions",
            dataIndex: "permissions",
            key: "permissions",
          },
          {
            title: "postLogoutRedirectUris",
            dataIndex: "postLogoutRedirectUris",
            key: "postLogoutRedirectUris",
          },
          {
            title: "properties",
            dataIndex: "properties",
            key: "properties",
          },
          {
            title: "redirectUris",
            dataIndex: "redirectUris",
            key: "redirectUris",
          },
          {
            title: "requirements",
            dataIndex: "requirements",
            key: "requirements",
          },
          {
            title: "settings",
            dataIndex: "settings",
            key: "settings",
          },
          //   {
          //     title: "authorizations",
          //     dataIndex: "authorizations",
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
            dataIndex: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "creationDate",
            dataIndex: "creationDate",
            key: "creationDate",
          },
          {
            title: "id",
            dataIndex: "id",
            key: "id",
          },
          {
            title: "properties",
            dataIndex: "properties",
            key: "properties",
          },
          {
            title: "scopes",
            dataIndex: "scopes",
            key: "scopes",
          },
          {
            title: "status",
            dataIndex: "status",
            key: "status",
          },
          {
            title: "subject",
            dataIndex: "subject",
            key: "subject",
          },
          {
            title: "type",
            dataIndex: "type",
            key: "type",
          },
          //   {
          //     title: "tokens",
          //     dataIndex: "tokens",
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
            dataIndex: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "creationDate",
            dataIndex: "creationDate",
            key: "creationDate",
          },
          {
            title: "expirationDate",
            dataIndex: "expirationDate",
            key: "expirationDate",
          },
          {
            title: "id",
            dataIndex: "id",
            key: "id",
          },
          {
            title: "payload",
            dataIndex: "payload",
            key: "payload",
          },
          {
            title: "properties",
            dataIndex: "properties",
            key: "properties",
          },
          {
            title: "redemptionDate",
            dataIndex: "redemptionDate",
            key: "redemptionDate",
          },
          {
            title: "referenceId",
            dataIndex: "referenceId",
            key: "referenceId",
          },
          {
            title: "status",
            dataIndex: "status",
            key: "status",
          },
          {
            title: "subject",
            dataIndex: "subject",
            key: "subject",
          },
          {
            title: "type",
            dataIndex: "type",
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
            dataIndex: "concurrencyToken",
            key: "concurrencyToken",
          },
          {
            title: "description",
            dataIndex: "description",
            key: "description",
          },
          {
            title: "descriptions",
            dataIndex: "descriptions",
            key: "descriptions",
          },
          {
            title: "displayName",
            dataIndex: "displayName",
            key: "displayName",
          },
          {
            title: "displayNames",
            dataIndex: "displayNames",
            key: "displayNames",
          },
          {
            title: "id",
            dataIndex: "id",
            key: "id",
          },
          {
            title: "name",
            dataIndex: "name",
            key: "name",
          },
          {
            title: "properties",
            dataIndex: "properties",
            key: "properties",
          },
          {
            title: "resources",
            dataIndex: "resources",
            key: "resources",
          },
        ]}
        dataSource={data?.openIdConnectScopes || []}
      />
    </Layout>
  );
}

export default Page;
