import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import ApplicationTable from "../../components/openIdConnect/applications/ApplicationTable";
import paths from "../../paths";
import { ApplicationsDocument } from "../../queries/openIdConnect.generated";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";
import { stringifyApolloError } from "../../lib/apollo";
import { useEffect } from "react";
import { App } from "antd";

function Page() {
	useRequireAuth({ returnTo: paths.openIdConnect });

	const { loading, error, data } = useQuery(ApplicationsDocument);
	const { message } = App.useApp();

	useEffect(() => {
		if (error) {
			message.error(stringifyApolloError(error));
		}
	}, [error, message]);

	return (
		<Layout>
			<ApplicationTable
				loading={loading}
				applications={data?.openIdConnectApplications || []}
			/>
		</Layout>
	);
}

export default Page;
