import Head from "next/head";
import { useEffect } from "react";
import Footer from "./Footer";
import NavBar from "./NavBar";
import { Modal, Layout as AntLayout, Typography } from "antd";
import paths from "../paths";
import { useCookies } from "react-cookie";

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
    path: paths.databases,
    label: `Databases`,
  },
  {
    path: paths.data,
    label: `Data`,
  },
  {
    path: paths.users,
    label: `Users`,
  },
];

const Layout = ({ children }) => {
  const appTitle = `Building Envelope Data`;
  const cookieConsentName = "consent";
  const cookieConsentValue = "yes";

  const [cookies, setCookie] = useCookies([cookieConsentName]);

  useEffect(() => {
    if (cookies[cookieConsentName] != cookieConsentValue) {
      Modal.info({
        title: "Cookie Consent",
        content: (
          <Typography.Paragraph>
            This website employs cookies to make it work securely. As these
            cookies are essential you need to agree to their usage to use this
            website.
          </Typography.Paragraph>
        ),
        okText: "I agree",
        onOk: () => {
          setCookie(cookieConsentName, cookieConsentValue);
        },
      });
    }
  }, []);

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
