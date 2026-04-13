import ManageLayout from "../../../components/me/ManageLayout";
import { Typography, List, QRCode } from "antd";
import { useState } from "react";
import { GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri } from "../../../components/me/GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri";
import { EnableUserTwoFactorAuthenticator } from "../../../components/me/EnableUserTwoFactorAuthenticator";

function Page() {
  const [sharedKey, setSharedKey] = useState<string | null | undefined>(
    undefined,
  );
  const [authenticatorUri, setAuthenticatorUri] = useState<
    string | null | undefined
  >(undefined);

  if (!sharedKey || !authenticatorUri) {
    return (
      <GenerateUserTwoFactorAuthenticatorSharedKeyAndQrCodeUri
        setSharedKey={setSharedKey}
        setAuthenticatorUri={setAuthenticatorUri}
      />
    );
  }

  return (
    <>
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
              Scan the QR Code or enter this key <kbd>{sharedKey}</kbd> into
              your two factor authenticator app. Spaces and casing do not
              matter.
            </Typography.Paragraph>
            <QRCode value={authenticatorUri} />
          </List>
          <List>
            <Typography.Paragraph>
              Once you have scanned the QR code or input the key above, your two
              factor authentication app will provide you with a unique code.
              Enter the code in the confirmation box below.
            </Typography.Paragraph>
            <EnableUserTwoFactorAuthenticator
              setSharedKey={setSharedKey}
              setAuthenticatorUri={setAuthenticatorUri}
            />
          </List>
        </List>
      </ManageLayout>
    </>
  );
}

export default Page;
