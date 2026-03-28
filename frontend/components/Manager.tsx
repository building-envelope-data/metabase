import Link from "next/link";
import { Scalars } from "../__generated__/graphql";
import paths from "../paths";
import { Tooltip } from "antd";

export default function Manager({
  data,
}: {
  data: {
    uuid: Scalars["Uuid"]["input"];
    name: string;
  };
}) {
  return (
    <div>
      Managed by{" "}
      <Tooltip title={data.uuid}>
        <Link href={paths.institution(data.uuid)}>{data.name}</Link>
      </Tooltip>
    </div>
  );
}
