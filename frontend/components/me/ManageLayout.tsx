import { ReactNode, useMemo } from "react";
import { useRouter } from "next/router";
import Link from "next/link";
import { Skeleton, Layout as AntLayout, Menu } from "antd";
import Layout from "../Layout";
import paths from "../../paths";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";

const navItems = [
  {
    path: paths.me.manage.profile,
    label: "Profile",
  },
  {
    path: paths.me.manage.email,
    label: "Email",
  },
  {
    path: paths.me.manage.twoFactorAuthentication,
    label: "Two-factor Authentication",
  },
  {
    path: paths.me.manage.personalData,
    label: "Personal Data",
  },
];

const changePasswordItem = {
  path: paths.me.manage.changePassword,
  label: "Change Password",
};
const setPasswordItem = {
  path: paths.me.manage.setPassword,
  label: "Set Password",
};

type ManageLayoutProps = {
  children?: ReactNode;
};

export default function ManageLayout({ children }: ManageLayoutProps) {
  const router = useRouter();
  const { authenticated, currentUser } = useRequireAuth({
    returnTo: paths.me.manage.profile,
  });

  const items = useMemo(
    () =>
      [
        ...navItems,
        currentUser?.hasPassword ? changePasswordItem : setPasswordItem,
      ].map((item) => ({
        key: item.path,
        label: <Link href={item.path}>{item.label}</Link>,
      })),
    [navItems, currentUser?.hasPassword, changePasswordItem, setPasswordItem],
  );

  if (!authenticated) {
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  return (
    <Layout>
      <AntLayout>
        <AntLayout.Sider>
          <Menu
            mode="inline"
            selectedKeys={[router.pathname]}
            style={{ height: "100%", borderRight: 0 }}
            items={items}
          />
        </AntLayout.Sider>
        <AntLayout.Content style={{ padding: "0 24px", minHeight: 280 }}>
          {children}
        </AntLayout.Content>
      </AntLayout>
    </Layout>
  );
}
