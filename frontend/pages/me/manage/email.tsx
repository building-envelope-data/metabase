import {
  Form,
  Typography,
  Alert,
  Input,
  Button,
  message,
  Skeleton,
} from "antd";
import { useState } from "react";
import ManageLayout from "../../../components/me/ManageLayout";
import { handleFormErrors } from "../../../lib/form";
import {
  CurrentUserDocument,
  useCurrentUserQuery,
  useChangeUserEmailMutation,
  useResendUserEmailVerificationMutation,
} from "../../../queries/currentUser.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const { data } = useCurrentUserQuery();
  const currentUser = data?.currentUser;

  const [changeUserEmailMutation] = useChangeUserEmailMutation({
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
  const [
    resendUserEmailVerificationMutation,
  ] = useResendUserEmailVerificationMutation();
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [changing, setChanging] = useState(false);

  const [resending, setResending] = useState(false);
  const resendUserEmailVerification = async () => {
    try {
      setResending(true);
      const { errors, data } = await resendUserEmailVerificationMutation();
      if (errors) {
        console.log(errors); // TODO What to do?
      } else if (data?.resendUserEmailVerification?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.resendUserEmailVerification?.errors
            .map((error) => error.message)
            .join(" ")
        );
      } else {
        message.success("Verification email sent. Please check your email.");
      }
    } finally {
      setResending(false);
    }
  };

  const onFinish = ({ newEmail }: { newEmail: string }) => {
    const change = async () => {
      try {
        setChanging(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await changeUserEmailMutation({
          variables: {
            newEmail: newEmail,
          },
        });
        handleFormErrors(
          errors,
          data?.changeUserEmail?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.changeUserEmail?.errors) {
          message.success(
            "Confirmation link to change email sent. Please check your email."
          );
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setChanging(false);
      }
    };
    change();
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
        Your current email address is {currentUser.email}.
        {!currentUser.emailConfirmed && (
          <>
            Please confirm it by clicking the confirmation link in the
            confirmation email you received. If you didn't receive a
            confirmation email, click the following button to resend it:{" "}
            <Button onClick={resendUserEmailVerification} loading={resending}>
              Resend confirmation email
            </Button>
          </>
        )}
      </Typography.Paragraph>
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
          <Button type="primary" htmlType="submit" loading={changing}>
            Change Email
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
