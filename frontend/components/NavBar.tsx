import { useQuery } from "@apollo/client/react";
import Link from "next/link";
import { useRouter } from "next/router";
import { Menu, Button, Spin } from "antd";
import { CurrentUserDocument } from "../queries/currentUser.generated";
import paths from "../paths";
import { extractAntiforgeryTokenFromCookie } from "../lib/apollo";
import { UserOutlined, LoadingOutlined } from "@ant-design/icons";
import type { Route } from "next";

type NavItemProps =
  | {
      path: Route;
      label: string;
      subitems: null;
    }
  | { label: string; subitems: { path: Route; label: string }[] };

interface NavBarProps {
  items: NavItemProps[];
}

export default function NavBar({ items }: NavBarProps) {
  const router = useRouter();
  const { loading, data } = useQuery(CurrentUserDocument);
  const currentUser = data?.currentUser;

  return (
    <>
      <Menu mode="horizontal" selectedKeys={[router.pathname]} theme="dark">
        {items.map((item) =>
          item.subitems === null ? (
            <Menu.Item key={item.path}>
              <Link href={item.path}>{item.label}</Link>
            </Menu.Item>
          ) : (
            // TODO find a better key
            <Menu.SubMenu title={item.label} key={item.label}>
              {item.subitems.map((subitem) => (
                <Menu.Item key={subitem.path}>
                  <Link href={subitem.path}>{subitem.label}</Link>
                </Menu.Item>
              ))}
            </Menu.SubMenu>
          ),
        )}
        {loading ? (
          <Menu.Item style={{ marginLeft: "auto" }}>
            <Spin
              indicator={<LoadingOutlined style={{ color: "white" }} spin />}
            />
          </Menu.Item>
        ) : currentUser ? (
          <>
            {currentUser?.isAuthorizedToManageOpenIdConnect && (
              <Menu.Item key={paths.openIdConnect}>
                <Link href={paths.openIdConnect}>OpenId Connect</Link>
              </Menu.Item>
            )}
            <Menu.SubMenu
              title={currentUser.name}
              key={paths.me.manage.home}
              icon={<UserOutlined />}
              style={{ marginLeft: "auto" }}
            >
              <Menu.Item key={paths.user(currentUser.uuid)}>
                <Link href={paths.user(currentUser.uuid)}>Profile</Link>
              </Menu.Item>
              <Menu.Item key={paths.me.manage.profile}>
                <Link href={paths.me.manage.profile}>Account</Link>
              </Menu.Item>
              <Menu.Item key={paths.openIdConnectClientLogout}>
                <form action={paths.openIdConnectClientLogout} method="post">
                  <input
                    name="__RequestVerificationToken"
                    type="hidden"
                    value={
                      typeof window !== "undefined"
                        ? (extractAntiforgeryTokenFromCookie() ?? "")
                        : ""
                    }
                  />
                  <Button type="primary" htmlType="submit">
                    Logout
                  </Button>
                </form>
              </Menu.Item>
            </Menu.SubMenu>
          </>
        ) : (
          <>
            <Menu.Item
              key={paths.openIdConnectClientLogin}
              style={{ marginLeft: "auto" }}
            >
              <Link href={paths.openIdConnectClientLogin}>Login</Link>
            </Menu.Item>
            <Menu.Item key={paths.userRegister}>
              <Link href={paths.userRegister}>Register</Link>
            </Menu.Item>
          </>
        )}
      </Menu>
    </>
  );
}
