import { useMutation } from "@apollo/client/react";
import {
  MethodDocument,
  MethodsDocument,
} from "../../queries/methods.generated";
import { Scalars } from "../../__generated__/graphql";
import { InstitutionDocument } from "../../queries/institutions.generated";
import {
  RemoveInstitutionMethodDeveloperDocument,
  RemoveInstitutionMethodDeveloperMutation,
} from "../../queries/institutionMethodDevelopers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../SafeDeleteButton";

interface Props {
  methodId: Scalars["Uuid"]["input"];
  institutionId: Scalars["Uuid"]["input"];
}

export default function RemoveInstitutionMethodDeveloper({
  methodId,
  institutionId,
}: Props) {
  const [removeInstitutionMethodDeveloperMutation] = useMutation(
    RemoveInstitutionMethodDeveloperDocument,
    {
      refetchQueries: [
        {
          query: MethodsDocument,
        },
        {
          query: MethodDocument,
          variables: {
            uuid: methodId,
          },
        },
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<RemoveInstitutionMethodDeveloperMutation>({
      getErrors: (data) => data.removeInstitutionMethodDeveloper.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        removeInstitutionMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
              institutionId: institutionId,
            },
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <SafeDeleteButton
      type="icon"
      kind="remove"
      onConfirm={mutate}
      deleting={mutating}
    />
  );
}
