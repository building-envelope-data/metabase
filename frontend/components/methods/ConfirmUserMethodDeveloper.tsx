import { useMutation } from "@apollo/client/react";
import { Scalars } from "../../__generated__/graphql";
import {
  ConfirmUserMethodDeveloperDocument,
  ConfirmUserMethodDeveloperMutation,
} from "../../queries/userMethodDevelopers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ConfirmButton from "../ConfirmButton";

interface Props {
  methodId: Scalars["Uuid"]["input"];
  userId: Scalars["Uuid"]["input"];
}

export default function ConfirmUserMethodDeveloper({
  methodId,
  userId,
}: Props) {
  const [confirmUserMethodDeveloperMutation] = useMutation(
    ConfirmUserMethodDeveloperDocument,
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<ConfirmUserMethodDeveloperMutation>({
      getErrors: (data) => data.confirmUserMethodDeveloper.errors,
    });

  const confirm = async () => {
    withMutationHandler(
      () =>
        confirmUserMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
              userId: userId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return <ConfirmButton type="icon" onClick={confirm} loading={mutating} />;
}
