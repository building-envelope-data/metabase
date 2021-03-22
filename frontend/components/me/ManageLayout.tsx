import { useEffect } from "react";
import { useRouter } from "next/router";
import Link from "next/link";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import { Layout as AntLayout, Menu } from "antd";
import Layout from "../Layout";
import paths from "../../paths";

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
    path: paths.me.manage.password,
    label: "Password",
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

const ManageLayout: React.FunctionComponent = ({ children }) => {
  const router = useRouter();

  const { loading, error, data } = useCurrentUserQuery();
  const currentUser = data?.currentUser;
  const shouldRedirect = !(loading || error || currentUser);

  useEffect(() => {
    if (shouldRedirect) {
      router.push({
        pathname: paths.userLogin,
        query: { returnTo: paths.userCurrent },
      });
    }
  }, [shouldRedirect]);

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
          </Menu>
        </AntLayout.Sider>
        <AntLayout.Content style={{ padding: "0 24px", minHeight: 280 }}>
          {children}
        </AntLayout.Content>
      </AntLayout>
    </Layout>
  );
};

export default ManageLayout;
