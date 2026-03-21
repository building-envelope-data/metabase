import { useMutation } from "@apollo/client/react";
import { Form, Input, Button, App } from "antd";
import { useState } from "react";
import {
  CurrentUserDocument,
  SetUserPhoneNumberDocument,
  SetUserPhoneNumberMutation,
} from "../../queries/currentUser.generated";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

interface FormValues {
  phoneNumber: string;
}

interface SetUserPhoneNumberProps {
  phoneNumber: string | null | undefined;
}

export function SetUserPhoneNumber({ phoneNumber }: SetUserPhoneNumberProps) {
  const { message } = App.useApp();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();

  const [setUserPhoneNumberMutation] = useMutation(SetUserPhoneNumberDocument, {
    update(cache, { data }) {
      // Read the data from our cache for this query.
      /* const { currentUser } = cache.readQuery({ query: CurrentUserDocument }) */
      /* const newCurrentUser = { ...currentUser } */
      // Add our comment from the mutation to the end.
      /* newCurrentUser.email = data.changeUserEmail.user.email */
      // Write our data back to the cache.
      if (data?.setUserPhoneNumber?.user)
        cache.writeQuery({
          query: CurrentUserDocument,
          data: {
            currentUser: data.setUserPhoneNumber.user,
          },
        });
    },
  });

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<SetUserPhoneNumberMutation>({
      getErrors: (data) => data.setUserPhoneNumber.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        setUserPhoneNumberMutation({
          variables: {
            input: {
              phoneNumber: values.phoneNumber,
            },
          },
        }),
      {
        onSuccess: () => {
          message.success("Your new phone number was set.");
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
        initialValues={{
          phoneNumber: phoneNumber,
        }}
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Phone Number"
          name="phoneNumber"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Set phone number
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
