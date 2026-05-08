import { useMutation } from "@apollo/client/react";
import { Input, Button, Form, App } from "antd";
import { EnableUserTwoFactorAuthenticatorDocument } from "../../queries/currentUser.generated";
import { useRouter } from "next/router";
import paths from "../../paths";
import { Dispatch, SetStateAction, useState } from "react";
import { recoveryCodesModal } from "../../lib/recoveryCodesModal";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import { EnableUserTwoFactorAuthenticatorMutation } from "../../queries/currentUser.generated";
import ErrorAlert from "../ErrorAlert";

interface FormValues {
  verificationCode: string;
}

interface Props {
  setSharedKey: Dispatch<SetStateAction<string | null | undefined>>;
  setAuthenticatorUri: Dispatch<SetStateAction<string | null | undefined>>;
}

export default function EnableUserTwoFactorAuthenticator({
  setSharedKey,
  setAuthenticatorUri,
}: Props) {
  const router = useRouter();

  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const { message, modal } = App.useApp();

  const [enableUserTwoFactorAuthenticatorMutation] = useMutation(
    EnableUserTwoFactorAuthenticatorDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<EnableUserTwoFactorAuthenticatorMutation>({
      getErrors: (data) => data.enableUserTwoFactorAuthenticator.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        enableUserTwoFactorAuthenticatorMutation({
          variables: {
            input: {
              verificationCode: values.verificationCode,
            },
          },
        }),
      {
        onSuccess: (data) => {
          const payload = data?.enableUserTwoFactorAuthenticator;
          if (payload?.sharedKey) {
            setSharedKey(payload.sharedKey);
          }
          if (payload?.authenticatorUri) {
            setAuthenticatorUri(payload.authenticatorUri);
          }
          if (payload?.twoFactorRecoveryCodes) {
            recoveryCodesModal(modal, payload.twoFactorRecoveryCodes);
          }
          message.success("Your authenticator app has been verified.");
          return router.push(paths.me.manage.twoFactorAuthentication);
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
      <Form
        {...layout}
        form={form}
        name="basic"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Verification Code"
          name="verificationCode"
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
            Verify
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
