import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, App } from "antd";
import { useState } from "react";
import {
  CurrentUserDocument,
  ChangeUserEmailDocument,
  ChangeUserEmailMutation,
} from "../../queries/currentUser.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../../components/ErrorAlert";

interface FormValues {
  newEmail: string;
}

export function ChangeUserEmail() {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const { message } = App.useApp();

  const [changeUserEmailMutation] = useMutation(ChangeUserEmailDocument, {
    update(cache, { data }) {
      // Read the data from our cache for this query.
      /* const { currentUser } = cache.readQuery({ query: CurrentUserDocument }) */
      /* const newCurrentUser = { ...currentUser } */
      // Add our comment from the mutation to the end.
      /* newCurrentUser.email = data.changeUserEmail.user.email */
      // Write our data back to the cache.
      if (data?.changeUserEmail?.user)
        cache.writeQuery({
          query: CurrentUserDocument,
          data: {
            currentUser: data.changeUserEmail.user,
          },
        });
    },
  });

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<ChangeUserEmailMutation>({
      getErrors: (data) => data.changeUserEmail.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        changeUserEmailMutation({
          variables: {
            input: {
              newEmail: values.newEmail,
            },
          },
        }),
      {
        onSuccess: () => {
          message.success(
            "Verification link to change email sent. Please check your email.",
          );
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
          label="New Email"
          name="newEmail"
          rules={[
            {
              required: true,
              message: "Please input your email!",
            },
            {
              type: "email",
              message: "Invalid email!",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Change Email
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
