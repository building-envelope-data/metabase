import Head from "next/head"
import * as React from "react"
import Footer from "./Footer"
import NavBar from "./NavBar"
import { Layout as AntLayout } from "antd"

const navItems = [
    {
        path: `/`,
        label: `Home`,
    },
    {
        path: `/about`,
        label: `About`,
    },
    {
        path: `/user/change-email`,
        label: `Change Email`
    }
]

const Layout: React.FunctionComponent = ({ children }) => {
    const appTitle = `Building Envelope Data`;

    return (
        <AntLayout>
            <Head>
                <title>{appTitle}</title>
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <meta charSet="utf-8" />
            </Head>
            <AntLayout.Header>
                <NavBar items={navItems} />
            </AntLayout.Header>
            <AntLayout.Content>
                {children}
            </AntLayout.Content>
            <AntLayout.Footer>
                <Footer />
            </AntLayout.Footer>
        </AntLayout>
    );
};

export default Layout;