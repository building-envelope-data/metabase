import { Typography, Skeleton } from "antd";
import ManageLayout from "../../../components/me/ManageLayout";
import { SetUserPhoneNumber } from "../../../components/me/SetUserPhoneNumber";
import { useRequireAuth } from "../../../lib/hooks/useRequireAuth";
import paths from "../../../paths";

function Page() {
  const { currentUser } = useRequireAuth({ returnTo: paths.me.manage.profile });

  if (!currentUser) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
    );
  }

  return (
    <ManageLayout>
      <Typography.Paragraph style={{ maxWidth: "75ch" }}>Hello {currentUser.name}!</Typography.Paragraph>
      {/* TODO Change name, postal address, and website locator */}
      <SetUserPhoneNumber phoneNumber={currentUser.contact.phoneNumber} />
    </ManageLayout>
  );
}

export default Page;
