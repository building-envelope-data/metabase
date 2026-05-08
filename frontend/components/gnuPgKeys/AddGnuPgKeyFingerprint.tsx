import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, Typography, App, Modal } from "antd";
import {
  AddGnuPgKeyFingerprintDocument,
  AddGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import GnuPgKeySummary from "./GnuPgKeySummary";
import NewButton from "../NewButton";
import CodeView from "../CodeView";

type FormValues = {
  fingerprint: string;
};

interface AddGnuPgKeyFingerprintProps {
  institutionId: Scalars["Uuid"]["input"];
}

export default function AddGnuPgKeyFingerprint({
  institutionId,
}: AddGnuPgKeyFingerprintProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const { notification } = App.useApp();

  const [addGnuPgKeyFingerprintMutation] = useMutation(
    AddGnuPgKeyFingerprintDocument,
  );

  const {
    mutating,
    withMutationHandler,
    augmentFormWithErrors,
    messageMissingModel,
  } = useMutationHandler<AddGnuPgKeyFingerprintMutation>({
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
        onSuccess: (data) => {
          const model = data?.addGnuPgKeyFingerprint?.gnuPgKeyFingerprint;
          if (!model) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Added GnuPG Key Fingerprint",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              style: {
                width: "max-content",
                minWidth: "384px",
              },
              description: <GnuPgKeySummary hideExtra entity={model} />,
            });
          }
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
      <NewButton onClick={() => setOpen(true)}>GnuPG Key Fingerprint</NewButton>
      <Modal
        open={open}
        title="Add GnuPG Key Fingerprint"
        // onOk={handleOk}
        onCancel={() => {
          setGlobalErrorMessages([]);
          form.resetFields();
          setOpen(false);
        }}
        footer={false}
      >
        <ErrorAlert messages={globalErrorMessages} />
        <Typography.Paragraph style={{ maxWidth: "75ch" }}>
          Before adding the GnuPG fingerprint of your GnuPG key here, you need
          to upload the GnuPG public key to the{" "}
          <Typography.Link href="https://keys.openpgp.org/">
            OpenPGP Keyserver
          </Typography.Link>{" "}
          and then verify the user ID of the key, which should be your email
          address. You can add your key either by running
          <CodeView
            code={`gpg \\
  --keyserver "hkps://keys.openpgp.org" \\
  --send-keys "\${SIGNING_KEY_FINGERPRINT}"`}
          />
          or by exporting it with
          <CodeView
            code={`gpg \\
  --export --armor \\
  --output ./my.pub.asc \\
  \${SIGNING_KEY_FINGERPRINT}`}
          />
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
      </Modal>
    </>
  );
}
