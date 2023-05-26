import Head from "next/head";
import { ReactNode, useEffect, useState } from "react";
import Footer from "./Footer";
import { Modal, Layout as AntLayout, Typography } from "antd";
import paths from "../paths";
import { useCookies } from "react-cookie";

export type SingleSignOnLayoutProps = {
  children?: ReactNode;
};

const cookieConsentName = "consent";
const cookieConsentValue = "yes";

export default function SingleSignOnLayout({
  children,
}: SingleSignOnLayoutProps) {
  const appTitle = "Single-Sign On &bull; Building Envelope Data";

  const [cookies, setCookie] = useCookies([cookieConsentName]);
  const shouldShowCookieConsent =
    cookies[cookieConsentName] != cookieConsentValue;
  const [loadedAntiforgeryToken, setLoadedAntiforgeryToken] = useState(false);

  useEffect(() => {
    if (shouldShowCookieConsent) {
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
  }, [shouldShowCookieConsent, setCookie]);

  useEffect(() => {
    fetch(paths.antiforgeryToken).then((_) => {
      setLoadedAntiforgeryToken(true);
    });
  }, []);

  if (!loadedAntiforgeryToken) {
    return null;
  }

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
