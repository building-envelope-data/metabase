import { useQuery } from "@apollo/client/react";
import { stringifyApolloError } from "../../lib/apollo";
import { NextRouter, useRouter } from "next/router";
import { Skeleton, Row, Col, Card, message } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useEffect } from "react";
import { CurrentUserDocument } from "../../queries/currentUser.generated";
import CreateInstitution from "../../components/institutions/CreateInstitution";

function redirectToLoginPage(router: NextRouter): void {
  router.push({
    pathname: paths.userLogin,
    query: { returnTo: paths.institutionCreate },
  });
}

function Page() {
  const router = useRouter();

  const { loading, data, error } = useQuery(CurrentUserDocument);
  const currentUser = data?.currentUser;
  const shouldRedirect = !(loading || error || currentUser);
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (error) {
      messageApi.error(stringifyApolloError(error));
    }
  }, [error]);

  useEffect(() => {
    if (router.isReady && shouldRedirect) {
      redirectToLoginPage(router);
    }
  }, [shouldRedirect, router]);

  if (loading || !currentUser) {
    // TODO Handle this case properly.
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  return (
    <Layout>
      {contextHolder}
      <Row justify="center">
        <Col>
          <Card title="Create">
            <CreateInstitution ownerIds={[currentUser.uuid]} />
          </Card>
        </Col>
      </Row>
    </Layout>
  );
}

export default Page;
