import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import ApplicationTable from "../../components/openIdConnect/applications/ApplicationTable";
import paths from "../../paths";
import { ApplicationsDocument } from "../../queries/openIdConnect.generated";
import { useRequireAuth } from "../../lib/hooks/useRequireAuth";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

function Page() {
	useRequireAuth({ returnTo: paths.openIdConnect });
	const { loading, error, data } = useQuery(ApplicationsDocument);
	useQueryHandler({ error });

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
