import Head from "next/head";
import { ReactNode, useEffect } from "react";
import Footer from "./Footer";
import NavBar from "./NavBar";
import { Layout as AntLayout, App, Divider, Flex, Typography } from "antd";
import paths from "../paths";
import { useCookies } from "react-cookie";

const navItems = [
  {
    path: paths.home,
    label: "Home",
    subitems: null,
  },
  {
    label: "Data",
    subitems: [
      {
        path: paths.allCalorimetricData,
        label: "Calorimetric Data",
      },
      {
        path: paths.allGeometricData,
        label: "Geometric Data",
      },
      {
        path: paths.allHygrothermalData,
        label: "Hygrothermal Data",
      },
      {
        path: paths.allLifeCycleData,
        label: "Life-Cycle Data",
      },
      {
        path: paths.allOpticalData,
        label: "Optical Data",
      },
      {
        path: paths.allPhotovoltaicData,
        label: "Photovoltaic Data",
      },
    ],
  },
  {
    path: paths.components,
    label: "Components",
    subitems: null,
  },
  {
    path: paths.institutions,
    label: "Institutions",
    subitems: null,
  },
  {
    path: paths.databases,
    label: "Databases",
    subitems: null,
  },
  {
    path: paths.dataFormats,
    label: "Data Formats",
    subitems: null,
  },
  {
    path: paths.methods,
    label: "Methods",
    subitems: null,
  },
  {
    path: paths.users,
    label: "Users",
    subitems: null,
  },
];

interface LayoutProps {
  children?: ReactNode;
}

const cookieConsentName = "consent";
const cookieConsentValue = "yes";

export default function Layout({ children }: LayoutProps) {
  const appTitle = "Building Envelope Data";

  const [cookies, setCookie] = useCookies([cookieConsentName]);
  const shouldShowCookieConsent =
    cookies[cookieConsentName] != cookieConsentValue;
  const { modal } = App.useApp();

  useEffect(() => {
    if (shouldShowCookieConsent) {
      modal.info({
        title: "Cookie Consent",
        content: (
          <Typography.Paragraph style={{ maxWidth: "75ch" }}>
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
  }, [shouldShowCookieConsent, setCookie, modal]);

  return (
    <AntLayout>
      <Head>
        <title>{appTitle}</title>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta charSet="utf-8" />
      </Head>
      <AntLayout.Header>
        <Flex justify="center">
          <NavBar
            items={navItems}
            style={{
              width: "100%",
              maxWidth: 1024,
            }}
          />
        </Flex>
      </AntLayout.Header>
      <AntLayout.Content
        style={{
          paddingTop: "24px",
          paddingBottom: "24px",
        }}
      >
        <Flex justify="center">
          <div style={{ width: "100%", maxWidth: 1024 }}>{children}</div>
        </Flex>
      </AntLayout.Content>
      <AntLayout.Footer>
        <Divider />
        <Flex justify="center">
          <Footer />
        </Flex>
      </AntLayout.Footer>
    </AntLayout>
  );
}
