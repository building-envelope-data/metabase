import { message, Skeleton, Typography } from "antd";
import { useEffect, useState } from "react";
import Layout from "../components/Layout";

const url =
  "https://dsi.informationssicherheit.fraunhofer.de/en/impressum/metabase.buildingenvelopedata.org";
const errorMessage = "Legal notice could not be fetched.";

function Page() {
  // Inspired by https://www.robinwieruch.de/react-hooks-fetch-data
  const [data, setData] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const result = await fetch(url);
        if (result.ok) {
          setData(await result.text());
        } else {
          messageApi.error(errorMessage);
        }
      } catch (error) {
        console.log(error);
        messageApi.error(errorMessage);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [messageApi]);

  if (loading) {
    return (
      <>
        {contextHolder}
        <Layout>
          <Skeleton />
        </Layout>
      </>
    );
  }

  if (!data) {
    return (
      <>
        {contextHolder}
        <Layout>
          You can view the legal notice under{" "}
          <Typography.Link href={url}>{url}</Typography.Link>
        </Layout>
      </>
    );
  }

  return (
    <Layout>
      <div dangerouslySetInnerHTML={{ __html: data }}></div>
    </Layout>
  );
}

export default Page;
