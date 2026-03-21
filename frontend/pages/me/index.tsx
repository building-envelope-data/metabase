import { Skeleton } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";

function Page() {
	const { authenticated } = useRequireAuth({ returnTo: paths.userCurrent });

	if (!authenticated) {
		return (
			<Layout>
				<Skeleton active avatar title />
			</Layout>
		);
	}

	return <Layout></Layout>;
}

export default Page;
