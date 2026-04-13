import { ReactNode } from "react";
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

type ManageLayoutProps = {
  children?: ReactNode;
};

export default function ManageLayout({ children }: ManageLayoutProps) {
  const router = useRouter();
  const { authenticated, currentUser } = useRequireAuth({
    returnTo: paths.me.manage.profile,
  });

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
          >
            {navItems.map(({ path, label }) => (
              <Menu.Item key={path}>
                <Link href={path}>{label}</Link>
              </Menu.Item>
            ))}
            {currentUser.hasPassword ? (
              <Menu.Item key={paths.me.manage.changePassword}>
                <Link href={paths.me.manage.changePassword}>
                  Change Password
                </Link>
              </Menu.Item>
            ) : (
              <Menu.Item key={paths.me.manage.setPassword}>
                <Link href={paths.me.manage.setPassword}>Set Password</Link>
              </Menu.Item>
            )}
          </Menu>
        </AntLayout.Sider>
        <AntLayout.Content style={{ padding: "0 24px", minHeight: 280 }}>
          {children}
        </AntLayout.Content>
      </AntLayout>
    </Layout>
  );
}
