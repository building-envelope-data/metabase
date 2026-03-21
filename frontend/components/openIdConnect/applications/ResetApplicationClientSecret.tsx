import { useMutation } from "@apollo/client/react";
import { Button, App, Typography } from "antd";
import {
  ApplicationDocument,
  ApplicationsDocument,
  ResetApplicationClientSecretDocument,
  ResetApplicationClientSecretMutation,
} from "../../../queries/openIdConnect.generated";
import { ExclamationCircleTwoTone } from "@ant-design/icons";
import { Scalars } from "../../../__generated__/graphql";
import { useMutationHandler } from "../../../lib/hooks/useMutationHandler";

export type ResetApplicationClientSecretProps = {
  applicationId: Scalars["Uuid"]["input"];
};

export default function ResetApplicationClientSecret({
  applicationId,
}: ResetApplicationClientSecretProps) {
  const { modal } = App.useApp();

  const [resetApplicationClientSecretMutation] = useMutation(
    ResetApplicationClientSecretDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: ApplicationsDocument,
        },
        {
          query: ApplicationDocument,
          variables: {
            uuid: applicationId,
          },
        },
      ],
    },
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
            applicationId: applicationId,
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
    <Button danger type="default" onClick={mutate} loading={mutating}>
      Reset Client Secret
    </Button>
  );
}
