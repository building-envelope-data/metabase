import { useRouter } from "next/router";
import { Skeleton, Row, Col, Card, message } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useEffect } from "react";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import CreateInstitution from "../../components/institutions/CreateInstitution";

function Create() {
  const router = useRouter();

  const { loading, data, error } = useCurrentUserQuery();
  const currentUser = data?.currentUser;

  useEffect(() => {
    if (!loading && !currentUser) {
      redirectToLoginPage();
    }
  }, [loading, currentUser]);

  useEffect(() => {
    if (error) {
      message.error(error);
    }
  }, [error]);

  const redirectToLoginPage = () => {
    router.push({
      pathname: paths.userLogin,
      query: { returnTo: paths.institutionCreate },
    });
  };

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

export default Create;
