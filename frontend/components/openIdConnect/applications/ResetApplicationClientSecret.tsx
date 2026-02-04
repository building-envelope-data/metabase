import { useMutation } from "@apollo/client/react";
import { Button, App, Modal, Typography } from "antd";
import { useState } from "react";
import {
  ApplicationDocument,
  ApplicationsDocument,
  ResetApplicationClientSecretDocument,
} from "../../../queries/openIdConnect.generated";
import { ExclamationCircleTwoTone } from "@ant-design/icons";
import { Scalars } from "../../../__generated__/graphql";

export type ResetApplicationClientSecretProps = {
  applicationId: Scalars["Uuid"]["input"];
};

export default function ResetApplicationClientSecret({
  applicationId,
}: ResetApplicationClientSecretProps) {
  const [resetting, setResetting] = useState(false);
  const { message } = App.useApp();

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

  const reset = async () => {
    try {
      setResetting(true);
      const { error, data } = await resetApplicationClientSecretMutation({
        variables: {
          applicationId: applicationId,
        },
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.resetOpenIdConnectApplicationClientSecret?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.resetOpenIdConnectApplicationClientSecret?.errors
            .map((error) => error.message)
            .join(" "),
        );
      } else if (
        data?.resetOpenIdConnectApplicationClientSecret?.clientSecret
      ) {
        Modal.info({
          title: "Reset Client Secret",
          centered: true,
          width: 500,
          content: (
            <Typography.Paragraph>
              <span>
                <ExclamationCircleTwoTone twoToneColor="#f9b02e" />{" "}
              </span>
              Please copy an save the client secret now, you will not be able to
              access it later.
              <p />
              <Typography.Paragraph copyable>
                {data.resetOpenIdConnectApplicationClientSecret.clientSecret}
              </Typography.Paragraph>
            </Typography.Paragraph>
          ),
        });
      }
    } finally {
      setResetting(false);
    }
  };

  return (
    <Button danger type="default" onClick={reset} loading={resetting}>
      Reset Client Secret
    </Button>
  );
}
