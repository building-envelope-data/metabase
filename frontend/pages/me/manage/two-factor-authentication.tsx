import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../../lib/apollo";
import ManageLayout from "../../../components/me/ManageLayout";
import {
  GenerateUserTwoFactorRecoveryCodesDocument,
  DisableUserTwoFactorAuthenticationDocument,
  ResetUserTwoFactorAuthenticatorDocument,
  ForgetUserTwoFactorAuthenticationClientDocument,
  TwoFactorAuthenticationDocument,
} from "../../../queries/currentUser.generated";
import { Button, Alert, Skeleton, Typography, message } from "antd";
import Link from "next/link";
import { useEffect, useState } from "react";
import paths from "../../../paths";
import { recoveryCodesModal } from "../../../lib/recoveryCodesModal";

function Page() {
  const { error, data } = useQuery(TwoFactorAuthenticationDocument);
  const twoFactorAuthentication = data?.currentUser?.twoFactorAuthentication;

  const [messageApi, contextHolder] = message.useMessage();

  const [forgetUserTwoFactorAuthenticationClientMutation] = useMutation(
    ForgetUserTwoFactorAuthenticationClientDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    },
  );
  const [
    forgettingUserTwoFactorAuthenticationClient,
    setForgettingUserTwoFactorAuthenticationClient,
  ] = useState(false);
  const forgetUserTwoFactorAuthenticationClient = async () => {
    try {
      setForgettingUserTwoFactorAuthenticationClient(true);
      const { error, data } =
        await forgetUserTwoFactorAuthenticationClientMutation();
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.forgetUserTwoFactorAuthenticationClient?.errors) {
        // TODO Is this how we want to display errors?
        messageApi.error(
          data?.forgetUserTwoFactorAuthenticationClient?.errors
            .map((error) => error.message)
            .join(" "),
        );
      } else {
        messageApi.success(
          "The current browser has been forgotten. When you login again from this browser you will be prompted for your two-factor authentication code.",
        );
      }
    } finally {
      setForgettingUserTwoFactorAuthenticationClient(false);
    }
  };

  const [disableUserTwoFactorAuthenticationMutation] = useMutation(
    DisableUserTwoFactorAuthenticationDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    },
  );
  const [
    disablingUserTwoFactorAuthentication,
    setDisablingUserTwoFactorAuthentication,
  ] = useState(false);
  const disableUserTwoFactorAuthentication = async () => {
    try {
      setDisablingUserTwoFactorAuthentication(true);
      const { error, data } =
        await disableUserTwoFactorAuthenticationMutation();
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.disableUserTwoFactorAuthentication?.errors) {
        // TODO Is this how we want to display errors?
        messageApi.error(
          data?.disableUserTwoFactorAuthentication?.errors
            .map((error) => error.message)
            .join(" "),
        );
      } else {
        messageApi.success(
          "Two-factor authentication has been disabled. You can reenable it when you setup an authenticator app.",
        );
      }
    } finally {
      setDisablingUserTwoFactorAuthentication(false);
    }
  };

  const [resetUserTwoFactorAuthenticatorMutation] = useMutation(
    ResetUserTwoFactorAuthenticatorDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    },
  );
  const [
    resettingUserTwoFactorAuthenticator,
    setResettingUserTwoFactorAuthenticator,
  ] = useState(false);
  const resetUserTwoFactorAuthenticator = async () => {
    try {
      setResettingUserTwoFactorAuthenticator(true);
      const { error, data } = await resetUserTwoFactorAuthenticatorMutation();
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.resetUserTwoFactorAuthenticator?.errors) {
        // TODO Is this how we want to display errors?
        messageApi.error(
          data?.resetUserTwoFactorAuthenticator?.errors
            .map((error) => error.message)
            .join(" "),
        );
      } else {
        messageApi.success(
          "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.",
        );
      }
    } finally {
      setResettingUserTwoFactorAuthenticator(false);
    }
  };

  const [generateUserTwoFactorRecoveryCodesMutation] = useMutation(
    GenerateUserTwoFactorRecoveryCodesDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: TwoFactorAuthenticationDocument,
        },
      ],
    },
  );
  const [
    generatingUserTwoFactorRecoveryCodes,
    setGeneratingUserTwoFactorRecoveryCodes,
  ] = useState(false);
  const generateUserTwoFactorRecoveryCodes = async () => {
    try {
      setGeneratingUserTwoFactorRecoveryCodes(true);
      const { error, data } =
        await generateUserTwoFactorRecoveryCodesMutation();
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.generateUserTwoFactorRecoveryCodes?.errors) {
        // TODO Is this how we want to display errors?
        messageApi.error(
          data?.generateUserTwoFactorRecoveryCodes?.errors
            .map((error) => error.message)
            .join(" "),
        );
      } else {
        recoveryCodesModal(
          data?.generateUserTwoFactorRecoveryCodes?.twoFactorRecoveryCodes ||
            [],
        );
      }
    } finally {
      setGeneratingUserTwoFactorRecoveryCodes(false);
    }
  };

  useEffect(() => {
    if (error) {
      messageApi.error(stringifyApolloError(error));
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
      {contextHolder}
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
