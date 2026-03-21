import { Typography, Skeleton } from "antd";
import ManageLayout from "../../../components/me/ManageLayout";
import paths from "../../../paths";
import { DeletePersonalUserData } from "../../../components/me/DeletePersonalUserData";
import { useRequireAuth } from "../../../lib/hooks/useRequireAuth";

function Page() {
  const { currentUser } = useRequireAuth({
    returnTo: paths.me.manage.personalData,
  });

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
        Your account contains personal data that you have given us. This page
        allows you to download or delete that data in accordance with the{" "}
        <Typography.Link href="https://gdpr.eu">
          General Data Protection Regulation (GDPR)
        </Typography.Link>
      </Typography.Paragraph>
      <Typography.Paragraph strong>
        Deleting this data will permanently remove your account, and this cannot
        be recovered.
      </Typography.Paragraph>
      <Typography.Link href={paths.personalUserData}>
        Download Personal User Data
      </Typography.Link>
      <DeletePersonalUserData hasPassword={currentUser.hasPassword} />
    </ManageLayout>
  );
}

export default Page;
