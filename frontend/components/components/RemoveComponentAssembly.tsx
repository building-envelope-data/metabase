import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveComponentAssemblyDocument,
  RemoveComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import {
  ComponentDocument,
  ComponentsDocument,
} from "../../queries/components.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { Button } from "antd";

interface Props {
  assembledComponentId: Scalars["Uuid"]["input"];
  partComponentId: Scalars["Uuid"]["input"];
}

export function RemoveComponentAssembly({
  assembledComponentId,
  partComponentId,
}: Props) {
  const [removeComponentAssemblyMutation] = useMutation(
    RemoveComponentAssemblyDocument,
    {
      refetchQueries: [
        {
          query: ComponentsDocument,
        },
        {
          query: ComponentDocument,
          variables: {
            uuid: assembledComponentId,
          },
        },
        {
          query: ComponentDocument,
          variables: {
            uuid: partComponentId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveComponentAssemblyMutation>({
      getErrors: (data) => data.removeComponentAssembly.errors,
    });

  const remove = () =>
    withMutationHandler(
      () =>
        removeComponentAssemblyMutation({
          variables: {
            input: {
              assembledComponentId: assembledComponentId,
              partComponentId: partComponentId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );

  return (
    <Button danger onClick={() => remove()} loading={mutating}>
      Remove
    </Button>
  );
}
