import { useQuery } from "@apollo/client/react";
import ManageLayout from "../../../components/me/ManageLayout";
import { TwoFactorAuthenticationDocument } from "../../../queries/currentUser.generated";
import { Alert, Result, Skeleton, Typography } from "antd";
import Link from "next/link";
import paths from "../../../paths";
import GenerateUserTwoFactorRecoveryCodes from "../../../components/me/GenerateUserTwoFactorRecoveryCodes";
import DisableUserTwoFactorAuthentication from "../../../components/me/DisableUserTwoFactorAuthentication";
import ResetUserTwoFactorAuthenticator from "../../../components/me/ResetUserTwoFactorAuthenticator";
import ForgetUserTwoFactorAuthenticationClient from "../../../components/me/ForgetUserTwoFactorAuthenticationClient";
import { useQueryHandler } from "../../../lib/hooks/useQueryHandler";

function Page() {
  const { loading, error, data } = useQuery(TwoFactorAuthenticationDocument);
  useQueryHandler({ error });
  const twoFactorAuthentication = data?.currentUser?.twoFactorAuthentication;

  if (loading) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
    );
  }

  if (!twoFactorAuthentication) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
      />
    );
  }

  return (
    <ManageLayout>
      <Typography.Title level={1}>Two-factor authentication</Typography.Title>
      {twoFactorAuthentication.isEnabled ? (
        <>
          {twoFactorAuthentication.recoveryCodesLeftCount == 0 && (
            <Alert
              message="You have no recovery codes left."
              description={
                <>
                  You must{" "}
                  <GenerateUserTwoFactorRecoveryCodes>
                    generate a new set of recovery codes
                  </GenerateUserTwoFactorRecoveryCodes>{" "}
                  before you can log in with a recovery code.
                </>
              }
              type="warning"
            />
          )}
          {twoFactorAuthentication.recoveryCodesLeftCount == 1 && (
            <Alert
              message="You have 1 recovery code left."
              description={
                <>
                  You should{" "}
                  <GenerateUserTwoFactorRecoveryCodes>
                    generate a new set of recovery codes
                  </GenerateUserTwoFactorRecoveryCodes>
                  .
                </>
              }
              type="warning"
            />
          )}
          {twoFactorAuthentication.recoveryCodesLeftCount >= 2 &&
            twoFactorAuthentication.recoveryCodesLeftCount <= 3 && (
              <Alert
                message={`You have ${twoFactorAuthentication.recoveryCodesLeftCount} recovery codes left.`}
                description={
                  <>
                    You should{" "}
                    <GenerateUserTwoFactorRecoveryCodes>
                      generate a new set of recovery codes
                    </GenerateUserTwoFactorRecoveryCodes>
                    .
                  </>
                }
                type="warning"
              />
            )}
          {twoFactorAuthentication.isMachineRemembered && (
            <ForgetUserTwoFactorAuthenticationClient />
          )}
          <DisableUserTwoFactorAuthentication />
          <Alert
            message="Resetting recovery codes does not change the keys used in authenticator apps. If you wish to change the key
        used in an authenticator app you should reset your authenticator keys below."
            type="info"
          />
          <GenerateUserTwoFactorRecoveryCodes>
            Reset recovery codes
          </GenerateUserTwoFactorRecoveryCodes>
        </>
      ) : (
        <Typography.Paragraph>
          Two-factor authentication is disabled. You can enable it when you
          setup an authenticator app.
        </Typography.Paragraph>
      )}
      <Typography.Title level={2}>Authenticator app</Typography.Title>
      {twoFactorAuthentication.hasAuthenticator ? (
        <>
          <Link href={paths.me.manage.enableAuthenticator}>
            Setup authenticator app
          </Link>

          <Alert
            message="If you reset your authenticator key your authenticator app will not work until you reconfigure it."
            description="This process disables two-factor authentication until you verify your authenticator app. If you do not complete your authenticator app configuration you may lose access to your account."
          />
          <ResetUserTwoFactorAuthenticator />
        </>
      ) : (
        <Link href={paths.me.manage.enableAuthenticator}>
          Add authenticator app
        </Link>
      )}
    </ManageLayout>
  );
}

export default Page;
