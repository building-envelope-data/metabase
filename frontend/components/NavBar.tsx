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
import { MenuItemType } from "antd/lib/menu/interface";

const firstUserOrLoginItemStyle = { marginLeft: "auto" };

const userLoadingItem = {
  key: "userLoading",
  style: firstUserOrLoginItemStyle,
  label: (
    <Spin indicator={<LoadingOutlined style={{ color: "white" }} spin />} />
  ),
};

const loginOrRegisterItems = (returnTo: string | string[] | undefined) => [
  {
    key: paths.openIdConnectClientLogin,
    style: firstUserOrLoginItemStyle,
    label: (
      <Link
        href={{
          pathname: paths.openIdConnectClientLogin,
          query: returnTo
            ? { returnTo: returnTo }
            : window.location.pathname != paths.openIdConnectClientLogin &&
                window.location.pathname != paths.userLogin
              ? { returnTo: window.location.pathname }
              : null,
        }}
      >
        Login
      </Link>
    ),
  },
  {
    key: paths.userRegister,
    label: (
      <Link
        href={{
          pathname: paths.userRegister,
          query: returnTo
            ? { returnTo: returnTo }
            : window.location.pathname != paths.userRegister
              ? { returnTo: window.location.pathname }
              : null,
        }}
      >
        Register
      </Link>
    ),
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
      style: firstUserOrLoginItemStyle,
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
              <input
                name="returnTo"
                type="hidden"
                value={
                  typeof window !== "undefined" ? window.location.pathname : ""
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

export type NavItemProps =
  | {
      path: Route;
      label: string;
      subitems: null;
    }
  | { label: string; subitems: { path: Route; label: string }[] }
  | MenuItemType;

interface NavBarProps {
  items: NavItemProps[];
  onlyUserOrLoginItems?: boolean;
  style?: CSSProperties;
}

export default function NavBar({ items, style }: NavBarProps) {
  const router = useRouter();
  const { returnTo } = router.query;
  const { loading, data } = useQuery(CurrentUserDocument);
  const currentUser = data?.currentUser;

  const mainItems = useMemo(
    () =>
      items.map((item) =>
        "subitems" in item
          ? item.subitems === null
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
              }
          : item,
      ),
    [items],
  );

  const userOrLoginItems = useMemo(
    () =>
      loading
        ? [userLoadingItem]
        : currentUser
          ? userItems(currentUser)
          : loginOrRegisterItems(returnTo),
    [loading, currentUser],
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
