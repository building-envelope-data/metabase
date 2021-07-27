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
            title: "UUID",
            dataIndex: "uuid",
            key: "uuid",
            sorter: (a, b) => a.uuid.localeCompare(b.uuid, "en"),
            sortDirections: ["ascend", "descend"],
            render: (_value, record, _index) => <Link href={paths.user(record.uuid)}>{record.uuid}</Link>
          },
          {
            title: "Name",
            dataIndex: "name",
            key: "name",
            // onFilter: (value, user) =>
            //   typeof value === "string" ? user.name.includes(value) : false,
            sorter: (a, b) => a.name.localeCompare(b.name, "en"),
            sortDirections: ["ascend", "descend"],
          },
          {
            title: "Email",
            dataIndex: "email",
            key: "email",
            sorter: (a, b) => a.email && b.email ? a.email.localeCompare(b.email, "en") : 0,
            sortDirections: ["ascend", "descend"],
          },
        ]}
        dataSource={data?.users?.nodes || []}
      />
    </Layout>
  );
}

export default Index;
