import { useMutation } from "@apollo/client/react";
import { useRouter } from "next/router";
import {
	InstitutionDocument,
	InstitutionsDocument,
	CreateInstitutionDocument,
	CreateInstitutionMutation,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import { Form, Input, Button } from "antd";
import paths from "../../paths";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

const layout = {
	labelCol: { span: 8 },
	wrapperCol: { span: 16 },
};
const tailLayout = {
	wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
	name: string;
	abbreviation: string | null | undefined;
	description: string;
	phoneNumber: string | null | undefined;
	postalAddress: string | null | undefined;
	emailAddress: string | null | undefined;
	websiteLocator: string | null | undefined;
};

export type CreateInstitutionProps = {
	ownerIds?: Scalars["Uuid"]["input"][];
	managerId?: Scalars["Uuid"]["input"];
};

export default function CreateInstitution({
	ownerIds,
	managerId,
}: CreateInstitutionProps) {
	const router = useRouter();

	const [createInstitutionMutation] = useMutation(CreateInstitutionDocument, {
		// TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
		// See https://www.apollographql.com/docs/react/data/mutations/#options
		refetchQueries: [
			{
				query: InstitutionsDocument,
			},
			...(managerId
				? [
						{
							query: InstitutionDocument,
							variables: { uuid: managerId },
						},
					]
				: []),
		],
	});
	const {
		globalErrorMessages,
		setGlobalErrorMessages,
		form,
		loading,
		withMutationHandler,
	} = useMutationHandler<
		CreateInstitutionMutation,
		"createInstitution",
		FormValues
	>({
		payloadKey: "createInstitution",
		getErrors: (payload) => payload.errors,
		onSuccess: (payload) => {
			if (payload?.institution && !managerId) {
				return router.push(paths.institution(payload.institution.uuid));
			}
		},
	});

	const onFinish = ({
		name,
		abbreviation,
		description,
		phoneNumber,
		postalAddress,
		emailAddress,
		websiteLocator,
	}: FormValues) => {
		withMutationHandler(() =>
			// https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
			createInstitutionMutation({
				variables: {
					input: {
						name: name,
						abbreviation: abbreviation,
						description: description,
						contact: {
							phoneNumber: phoneNumber,
							postalAddress: postalAddress,
							emailAddress: emailAddress,
							websiteLocator: websiteLocator,
						},
						ownerIds: ownerIds || [],
						managerId: managerId,
					},
				},
			}),
		);
	};

	const onFinishFailed = () => {
		setGlobalErrorMessages(["Fix the errors below."]);
	};

	return (
		<>
			<ErrorAlert messages={globalErrorMessages} />
			<Form
				{...layout}
				form={form}
				name="basic"
				onFinish={onFinish}
				onFinishFailed={onFinishFailed}
			>
				<Form.Item
					label="Name"
					name="name"
					rules={[
						{
							required: true,
						},
					]}
				>
					<Input />
				</Form.Item>
				<Form.Item label="Abbreviation" name="abbreviation">
					<Input />
				</Form.Item>
				<Form.Item
					label="Description"
					name="description"
					rules={[
						{
							required: true,
						},
					]}
				>
					<Input />
				</Form.Item>
				<Form.Item label="Phone Number" name="phoneNumber">
					<Input />
				</Form.Item>
				<Form.Item label="Postal Address" name="postalAddress">
					<Input />
				</Form.Item>
				<Form.Item
					label="E-Mail Address"
					name="emailAddress"
					rules={[
						{
							type: "email",
						},
					]}
				>
					<Input />
				</Form.Item>
				<Form.Item
					label="Website Locator"
					name="websiteLocator"
					rules={[
						{
							type: "url",
						},
					]}
				>
					<Input />
				</Form.Item>
				<Form.Item {...tailLayout}>
					<Button type="primary" htmlType="submit" loading={loading}>
						Create
					</Button>
				</Form.Item>
			</Form>
		</>
	);
}
