import { NextRouter, useRouter } from "next/router";
import {
  InstitutionDocument,
  InstitutionsDocument,
  useCreateInstitutionMutation,
} from "../../queries/institutions.graphql";
import { Scalars } from "../../__generated__/__types__";
import { Skeleton, Alert, Form, Input, Button } from "antd";
import Layout from "../../components/Layout";
import paths from "../../paths";
import { useState, useEffect } from "react";
import { handleFormErrors } from "../../lib/form";
import { useCurrentUserQuery } from "../../queries/currentUser.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  name: string;
  abbreviation: string;
  description: string;
  websiteLocator: string;
};

export type CreateInstitutionProps = {
  ownerIds?: Scalars["Uuid"][];
  managerId?: Scalars["Uuid"];
};

function redirectToLoginPage(router: NextRouter): void {
  router.push({
    pathname: paths.userLogin,
    query: { returnTo: paths.institutionCreate },
  });
}

export default function CreateInstitution({
  ownerIds,
  managerId,
}: CreateInstitutionProps) {
  const router = useRouter();

  const currentUserQuery = useCurrentUserQuery();
  const currentUserLoading = currentUserQuery.loading;
  const currentUser = currentUserQuery.data?.currentUser;
  const shouldRedirect = !(
    currentUserLoading ||
    currentUserQuery.error ||
    currentUser
  );

  const [createInstitutionMutation] = useCreateInstitutionMutation({
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
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm<FormValues>();
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    if (router.isReady && shouldRedirect) {
      redirectToLoginPage(router);
    }
  }, [router, shouldRedirect]);

  const onFinish = ({
    name,
    abbreviation,
    description,
    websiteLocator,
  }: FormValues) => {
    const create = async () => {
      try {
        if (!currentUser) {
          return redirectToLoginPage(router);
        }
        setCreating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await createInstitutionMutation({
          variables: {
            name: name,
            abbreviation: abbreviation,
            description: description,
            websiteLocator: websiteLocator,
            ownerIds: ownerIds || [],
            managerId: managerId,
          },
        });
        handleFormErrors(
          errors,
          data?.createInstitution?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !managerId &&
          !errors &&
          !data?.createInstitution?.errors &&
          data?.createInstitution?.institution
        ) {
          await router.push(
            paths.institution(data.createInstitution.institution.uuid)
          );
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setCreating(false);
      }
    };
    create();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  if (currentUserLoading) {
    // TODO Handle this case properly.
    return (
      <Layout>
        <Skeleton active avatar title />
      </Layout>
    );
  }

  return (
    <>
      {/* TODO Display error messages in a list? */}
      {globalErrorMessages.length > 0 ? (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      ) : (
        <></>
      )}
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
        <Form.Item
          label="Website"
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
          <Button type="primary" htmlType="submit" loading={creating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
