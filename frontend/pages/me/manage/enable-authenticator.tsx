import ManageLayout from "../../../components/me/ManageLayout";
import {
  Input,
  Button,
  Typography,
  List,
  Alert,
  message,
  Form,
  Skeleton,
} from "antd";
import {
  useGenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation,
  useEnableUserTwoFactorAuthenticatorMutation,
} from "../../../queries/currentUser.graphql";
import { useRouter } from "next/router";
import paths from "../../../paths";
import { handleFormErrors } from "../../../lib/form";
import { useEffect, useState } from "react";
import QRCode from "qrcode.react";
import { recoveryCodesModal } from "../../../lib/recoveryCodesModal";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const router = useRouter();
  const [generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation] =
    useGenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation();
  const [enableUserTwoFactorAuthenticatorMutation] =
    useEnableUserTwoFactorAuthenticatorMutation();
  const [sharedKey, setSharedKey] = useState<string | null | undefined>(
    undefined
  );
  const [authenticatorUri, setAuthenticatorUri] = useState<
    string | null | undefined
  >(undefined);

  const [messageApi, contextHolder] = message.useMessage();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [enabling, setEnabling] = useState(false);

  const onFinish = ({ verificationCode }: { verificationCode: string }) => {
    const enable = async () => {
      try {
        setEnabling(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await enableUserTwoFactorAuthenticatorMutation(
          {
            variables: {
              verificationCode: verificationCode,
            },
          }
        );
        handleFormErrors(
          errors,
          data?.enableUserTwoFactorAuthenticator?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (data?.enableUserTwoFactorAuthenticator?.sharedKey) {
          setSharedKey(data.enableUserTwoFactorAuthenticator.sharedKey);
        }
        if (data?.enableUserTwoFactorAuthenticator?.authenticatorUri) {
          setAuthenticatorUri(
            data.enableUserTwoFactorAuthenticator.authenticatorUri
          );
        }
        if (data?.enableUserTwoFactorAuthenticator?.twoFactorRecoveryCodes) {
          recoveryCodesModal(
            data.enableUserTwoFactorAuthenticator.twoFactorRecoveryCodes
          );
        }
        if (!errors && !data?.enableUserTwoFactorAuthenticator?.errors) {
          message.success("Your authenticator app has been verified.");
          await router.push(paths.me.manage.twoFactorAuthentication);
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setEnabling(false);
      }
    };
    enable();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  useEffect(() => {
    const generate = async () => {
      if (router.isReady) {
        const { errors, data } =
          await generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation();
        if (errors) {
          messageApi.error(errors.map(error => error.message));
        }
        if (data) {
          setSharedKey(
            data?.generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri
              ?.sharedKey
          );
          setAuthenticatorUri(
            data?.generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri
              ?.authenticatorUri
          );
        }
      }
    };
    generate();
  }, [router, generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation, messageApi]);

  if (!sharedKey || !authenticatorUri) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
    );
  }

  return (
    <>
    {contextHolder}
    <ManageLayout>
      <Typography.Title level={3}>Configure</Typography.Title>
      <Typography.Paragraph>
        To use an authenticator app go through the following steps:
      </Typography.Paragraph>
      <List>
        <List>
          <Typography.Paragraph>
            Download a two-factor authenticator app like{" "}
            <a href="https://freeotp.github.io/">FreeOTP</a> or{" "}
            <a href="https://www.microsoft.com/en-us/account/authenticator">
              Microsoft Authenticator
            </a>{" "}
            or{" "}
            <a href="https://support.google.com/accounts/answer/1066447">
              Google Authenticator
            </a>
            .
          </Typography.Paragraph>
        </List>
        <List>
          <Typography.Paragraph>
            Scan the QR Code or enter this key <kbd>{sharedKey}</kbd> into your
            two factor authenticator app. Spaces and casing do not matter.
          </Typography.Paragraph>
          <QRCode value={authenticatorUri} />
        </List>
        <List>
          <Typography.Paragraph>
            Once you have scanned the QR code or input the key above, your two
            factor authentication app will provide you with a unique code. Enter
            the code in the confirmation box below.
          </Typography.Paragraph>
          {globalErrorMessages.length > 0 ? (
            <Alert type="error" message={globalErrorMessages.join(" ")} />
          ) : (
            <></>
          )}
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
              <Button type="primary" htmlType="submit" loading={enabling}>
                Verify
              </Button>
            </Form.Item>
          </Form>
        </List>
      </List>
    </ManageLayout>
</>
  );
}

export default Page;
