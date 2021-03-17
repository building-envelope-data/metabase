import Head from "next/head";
import * as React from "react";
import Footer from "./Footer";
import NavBar from "./NavBar";
import { Layout as AntLayout } from "antd";
import paths from "../paths";
import { signIn, useSession } from "next-auth/client";
import { useEffect } from "react";

const navItems = [
  {
    path: paths.home,
    label: `Home`,
  },
  {
    path: paths.institutions,
    label: `Institutions`,
  },
  {
    path: paths.dataFormats,
    label: `Data Formats`,
  },
  {
    path: paths.methods,
    label: `Methods`,
  },
  {
    path: paths.components,
    label: `Components`,
  },
  {
    path: paths.users,
    label: `Users`,
  },
  {
    path: paths.userChangeEmail,
    label: `Change Email`,
  },
  {
    path: paths.openIdConnectClient,
    label: `OpenId Connect Client`,
  },
  {
    path: paths.openIdConnect,
    label: `OpenId Connect`,
  },
];

const Layout: React.FunctionComponent = ({ children }) => {
  // const [session] = useSession();
  const appTitle = `Building Envelope Data`;

  // useEffect(() => {
  //   if (session?.error === "RefreshAccessTokenError") {
  //     signIn(); // Force sign in to hopefully resolve error
  //   }
  // }, [session]);

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
      <AntLayout.Content style={{ padding: "50px" }}>
        {children}
      </AntLayout.Content>
      <AntLayout.Footer>
        <Footer />
      </AntLayout.Footer>
    </AntLayout>
  );
};

export default Layout;
