import { useMutation } from "@apollo/client/react";
import { App, Typography } from "antd";
import {
  ResetApplicationClientSecretDocument,
  ResetApplicationClientSecretMutation,
} from "../../../queries/openIdConnect.generated";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../../SafeDeleteButton";
import CodeViewer from "../../CodeViewer";

interface ResetApplicationClientSecretProps {
  applicationId: Scalars["Uuid"]["input"];
}

export default function ResetOpenIdConnectApplicationClientSecret({
  applicationId,
}: ResetApplicationClientSecretProps) {
  const { modal } = App.useApp();

  const [resetApplicationClientSecretMutation] = useMutation(
    ResetApplicationClientSecretDocument,
  );

  const { mutating, withMutationHandler, messageMissingModel, messageErrors } =
    useMutationHandler<ResetApplicationClientSecretMutation>({
      getErrors: (data) =>
        data.resetOpenIdConnectApplicationClientSecret.errors,
    });

  const mutate = async () => {
    withMutationHandler(
      () =>
        resetApplicationClientSecretMutation({
          variables: {
            input: {
              applicationId: applicationId,
            },
          },
        }),
      {
        onSuccess: (data) => {
          if (!data?.resetOpenIdConnectApplicationClientSecret?.clientSecret) {
            messageMissingModel();
          } else {
            modal.info({
              title: "Reset Client Secret",
              centered: true,
              width: 500,
              content: (
                <Typography.Paragraph style={{ maxWidth: "75ch" }}>
                  Please copy and save the following client secret now, you will
                  not be able to access it later
                  <CodeViewer
                    code={
                      data.resetOpenIdConnectApplicationClientSecret
                        .clientSecret
                    }
                  />
                </Typography.Paragraph>
              ),
            });
          }
        },
        onError: messageErrors,
      },
    );
  };

  return (
    <SafeDeleteButton type="default" deleting={mutating} onConfirm={mutate}>
      Reset Client Secret
    </SafeDeleteButton>
  );
}
