import Head from "next/head"
import * as React from "react"
import Header from "./Header"
import Footer from "./Footer"
import NavBar from "./NavBar"
import * as Ant from "antd"

import 'antd/dist/antd.css';

const navItems = [
    {
        path: `/`,
        label: `Home`,
    },
    {
        path: `/about`,
        label: `About`,
    }
]

const Layout: React.FunctionComponent = ({ children }) => {
    const appTitle = `Building Envelope Data`;

    return (
        <Ant.Layout>
            <Head>
                <title>{appTitle}</title>
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <meta charSet="utf-8" />
            </Head>
            <Ant.Layout.Header>
                <Header appTitle={appTitle} />
                <NavBar items={navItems} />
            </Ant.Layout.Header>
            <Ant.Layout.Content>
                {children}
            </Ant.Layout.Content>
            <Ant.Layout.Footer>
                <Footer />
            </Ant.Layout.Footer>
        </Ant.Layout>
    );
};

export default Layout;