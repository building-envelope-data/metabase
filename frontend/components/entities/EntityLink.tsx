import { Tooltip } from "antd";
import { Scalars } from "../../__generated__/graphql";
import { Route } from "next";
import Link from "next/link";
import Copyable from "../Copyable";

export default function EntityLink({
  entity,
  route,
}: {
  entity: {
    uuid: Scalars["Uuid"]["output"];
    name: string;
  };
  route: (id: Scalars["Uuid"]["output"]) => Route;
}) {
  return (
    <Tooltip
      title={<Copyable text={entity.uuid} color="white" />}
      styles={{
        container: {
          whiteSpace: "nowrap",
          minWidth: "max-content",
          maxWidth: "none",
        },
      }}
    >
      <Link href={route(entity.uuid)}>{entity.name}</Link>
    </Tooltip>
  );
}
