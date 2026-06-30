import { Scalars } from "../__generated__/graphql";
import paths from "../paths";
import EntityLink from "./entities/EntityLink";

export default function Manager({
  data,
}: {
  data: {
    uuid: Scalars["Uuid"]["input"];
    name: string;
  };
}) {
  return (
    <>
      Managed by <EntityLink entity={data} route={paths.institution} />
    </>
  );
}
