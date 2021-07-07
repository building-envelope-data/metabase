import * as React from "react";
import { DatePicker, Select, Alert, Form, Input, Button } from "antd";
import {
  useCreateMethodMutation,
  Scalars,
  MethodsDocument,
  MethodCategory,
} from "../../queries/methods.graphql";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.graphql";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type CreateMethodProps = {
  institutionDeveloperId: Scalars["Uuid"];
};

export const CreateMethod: React.FunctionComponent<CreateMethodProps> = ({
  institutionDeveloperId,
}) => {
  const [createMethodMutation] = useCreateMethodMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionDocument,
        variables: {
          uuid: institutionDeveloperId,
        },
      },
      {
        query: MethodsDocument,
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm();
  const [creating, setCreating] = useState(false);

  const onFinish = ({
    name,
    description,
    validity,
    availability,
    // standard,
    // publication,
    calculationLocator,
    categories,
  }: {
    name: string;
    description: string;
    validity: [moment.Moment | null, moment.Moment | null] | null;
    availability: [moment.Moment | null, moment.Moment | null] | null;
    // standard: CreateStandardInput;
    // publication: CreatePublicationInput;
    calculationLocator: Scalars["Url"] | null;
    categories: MethodCategory[] | null;
    // institutionDeveloperIds: Scalars["Uuid"][];
    // userDeveloperIds: Scalars["Uuid"][];
  }) => {
    const create = async () => {
      try {
        setCreating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await createMethodMutation({
          variables: {
            name: name,
            description: description,
            validity: { from: validity?.[0], to: validity?.[1] },
            availability: { from: availability?.[0], to: availability?.[1] },
            // standard: standard,
            // publication: publication,
            calculationLocator: calculationLocator,
            categories: categories || [],
            institutionDeveloperIds: [institutionDeveloperId],
            userDeveloperIds: [],
          },
        });
        handleFormErrors(
          errors,
          data?.createMethod?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
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
        name="createMethod"
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
        <Form.Item label="Validity" name="validity">
          <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
        </Form.Item>
        <Form.Item label="Availability" name="availability">
          <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
        </Form.Item>
        {/* TODO $standard: CreateStandardInput
            $publication: CreatePublicationInput */}
        <Form.Item
          label="Calculation Locator"
          name="calculationLocator"
          rules={[
            {
              required: false,
            },
            {
              type: "url",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item label="Categories" name="categories" initialValue={[]}>
          <Select mode="multiple" placeholder="Please select">
            <Select.Option value={MethodCategory.Measurement}>
              Measurement
            </Select.Option>
            <Select.Option value={MethodCategory.Calculation}>
              Calculation
            </Select.Option>
          </Select>
        </Form.Item>
        {/* TODO $institutionDeveloperIds: [Uuid]
        $userDeveloperIds: [Uuid] */}
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={creating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
};

export default CreateMethod;
