import Layout from "../../components/Layout";
import { Table, message } from "antd";
import { useUsersQuery } from "../../queries/users.graphql";
import paths from "../../paths";
import Link from "next/link";
import { useEffect } from "react";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Index() {
  const { loading, error, data } = useUsersQuery();

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  return (
    <Layout>
      <Table
        loading={loading}
        columns={[
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            // onFilter: (value, user) =>
            //   typeof value === "string" ? user.name.includes(value) : false,
            sorter: (a, b) => a.name.length - b.name.length,
            sortDirections: ["ascend", "descend", "ascend"],
            defaultSortOrder: "ascend",
            render: (name, user, _) => (
              <Link href={paths.user(user.uuid)}>{name}</Link>
            ),
          },
          {
            title: "Email",
            dataIndex: "email",
            key: "email",
            sorter: (a, b) =>
              a.email && b.email ? a.email.length - b.email.length : 0,
            sortDirections: ["ascend", "descend", "ascend"],
            defaultSortOrder: "ascend",
          },
        ]}
        dataSource={data?.users?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
