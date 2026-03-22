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
import { useState } from "react";
import { layout, tailLayout } from "../../lib/form";

type ContactFormValues = {
  phoneNumber: string | null | undefined;
  postalAddress: string | null | undefined;
  emailAddress: string | null | undefined;
  websiteLocator: string | null | undefined;
};

type FormValues = {
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  contact: ContactFormValues | null | undefined;
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
  const [globalErrorMessages, setGlobalErrorMessages] = useState<string[]>([]);
  const [form] = Form.useForm<FormValues>();

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
    mutating,
    withMutationHandler,
    messageMissingModel,
    augmentFormWithErrors,
  } = useMutationHandler<CreateInstitutionMutation>({
    getErrors: (data) => data.createInstitution.errors,
  });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        createInstitutionMutation({
          variables: {
            input: {
              name: values.name,
              abbreviation: values.abbreviation,
              description: values.description,
              contact: {
                phoneNumber: values.contact?.phoneNumber,
                postalAddress: values.contact?.postalAddress,
                emailAddress: values.contact?.emailAddress,
                websiteLocator: values.contact?.websiteLocator,
              },
              ownerIds: ownerIds || [],
              managerId: managerId,
            },
          },
        }),
      {
        onSuccess: (data) => {
          if (!managerId) {
            const model = data?.createInstitution?.institution;
            if (!model) {
              messageMissingModel();
            } else {
              return router.push(paths.institution(model.uuid));
            }
          }
        },
        onError: (graphQlErrors, userErrors) =>
          setGlobalErrorMessages(
            augmentFormWithErrors(graphQlErrors, userErrors, form),
          ),
      },
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
        <Form.Item label="Phone Number" name={["contact", "phoneNumber"]}>
          <Input />
        </Form.Item>
        <Form.Item label="Postal Address" name={["contact", "postalAddress"]}>
          <Input />
        </Form.Item>
        <Form.Item
          label="E-Mail Address"
          name={["contact", "emailAddress"]}
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
          name={["contact", "websiteLocator"]}
          rules={[
            {
              type: "url",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
