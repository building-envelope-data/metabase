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
import recoveryCodesModal from "../../../lib/recoveryCodesModal";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const router = useRouter();
  const [
    generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation,
  ] = useGenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation();
  const [
    enableUserTwoFactorAuthenticatorMutation,
  ] = useEnableUserTwoFactorAuthenticatorMutation();
  const [sharedKey, setSharedKey] = useState<string | null | undefined>(
    undefined
  );
  const [authenticatorUri, setAuthenticatorUri] = useState<
    string | null | undefined
  >(undefined);

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
          router.push(paths.me.manage.twoFactorAuthentication);
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
        const {
          errors,
          data,
        } = await generateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUriMutation();
        if (errors) {
          message.error(errors);
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
  }, []);

  if (!sharedKey || !authenticatorUri) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
    );
  }

  return (
    <ManageLayout>
      <Typography.Title level={3}>Configure</Typography.Title>
      <Typography.Paragraph>
        To use an authenticator app go through the following steps:
      </Typography.Paragraph>
      <List>
        <List>
          <Typography.Paragraph>
            Download a two-factor authenticator app like Microsoft Authenticator
            for{" "}
            <a href="https://go.microsoft.com/fwlink/?Linkid=825072">Android</a>{" "}
            and <a href="https://go.microsoft.com/fwlink/?Linkid=825073">iOS</a>{" "}
            or Google Authenticator for{" "}
            <a href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en">
              Android
            </a>{" "}
            and{" "}
            <a href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8">
              iOS
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
  );
}

export default Page;
