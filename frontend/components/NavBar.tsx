import * as React from "react";
import Link from "next/link";
import { useRouter } from "next/router";
import { Menu, Button, message } from "antd";
import {
  useCurrentUserQuery,
  useLogoutUserMutation,
} from "../queries/currentUser.graphql";
import paths from "../paths";
// import { initializeApollo } from "../lib/apollo";
import { useState } from "react";
import { signIn, signOut } from "next-auth/client";

type NavItemProps = {
  path: string;
  label: string;
};

export type NavBarProps = {
  items: NavItemProps[];
};

export const NavBar: React.FunctionComponent<NavBarProps> = ({ items }) => {
  const router = useRouter();
  const currentUser = useCurrentUserQuery()?.data?.currentUser;
  // const apolloClient = initializeApollo();
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
        // await apolloClient.resetStore();
        signOut();
        // await router.push(paths.userLogin);
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
        <Menu.Item>
          <Button type="link" onClick={logout} loading={loggingOut}>
            Logout
          </Button>
        </Menu.Item>
      ) : (
        <>
          <Menu.Item>
            {/* TODO Instead of the provider id `metabase` use a global constant. It must match the one set in `[...nextauth].ts` */}
            <Button type="link" onClick={() => signIn("metabase")}>
              Login
            </Button>
          </Menu.Item>
          <Menu.Item key={paths.userRegister}>
            <Link href={paths.userRegister}>Register</Link>
          </Menu.Item>
        </>
      )}
    </Menu>
  );
};

export default NavBar;
