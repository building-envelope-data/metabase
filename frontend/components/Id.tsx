import { Scalars } from "../__generated__/graphql";

export default function Id({ value }: { value: Scalars["Uuid"]["output"] }) {
  return <code style={{ whiteSpace: "nowrap" }}>{value}</code>;
}
