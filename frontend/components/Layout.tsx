import Head from "next/head";
import * as React from "react";
import Footer from "./Footer";
import NavBar from "./NavBar";
import { Layout as AntLayout } from "antd";
import paths from "../paths";

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
    path: paths.me.manage.index,
    label: `Account`,
  },
  {
    path: paths.openIdConnect,
    label: `OpenId Connect`,
  },
];

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
