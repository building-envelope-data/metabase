import { Typography, Skeleton } from "antd";
import ManageLayout from "../../../components/me/ManageLayout";
import { ChangeUserEmail } from "../../../components/me/ChangeUserEmail";
import { ResendUserEmailVerification } from "../../../components/me/ResendUserEmailVerification";
import { useRequireAuth } from "../../../lib/hooks/useRequireAuth";
import paths from "../../../paths";

function Page() {
  const { currentUser } = useRequireAuth({ returnTo: paths.me.manage.email });

  if (!currentUser) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
    );
  }

  return (
    <ManageLayout>
      <Typography.Paragraph>
        Your current email address is {currentUser.email}.
        {!currentUser.isEmailConfirmed && (
          <>
            Please verify it by following the verification link in the
            verification email you received. If you didn&apos;t receive a
            verification email, click the following button to resend it:{" "}
            <ResendUserEmailVerification />
          </>
        )}
      </Typography.Paragraph>
      <ChangeUserEmail />
    </ManageLayout>
  );
}

export default Page;
