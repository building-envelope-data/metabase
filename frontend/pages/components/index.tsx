import { useQuery } from "@apollo/client/react";
import Layout from "../../components/Layout";
import { Typography } from "antd";
import { ComponentsDocument } from "../../queries/components.generated";
import paths from "../../paths";
import Link from "next/link";
import { ComponentTable } from "../../components/components/ComponentTable";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";

// TODO Pagination. See https://www.apollographql.com/docs/react/pagination/core-api/

function Page() {
	const { loading, error, data } = useQuery(ComponentsDocument);
	const nodes = data?.components?.edges?.map((e) => e.node) || [];

	useQueryHandler({ error });

	return (
		<Layout>
			<Typography.Paragraph style={{ maxWidth: 768 }}>
				The building envelope components for which{" "}
				<Link href={paths.data}>data</Link> is available are presented here.
			</Typography.Paragraph>
			<ComponentTable loading={loading} components={nodes} />
			<Typography.Paragraph style={{ maxWidth: 768 }}>
				The <Typography.Link href="/graphql/">GraphQL endpoint</Typography.Link>{" "}
				provides all information about components.
			</Typography.Paragraph>
		</Layout>
	);
}

export default Page;
