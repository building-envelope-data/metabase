import { useMutation } from "@apollo/client/react";
import { App, Typography } from "antd";
import {
  ResetApplicationClientSecretDocument,
  ResetApplicationClientSecretMutation,
} from "../../../queries/openIdConnect.generated";
import { ExclamationCircleTwoTone } from "@ant-design/icons";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";
import SafeDeleteButton from "../../SafeDeleteButton";

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
                  <span>
                    <ExclamationCircleTwoTone twoToneColor="#f9b02e" />{" "}
                  </span>
                  Please copy an save the client secret now, you will not be
                  able to access it later.
                  <p />
                  <Typography.Paragraph copyable>
                    {
                      data.resetOpenIdConnectApplicationClientSecret
                        .clientSecret
                    }
                  </Typography.Paragraph>
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
    <SafeDeleteButton
      title="Reset Client Secret"
      deleting={mutating}
      onConfirm={mutate}
    />
  );
}
