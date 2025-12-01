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
  SetUserPhoneNumberDocument,
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

  const [messageApi, contextHolder] = message.useMessage();

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
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm();
  const [setting, setSetting] = useState(false);

  const onFinish = ({ phoneNumber }: { phoneNumber: string }) => {
    const set = async () => {
      try {
        setSetting(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { error, data } = await setUserPhoneNumberMutation({
          variables: {
            input: {
              phoneNumber: phoneNumber,
            },
          },
        });
        handleFormErrors(
          error,
          data?.setUserPhoneNumber?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form,
        );
        if (!error && !data?.setUserPhoneNumber?.errors) {
          messageApi.success("Your new phone number was set.");
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setSetting(false);
      }
    };
    set();
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
      <Typography.Paragraph>Hello {currentUser.name}!</Typography.Paragraph>
      {/* TODO Change name, postal address, and website locator */}
      {globalErrorMessages.length > 0 && (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      )}
      <Form
        {...layout}
        form={form}
        name="basic"
        initialValues={{
          phoneNumber: currentUser.phoneNumber || undefined,
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
          <Button type="primary" htmlType="submit" loading={setting}>
            Set phone number
          </Button>
        </Form.Item>
      </Form>
    </ManageLayout>
  );
}

export default Page;
