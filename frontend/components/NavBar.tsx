import { useQuery } from "@apollo/client/react";
import Link from "next/link";
import { useRouter } from "next/router";
import { Menu, Button, Spin } from "antd";
import {
  CurrentUserDocument,
  CurrentUserPartialFragment,
} from "../queries/currentUser.generated";
import paths from "../paths";
import { extractAntiforgeryTokenFromCookie } from "../lib/apollo";
import { UserOutlined, LoadingOutlined } from "@ant-design/icons";
import type { Route } from "next";
import { isTruthy } from "../lib/array";
import { CSSProperties, useMemo } from "react";

const userLoadingItem = {
  key: paths.openIdConnect,
  style: { marginLeft: "auto" },
  label: (
    <Spin indicator={<LoadingOutlined style={{ color: "white" }} spin />} />
  ),
};

const loginOrRegisterItems = [
  {
    key: paths.openIdConnectClientLogin,
    style: { marginLeft: "auto" },
    label: <Link href={paths.openIdConnectClientLogin}>Login</Link>,
  },
  {
    key: paths.userRegister,
    label: <Link href={paths.userRegister}>Register</Link>,
  },
];

const userItems = (currentUser: CurrentUserPartialFragment) =>
  [
    currentUser?.isAuthorizedToManageOpenIdConnect && {
      key: paths.openIdConnect,
      label: <Link href={paths.openIdConnect}>OpenID Connect</Link>,
    },
    {
      key: paths.me.manage.home,
      label: currentUser.name,
      icon: <UserOutlined />,
      style: { marginLeft: "auto" },
      children: [
        {
          key: paths.user(currentUser.uuid),
          label: (
            <Link href={paths.user(currentUser.uuid)}>
              Profile &amp; Dashboard
            </Link>
          ),
        },
        {
          key: paths.me.manage.profile,
          label: <Link href={paths.me.manage.profile}>Account</Link>,
        },
        {
          key: paths.openIdConnectClientLogout,
          label: (
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
          ),
        },
      ],
    },
  ].filter(isTruthy);

type NavItemProps =
  | {
      path: Route;
      label: string;
      subitems: null;
    }
  | { label: string; subitems: { path: Route; label: string }[] };

interface NavBarProps {
  items: NavItemProps[];
  style?: CSSProperties;
}

export default function NavBar({ items, style }: NavBarProps) {
  const router = useRouter();
  const { loading, data } = useQuery(CurrentUserDocument);
  const currentUser = data?.currentUser;

  const mainItems = useMemo(
    () =>
      items.map((item) =>
        item.subitems === null
          ? {
              key: item.path,
              label: <Link href={item.path}>{item.label}</Link>,
            }
          : {
              key: item.label,
              label: item.label,
              children: item.subitems.map((subitem) => ({
                key: subitem.path,
                label: <Link href={subitem.path}>{subitem.label}</Link>,
              })),
            },
      ),
    [items],
  );

  const userOrLoginItems = useMemo(
    () =>
      loading
        ? [userLoadingItem]
        : currentUser
          ? userItems(currentUser)
          : loginOrRegisterItems,
    [loading, userLoadingItem, currentUser, loginOrRegisterItems],
  );

  return (
    <Menu
      mode="horizontal"
      theme="dark"
      selectedKeys={[router.pathname]}
      style={style}
      items={[...mainItems, ...userOrLoginItems]}
    />
  );
}
