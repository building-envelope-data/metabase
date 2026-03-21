import { Skeleton, Row, Col, Card } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import CreateInstitution from "../../components/institutions/CreateInstitution";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";

function Page() {
	const { currentUser } = useRequireAuth({ returnTo: paths.institutionCreate });

	if (!currentUser) {
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

export default Page;
