import { useMutation } from "@apollo/client/react";
import { Button, App, Typography, Popconfirm } from "antd";
import {
  ResetApplicationClientSecretDocument,
  ResetApplicationClientSecretMutation,
} from "../../../queries/openIdConnect.generated";
import { ExclamationCircleTwoTone } from "@ant-design/icons";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";

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
                <Typography.Paragraph>
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
    <Popconfirm
      title="Reset Client Secret"
      description="Are you sure?"
      okText="Yes"
      cancelText="No"
      okButtonProps={{ danger: true }}
      onConfirm={mutate}
    >
      <Button danger type="default" loading={mutating}>
        Reset Client Secret
      </Button>
    </Popconfirm>
  );
}
