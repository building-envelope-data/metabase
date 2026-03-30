import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, Typography } from "antd";
import {
  AddGnuPgKeyFingerprintDocument,
  AddGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = {
  fingerprint: string;
};

interface AddGnuPgKeyFingerprintProps {
  institutionId: Scalars["Uuid"]["input"];
}

export default function AddGnuPgKeyFingerprint({
  institutionId,
}: AddGnuPgKeyFingerprintProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addGnuPgKeyFingerprintMutation] = useMutation(
    AddGnuPgKeyFingerprintDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddGnuPgKeyFingerprintMutation>({
      getErrors: (data) => data.addGnuPgKeyFingerprint.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        addGnuPgKeyFingerprintMutation({
          variables: {
            input: {
              fingerprint: values.fingerprint,
              institutionId: institutionId,
            },
          },
        }),
      {
        onSuccess: () => {
          setGlobalErrorMessages([]);
          form.resetFields();
        },
        onError: (graphQlErrors, userErrors) =>
          setGlobalErrorMessages(
            augmentFormWithErrors(graphQlErrors, userErrors, form),
          ),
      },
    );
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      <ErrorAlert messages={globalErrorMessages} />
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>
        Before adding the GnuPG fingerprint of your GnuPG key here, you need to
        upload the GnuPG public key to the{" "}
        <Typography.Link href="https://keys.openpgp.org/">
          OpenPGP Keyserver
        </Typography.Link>{" "}
        and then verify the user ID of the key, which should be your email
        address. You can add your key either by running{" "}
        <Typography.Text code>
          gpg --keyserver hkps://keys.openpgp.org --send-keys
          $SIGNING_KEY_FINGERPRINT
        </Typography.Text>{" "}
        or by exporting it with{" "}
        <Typography.Text code>
          gpg --export --armor --output ./my.pub.asc $SIGNING_KEY_FINGERPRINT
        </Typography.Text>{" "}
        and then uploading it via the form on{" "}
        <Typography.Link href="https://keys.openpgp.org/upload">
          OpenPGP Key Upload
        </Typography.Link>{" "}
        You can verify the user ID by following the link in the email sent to
        you by the OpenPGP Keyserver.
      </Typography.Paragraph>
      <Form
        {...layout}
        form={form}
        name="addGnuPgKeyFingerprint"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Fingerprint"
          name="fingerprint"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
