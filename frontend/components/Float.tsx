import Copyable from "./Copyable";
import { Scalars } from "../__generated__/graphql";

export default function Float({
  value,
}: {
  value: Scalars["Float"]["output"];
}) {
  return (
    <Copyable onlyIcon text={value.toString()}>
      {value}
    </Copyable>
  );
}
