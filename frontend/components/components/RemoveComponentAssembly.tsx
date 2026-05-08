import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  RemoveComponentAssemblyDocument,
  RemoveComponentAssemblyMutation,
} from "../../queries/componentAssemblies.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  assembledComponentId: Scalars["Uuid"]["input"];
  partComponentId: Scalars["Uuid"]["input"];
}

export default function RemoveComponentAssembly({
  assembledComponentId,
  partComponentId,
}: Props) {
  const [removeComponentAssemblyMutation] = useMutation(
    RemoveComponentAssemblyDocument,
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
    <SafeDeleteButton
      type="icon"
      kind="remove"
      onConfirm={remove}
      deleting={mutating}
    />
  );
}
