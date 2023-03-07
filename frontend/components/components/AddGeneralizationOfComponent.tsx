import * as React from "react";
import { Alert, Form, Button } from "antd";
import { useAddComponentGeneralizationMutation } from "../../queries/componentGeneralizations.graphql";
import { Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { ComponentDocument } from "../../queries/components.graphql";
import { SelectComponentId } from "../SelectComponentId";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type AddAssembledOfComponentProps = {
  concreteComponentId: Scalars["Uuid"];
};

export default function AddAssembledOfComponent({
  concreteComponentId,
}: AddAssembledOfComponentProps) {
  const [addComponentGeneralizationMutation] =
    useAddComponentGeneralizationMutation({
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
      // See https://www.apollographql.com/docs/react/data/mutations/#options
      refetchQueries: [
        {
          query: ComponentDocument,
          variables: {
            uuid: concreteComponentId,
          },
        },
      ],
    });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [adding, setAdding] = useState(false);

  const onFinish = ({
    generalComponentId,
  }: {
    generalComponentId: Scalars["Uuid"];
  }) => {
    const add = async () => {
      try {
        setAdding(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await addComponentGeneralizationMutation({
          variables: {
            generalComponentId: generalComponentId,
            concreteComponentId: concreteComponentId,
          },
        });
        handleFormErrors(
          errors,
          data?.addComponentGeneralization?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.addComponentGeneralization?.errors) {
          form.resetFields();
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setAdding(false);
      }
    };
    add();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      {globalErrorMessages.length > 0 ? (
        <Alert type="error" message={globalErrorMessages.join(" ")} />
      ) : (
        <></>
      )}
      <Form
        {...layout}
        form={form}
        name="addGeneralComponent"
        onFinish={onFinish}
        onFinishFailed={onFinishFailed}
      >
        <Form.Item
          label="Generalization"
          name="generalComponentId"
          rules={[
            {
              required: true,
            },
          ]}
        >
          <SelectComponentId />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={adding}>
            Add
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
