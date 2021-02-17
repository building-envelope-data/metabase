import * as React from 'react'
import Link from "next/link";
import { useRouter } from "next/router";
import { Menu } from "antd"

type NavItemProps = {
    path: string,
    label: string,
}

export type NavBarProps = {
    items: NavItemProps[]
}

export const NavBar: React.FunctionComponent<NavBarProps> = ({ items }) => {
    const router = useRouter()
    return (
        <Menu
            mode="horizontal"
            selectedKeys={[router.pathname]}
            theme="dark"
        >
            {items.map(({ path, label }) => (
                <Menu.Item key={path}>
                    <Link href={path}>
                        {label}
                    </Link>
                </Menu.Item>
            ))}
            <Menu.Item key="/user/login">
                <Link href="/user/login">
                    Login
                </Link>
            </Menu.Item>
            <Menu.Item key="/user/register">
                <Link href="/user/register">
                    Register
                </Link>
            </Menu.Item>
        </Menu>
    )
};

export default NavBar;