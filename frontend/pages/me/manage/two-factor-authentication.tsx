import { messageApolloError } from "../../../lib/apollo";
import ManageLayout from "../../../components/me/ManageLayout";
import {
  useTwoFactorAuthenticationQuery,
  useGenerateUserTwoFactorRecoveryCodesMutation,
  useDisableUserTwoFactorAuthenticationMutation,
  useResetUserTwoFactorAuthenticatorMutation,
  useForgetUserTwoFactorAuthenticationClientMutation,
  TwoFactorAuthenticationDocument,
} from "../../../queries/currentUser.graphql";
import { Button, Alert, message, Skeleton, Typography } from "antd";
import Link from "next/link";
import { useEffect, useState } from "react";
import paths from "../../../paths";
import { recoveryCodesModal } from "../../../lib/recoveryCodesModal";

function Page() {
  const { error, data } = useTwoFactorAuthenticationQuery();
  const twoFactorAuthentication = data?.currentUser?.twoFactorAuthentication;

  const [forgetUserTwoFactorAuthenticationClientMutation] =
    useForgetUserTwoFactorAuthenticationClientMutation({
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    });
  const [
    forgettingUserTwoFactorAuthenticationClient,
    setForgettingUserTwoFactorAuthenticationClient,
  ] = useState(false);
  const forgetUserTwoFactorAuthenticationClient = async () => {
    try {
      setForgettingUserTwoFactorAuthenticationClient(true);
      const { errors, data } =
        await forgetUserTwoFactorAuthenticationClientMutation();
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.forgetUserTwoFactorAuthenticationClient?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.forgetUserTwoFactorAuthenticationClient?.errors
            .map((error) => error.message)
            .join(" ")
        );
      } else {
        message.success(
          "The current browser has been forgotten. When you login again from this browser you will be prompted for your two-factor authentication code."
        );
      }
    } finally {
      setForgettingUserTwoFactorAuthenticationClient(false);
    }
  };

  const [disableUserTwoFactorAuthenticationMutation] =
    useDisableUserTwoFactorAuthenticationMutation({
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    });
  const [
    disablingUserTwoFactorAuthentication,
    setDisablingUserTwoFactorAuthentication,
  ] = useState(false);
  const disableUserTwoFactorAuthentication = async () => {
    try {
      setDisablingUserTwoFactorAuthentication(true);
      const { errors, data } =
        await disableUserTwoFactorAuthenticationMutation();
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.disableUserTwoFactorAuthentication?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.disableUserTwoFactorAuthentication?.errors
            .map((error) => error.message)
            .join(" ")
        );
      } else {
        message.success(
          "Two-factor authentication has been disabled. You can reenable it when you setup an authenticator app."
        );
      }
    } finally {
      setDisablingUserTwoFactorAuthentication(false);
    }
  };

  const [resetUserTwoFactorAuthenticatorMutation] =
    useResetUserTwoFactorAuthenticatorMutation({
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    });
  const [
    resettingUserTwoFactorAuthenticator,
    setResettingUserTwoFactorAuthenticator,
  ] = useState(false);
  const resetUserTwoFactorAuthenticator = async () => {
    try {
      setResettingUserTwoFactorAuthenticator(true);
      const { errors, data } = await resetUserTwoFactorAuthenticatorMutation();
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.resetUserTwoFactorAuthenticator?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.resetUserTwoFactorAuthenticator?.errors
            .map((error) => error.message)
            .join(" ")
        );
      } else {
        message.success(
          "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key."
        );
      }
    } finally {
      setResettingUserTwoFactorAuthenticator(false);
    }
  };

  const [generateUserTwoFactorRecoveryCodesMutation] =
    useGenerateUserTwoFactorRecoveryCodesMutation({
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    });
  const [
    generatingUserTwoFactorRecoveryCodes,
    setGeneratingUserTwoFactorRecoveryCodes,
  ] = useState(false);
  const generateUserTwoFactorRecoveryCodes = async () => {
    try {
      setGeneratingUserTwoFactorRecoveryCodes(true);
      const { errors, data } =
        await generateUserTwoFactorRecoveryCodesMutation();
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.generateUserTwoFactorRecoveryCodes?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.generateUserTwoFactorRecoveryCodes?.errors
            .map((error) => error.message)
            .join(" ")
        );
      } else {
        recoveryCodesModal(
          data?.generateUserTwoFactorRecoveryCodes?.twoFactorRecoveryCodes || []
        );
      }
    } finally {
      setGeneratingUserTwoFactorRecoveryCodes(false);
    }
  };

  useEffect(() => {
    if (error) {
      messageApolloError(error);
    }
  }, [error]);

  if (!twoFactorAuthentication) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
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
                  <Button
                    onClick={generateUserTwoFactorRecoveryCodes}
                    loading={generatingUserTwoFactorRecoveryCodes}
                  >
                    generate a new set of recovery codes
                  </Button>{" "}
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
                  <Button
                    onClick={generateUserTwoFactorRecoveryCodes}
                    loading={generatingUserTwoFactorRecoveryCodes}
                  >
                    generate a new set of recovery codes
                  </Button>
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
                    <Button
                      onClick={generateUserTwoFactorRecoveryCodes}
                      loading={generatingUserTwoFactorRecoveryCodes}
                    >
                      generate a new set of recovery codes
                    </Button>
                    .
                  </>
                }
                type="warning"
              />
            )}
          {twoFactorAuthentication.isMachineRemembered && (
            <Button
              onClick={forgetUserTwoFactorAuthenticationClient}
              loading={forgettingUserTwoFactorAuthenticationClient}
            >
              Forget this browser
            </Button>
          )}
          <Button
            onClick={disableUserTwoFactorAuthentication}
            loading={disablingUserTwoFactorAuthentication}
          >
            Disable two-factor authentication
          </Button>
          <Alert
            message="Resetting recovery codes does not change the keys used in authenticator apps. If you wish to change the key
        used in an authenticator app you should reset your authenticator keys below."
            type="info"
          />
          <Button
            onClick={generateUserTwoFactorRecoveryCodes}
            loading={generatingUserTwoFactorRecoveryCodes}
          >
            Reset recovery codes
          </Button>
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
          <Button
            onClick={resetUserTwoFactorAuthenticator}
            loading={resettingUserTwoFactorAuthenticator}
          >
            Reset authenticator app
          </Button>
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
