import * as React from "react";
import Link from "next/link";
import { useRouter } from "next/router";
import { Menu, Button, message } from "antd";
import {
  useCurrentUserQuery,
  useLogoutUserMutation,
} from "../queries/currentUser.graphql";
import paths from "../paths";
import { initializeApollo } from "../lib/apollo";
import { useState } from "react";
import { UserRole } from "../__generated__/__types__";
import { UserOutlined } from "@ant-design/icons";

type NavItemProps = {
  path: string;
  label: string;
};

export type NavBarProps = {
  items: NavItemProps[];
};

export default function NavBar({ items }: NavBarProps) {
  const router = useRouter();
  const currentUser = useCurrentUserQuery()?.data?.currentUser;
  const apolloClient = initializeApollo();
  const [logoutUserMutation] = useLogoutUserMutation();
  const [loggingOut, setLoggingOut] = useState(false);

  const logout = async () => {
    try {
      setLoggingOut(true);
      const { errors, data } = await logoutUserMutation();
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.logoutUser?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.logoutUser?.errors.map((error) => error.message).join(" ")
        );
      } else {
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        await apolloClient.resetStore();
        await router.push(paths.userLogin);
        // signOut();
      }
    } finally {
      setLoggingOut(false);
    }
  };

  return (
    <Menu mode="horizontal" selectedKeys={[router.pathname]} theme="dark">
      {items.map(({ path, label }) => (
        <Menu.Item key={path}>
          <Link href={path}>{label}</Link>
        </Menu.Item>
      ))}
      {/* I would like the following to be on the right but that is not possible at the moment, see issue https://github.com/ant-design/ant-design/issues/10749 */}
      {currentUser ? (
        <>
          {/* TODO Put information whether person is allowed to access OpenIdConnect information in query result of current user (using OpenIdConnectAuthorization) */}
          {currentUser?.roles?.includes(UserRole.Administrator) && (
            <Menu.Item key={paths.openIdConnect}>
              <Link href={paths.openIdConnect}>OpenId Connect</Link>
            </Menu.Item>
          )}
          <Menu.SubMenu
            title={currentUser.name}
            key={paths.me.manage.home}
            icon={<UserOutlined />}
          >
            <Menu.Item key={paths.user(currentUser.uuid)}>
              <Link href={paths.user(currentUser.uuid)}>Profile</Link>
            </Menu.Item>
            <Menu.Item key={paths.me.manage.profile}>
              <Link href={paths.me.manage.profile}>Account</Link>
            </Menu.Item>
            <Menu.Item key="logout">
              <Button type="primary" onClick={logout} loading={loggingOut}>
                Logout
              </Button>
            </Menu.Item>
          </Menu.SubMenu>
        </>
      ) : (
        <>
          <Menu.Item key={paths.userLogin}>
            <Link href={paths.userLogin}>Login</Link>
          </Menu.Item>
          <Menu.Item key={paths.userRegister}>
            <Link href={paths.userRegister}>Register</Link>
          </Menu.Item>
        </>
      )}
    </Menu>
  );
}
