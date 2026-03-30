import { useMutation } from "@apollo/client/react";
import { Form, Button, Space } from "antd";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { MethodDocument } from "../../queries/methods.generated";
import { UserIdSelect } from "../users/UserIdSelect";
import {
  AddUserMethodDeveloperDocument,
  AddUserMethodDeveloperMutation,
} from "../../queries/userMethodDevelopers.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = { userId: Scalars["Uuid"]["input"] };

interface AddUserMethodDeveloperProps {
  methodId: Scalars["Uuid"]["input"];
}

export default function AddUserMethodDeveloper({
  methodId,
}: AddUserMethodDeveloperProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [addUserMethodDeveloperMutation] = useMutation(
    AddUserMethodDeveloperDocument,
    {
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: MethodDocument,
          variables: {
            uuid: methodId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<AddUserMethodDeveloperMutation>({
      getErrors: (data) => data.addUserMethodDeveloper.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () =>
        addUserMethodDeveloperMutation({
          variables: {
            input: {
              methodId: methodId,
              userId: values.userId,
            },
          },
        }),
      {
        onSuccess: () => {
          setGlobalErrorMessages([]);
          form.resetFields();
        },
        onError: (graphQlErrors, userErrors) =>
          setGlobalErrorMessages(
            augmentFormWithErrors(graphQlErrors, userErrors, form),
          ),
      },
    );
  };

  return (
    <>
      <ErrorAlert messages={globalErrorMessages} />
      <Form
        form={form}
        name="addUserMethodDeveloper"
        onFinish={onFinish}
        style={{ display: "flex" }}
      >
        <Space.Compact style={{ flex: 1 }}>
          <Form.Item
            label="User"
            name="userId"
            rules={[
              {
                required: true,
              },
            ]}
            style={{ width: "100%" }}
          >
            <UserIdSelect />
          </Form.Item>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Add
          </Button>
        </Space.Compact>
      </Form>
    </>
  );
}
