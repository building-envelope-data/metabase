import Layout from "../../components/Layout";
import { Divider, List, Card, Typography, message, Skeleton } from "antd";
import { EditOutlined, MoreOutlined } from "@ant-design/icons";
import { useInstitutionQuery } from "../../queries/institutions.graphql";
import { useRouter } from "next/router";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";
import CreateComponent from "../../components/components/CreateComponent";

function Show() {
  const router = useRouter();

  const currentUserQuery = useCurrentUserQuery();
  const currentUser = currentUserQuery.data?.currentUser;

  const { uuid } = router.query;
  const { loading, error, data } = useInstitutionQuery({
    variables: {
      uuid: uuid,
    },
  });
  const institution = data?.institution;

  if (error) {
    message.error(error);
  }

  if (loading || !institution) {
    return <Layout></Layout>;
  }

  return (
    <Layout>
      {loading || !institution ? (
        <Skeleton active avatar title />
      ) : (
        <>
          <Typography.Title>
            {institution.websiteLocator ? (
              <Typography.Link href={institution.websiteLocator}>
                {`${institution.name} (${institution.abbreviation})`}
              </Typography.Link>
            ) : (
              `${institution.name} (${institution.abbreviation})`
            )}
          </Typography.Title>
          <Typography.Text>{institution.description}</Typography.Text>
        </>
      )}
      <Divider />
      <Typography.Title level={2}>Manufactured Components</Typography.Title>
      <List
        grid={{
          gutter: 16,
          xs: 1,
          sm: 2,
          md: 4,
          lg: 4,
          xl: 6,
          xxl: 3,
        }}
        dataSource={
          institution?.manufacturedComponents?.edges?.map((x) => x.node) || []
        }
        renderItem={(item) => (
          <Card
            actions={[<EditOutlined key="edit" />, <MoreOutlined key="more" />]}
          >
            <Skeleton loading={loading} active>
              <Card.Meta title={item.name} description={item.description} />
            </Skeleton>
          </Card>
        )}
      />
      {currentUser ? (
        <CreateComponent manufacturerId={institution.uuid} />
      ) : (
        <></>
      )}
      <Divider />
      <Typography.Title level={2}>Operated Databases</Typography.Title>
      <Divider />
      <Typography.Title level={2}>Representatives</Typography.Title>
    </Layout>
  );
}

export default Show;
