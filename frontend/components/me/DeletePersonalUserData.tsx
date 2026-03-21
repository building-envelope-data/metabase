import { useMutation } from "@apollo/client/react";
import { useApolloClient } from "@apollo/client/react";
import { Form, Input, Button, App } from "antd";
import { useRouter } from "next/router";
import { useState } from "react";
import paths from "../../paths";
import {
  DeletePersonalUserDataDocument,
  DeletePersonalUserDataMutation,
} from "../../queries/currentUser.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";

interface FormValues {
  password?: string;
}

interface DeletePersonalUserDataProps {
  hasPassword: boolean | null;
}

export function DeletePersonalUserData({
  hasPassword,
}: DeletePersonalUserDataProps) {
  const router = useRouter();

  const apolloClient = useApolloClient();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const { message } = App.useApp();

  const [deletePersoanlUserDataMutation] = useMutation(
    DeletePersonalUserDataDocument,
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<DeletePersonalUserDataMutation>({
      getErrors: (data) => data.deletePersonalUserData.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        deletePersoanlUserDataMutation({
          variables: {
            input: {
              password: values.password,
            },
          },
        }),
      {
        onSuccess: async () => {
          message.success("Your user data was deleted and account closed.");
          await apolloClient.resetStore();
          await router.push(paths.openIdConnectClientLogin);
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
        {hasPassword && (
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
          <Button type="primary" htmlType="submit" loading={mutating}>
            Delete data and close my account
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
