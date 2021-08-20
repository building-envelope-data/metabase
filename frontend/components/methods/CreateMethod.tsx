import * as React from "react";
import { DatePicker, Select, Alert, Form, Input, Button, Divider } from "antd";
import {
  useCreateMethodMutation,
  MethodsDocument,
} from "../../queries/methods.graphql";
import {
  CreatePublicationInput,
  CreateStandardInput,
  MethodCategory,
  Scalars,
} from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { SelectMultipleInstitutionIds } from "../SelectMultipleInstitutionIds";
import { SelectMultipleUserIds } from "../SelectMultipleUserIds";
import { ReferenceForm } from "../ReferenceForm";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  name: string;
  description: string;
  validity:
    | [moment.Moment | null | undefined, moment.Moment | null | undefined]
    | null
    | undefined;
  availability:
    | [moment.Moment | null | undefined, moment.Moment | null | undefined]
    | null
    | undefined;
  standard: CreateStandardInput | null | undefined;
  publication: CreatePublicationInput | null | undefined;
  calculationLocator: Scalars["Url"] | null | undefined;
  categories: MethodCategory[] | null | undefined;
  institutionDeveloperIds: Scalars["Uuid"][] | null | undefined;
  userDeveloperIds: Scalars["Uuid"][] | null | undefined;
};

export type CreateMethodProps = {
  managerId: Scalars["Uuid"];
};

export default function CreateMethod({ managerId }: CreateMethodProps) {
  const [createMethodMutation] = useCreateMethodMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionDocument,
        variables: {
          uuid: managerId,
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
  const [form] = Form.useForm<FormValues>();
  const [creating, setCreating] = useState(false);

  const onFinish = ({
    name,
    description,
    validity,
    availability,
    standard,
    publication,
    calculationLocator,
    categories,
    institutionDeveloperIds,
    userDeveloperIds,
  }: FormValues) => {
    const create = async () => {
      try {
        setCreating(true);
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (standard != null && standard.standardizers == undefined) {
          standard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await createMethodMutation({
          variables: {
            name: name,
            description: description,
            validity: { from: validity?.[0], to: validity?.[1] },
            availability: { from: availability?.[0], to: availability?.[1] },
            standard: standard,
            publication: publication,
            calculationLocator: calculationLocator,
            categories: categories || [],
            managerId: managerId,
            institutionDeveloperIds: institutionDeveloperIds || [],
            userDeveloperIds: userDeveloperIds || [],
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
        <Form.Item
          label="Institution Developers"
          name="institutionDeveloperIds"
          initialValue={[]}
        >
          <SelectMultipleInstitutionIds />
        </Form.Item>
        <Form.Item
          label="User Developers"
          name="userDeveloperIds"
          initialValue={[]}
        >
          <SelectMultipleUserIds />
        </Form.Item>
        <Divider />
        <ReferenceForm form={form} />
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={creating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
