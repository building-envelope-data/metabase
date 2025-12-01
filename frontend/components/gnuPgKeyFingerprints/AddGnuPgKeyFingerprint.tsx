import { useMutation } from "@apollo/client/react";
import { Alert, Form, Input, Button, Typography } from "antd";
import { AddGnuPgKeyFingerprintDocument } from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.generated";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  fingerprint: string;
};

export type AddGnuPgKeyFingerprintProps = {
  institutionId: Scalars["Uuid"]["input"];
};

export default function AddGnuPgKeyFingerprint({
  institutionId,
}: AddGnuPgKeyFingerprintProps) {
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
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const [creating, setCreating] = useState(false);

  const onFinish = ({ fingerprint }: FormValues) => {
    const add = async () => {
      try {
        setCreating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { error, data } = await addGnuPgKeyFingerprintMutation({
          variables: {
            input: {
              fingerprint: fingerprint,
              institutionId: institutionId,
            },
          },
        });
        handleFormErrors(
          error,
          data?.addGnuPgKeyFingerprint?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form,
        );
        if (
          !error &&
          !data?.addGnuPgKeyFingerprint?.errors &&
          data?.addGnuPgKeyFingerprint?.gnuPgKeyFingerprint
        ) {
          form.resetFields();
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setCreating(false);
      }
    };
    add();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      {globalErrorMessages.length > 0 ? (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      ) : (
        <></>
      )}
      <Typography.Paragraph>
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
          <Button type="primary" htmlType="submit" loading={creating}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
