import { useMutation } from "@apollo/client/react";
import {
  GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriDocument,
  GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation,
} from "../../queries/currentUser.generated";
import { Dispatch, SetStateAction, useEffect } from "react";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ManageLayout from "./ManageLayout";
import { Skeleton } from "antd";

interface Props {
  setSharedKey: Dispatch<SetStateAction<string | null | undefined>>;
  setAuthenticatorUri: Dispatch<SetStateAction<string | null | undefined>>;
}

export function GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri({
  setSharedKey,
  setAuthenticatorUri,
}: Props) {
  const [generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation] =
    useMutation(
      GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriDocument,
    );

  const { withMutationHandler, messageErrors } =
    useMutationHandler<GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation>(
      {
        getErrors: (data) =>
          data.generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri.errors,
      },
    );

  useEffect(() => {
    const generate = async () => {
      withMutationHandler(
        () => generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation(),
        {
          onSuccess: (data) => {
            setSharedKey(
              data?.generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri
                ?.sharedKey,
            );
            setAuthenticatorUri(
              data?.generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri
                ?.authenticatorUri,
            );
          },
          onError: messageErrors,
        },
      );
    };
    generate();
  }, [
    generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation,
    messageErrors,
    setAuthenticatorUri,
    setSharedKey,
    withMutationHandler,
  ]);

  return (
    <ManageLayout>
      <Skeleton />
    </ManageLayout>
  );
}
