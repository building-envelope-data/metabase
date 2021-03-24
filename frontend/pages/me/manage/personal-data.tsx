import { useApolloClient } from "@apollo/client";
import {
  Form,
  Typography,
  Alert,
  Input,
  Button,
  message,
  Skeleton,
} from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import ManageLayout from "../../../components/me/ManageLayout";
import { handleFormErrors } from "../../../lib/form";
import paths from "../../../paths";
import {
  useCurrentUserQuery,
  useDeletePersonalUserDataMutation,
} from "../../../queries/currentUser.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const router = useRouter();

  const { data } = useCurrentUserQuery();
  const currentUser = data?.currentUser;

  const apolloClient = useApolloClient();

  const [deletePersoanlUserDataMutation] = useDeletePersonalUserDataMutation();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [deleting, setDeleting] = useState(false);

  const onFinish = ({ password }: { password?: string }) => {
    const del = async () => {
      try {
        setDeleting(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await deletePersoanlUserDataMutation({
          variables: {
            password: password,
          },
        });
        handleFormErrors(
          errors,
          data?.deletePersonalUserData?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.deletePersonalUserData?.errors) {
          message.success("Your user data was deleted and account closed.");
          await apolloClient.resetStore();
          await router.push(paths.userLogin);
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setDeleting(false);
      }
    };
    del();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  if (!currentUser) {
    return (
      <ManageLayout>
        <Skeleton />
      </ManageLayout>
    );
  }

  return (
    <ManageLayout>
      <Typography.Paragraph>
        Your account contains personal data that you have given us. This page
        allows you to download or delete that data in accordance with the
        <Typography.Link href="https://gdpr.eu">
          General Data Protection Regulation (GDPR)
        </Typography.Link>
      </Typography.Paragraph>
      <Typography.Paragraph strong>
        Deleting this data will permanently remove your account, and this cannot
        be recovered.
      </Typography.Paragraph>
      <Typography.Link href={paths.personalUserData}>
        Download Personal User Data
      </Typography.Link>
      {globalErrorMessages.length > 0 && (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      )}
      <Form
        {...layout}
        form={form}
        name="basic"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        {currentUser.hasPassword && (
          <Form.Item
            label="Password"
            name="password"
            rules={[
              {
                required: true,
              },
            ]}
          >
            <Input.Password />
          </Form.Item>
        )}
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={deleting}>
            Delete data and close my account
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
