import { useQuery } from "@apollo/client/react";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import { Select, SelectProps, Space } from "antd";
import Id from "../Id";

export default function RepresentedInstitutionIdSelect(props: SelectProps) {
  const currentUser = useQuery(CurrentUserDocument)?.data?.currentUser;

  return (
    <Select
      {...props}
      options={currentUser?.representedInstitutions.edges.map((edge) => ({
        value: edge.node.id,
        label: (
          <Space>
            {edge.node.name} (<Id value={edge.node.uuid} />)
          </Space>
        ),
      }))}
    />
  );
}
