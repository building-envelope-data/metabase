import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
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
  ChangeUserEmailDocument,
  ResendUserEmailVerificationDocument,
} from "../../../queries/currentUser.generated";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

function Page() {
  const { data } = useQuery(CurrentUserDocument);
  const currentUser = data?.currentUser;

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
  const [resendUserEmailVerificationMutation] = useMutation(
    ResendUserEmailVerificationDocument,
  );
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const [changing, setChanging] = useState(false);

  const [messageApi, contextHolder] = message.useMessage();

  const [resending, setResending] = useState(false);
  const resendUserEmailVerification = async () => {
    try {
      setResending(true);
      const { error, data } = await resendUserEmailVerificationMutation();
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.resendUserEmailVerification?.errors) {
        // TODO Is this how we want to display errors?
        messageApi.error(
          data?.resendUserEmailVerification?.errors
            .map((error) => error.message)
            .join(" "),
        );
      } else {
        messageApi.success("Verification email sent. Please check your email.");
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
        const { error, data } = await changeUserEmailMutation({
          variables: {
            input: {
              newEmail: newEmail,
            },
          },
        });
        handleFormErrors(
          error,
          data?.changeUserEmail?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form,
        );
        if (!error && !data?.changeUserEmail?.errors) {
          messageApi.success(
            "Verification link to change email sent. Please check your email.",
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
      {contextHolder}
      <Typography.Paragraph>
        Your current email address is {currentUser.email}.
        {!currentUser.isEmailConfirmed && (
          <>
            Please verify it by following the verification link in the
            verification email you received. If you didn&apos;t receive a
            verification email, click the following button to resend it:{" "}
            <Button onClick={resendUserEmailVerification} loading={resending}>
              Resend verification email
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
