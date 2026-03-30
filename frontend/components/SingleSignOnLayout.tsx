import Head from "next/head";
import { ReactNode, useEffect } from "react";
import Footer from "./Footer";
import { Layout as AntLayout, Typography, App } from "antd";
import { useCookies } from "react-cookie";

interface SingleSignOnLayoutProps {
  children?: ReactNode;
};

const cookieConsentName = "consent";
const cookieConsentValue = "yes";

export default function SingleSignOnLayout({
  children,
}: SingleSignOnLayoutProps) {
  const appTitle = "Single-Sign On • Building Envelope Data";

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
  }, [shouldShowCookieConsent, setCookie]);

  return (
    <AntLayout>
      <Head>
        <title>{appTitle}</title>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta charSet="utf-8" />
      </Head>
      <AntLayout.Content style={{ padding: "50px" }}>
        {children}
      </AntLayout.Content>
      <AntLayout.Footer>
        <Footer />
      </AntLayout.Footer>
    </AntLayout>
  );
}
