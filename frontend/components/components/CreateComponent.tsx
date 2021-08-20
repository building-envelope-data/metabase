import * as React from "react";
import { DatePicker, Alert, Select, Form, Input, Button } from "antd";
import {
  useCreateComponentMutation,
  ComponentsDocument,
} from "../../queries/components.graphql";
import { ComponentCategory, Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import * as moment from "moment";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { SelectInstitutionId } from "../SelectInstitutionId";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type CreateComponentProps = {
  manufacturerId: Scalars["Uuid"];
};

export default function CreateComponent({
  manufacturerId,
}: CreateComponentProps) {
  const [createComponentMutation] = useCreateComponentMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: InstitutionDocument,
        variables: {
          uuid: manufacturerId,
        },
      },
      {
        query: ComponentsDocument,
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
    abbreviation,
    description,
    availability,
    categories,
    furtherManufacturerIds,
  }: {
    name: string;
    abbreviation: string | null;
    description: string;
    availability: [moment.Moment | null, moment.Moment | null] | null;
    categories: ComponentCategory[];
    furtherManufacturerIds: Scalars["Uuid"];
  }) => {
    const create = async () => {
      try {
        setCreating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await createComponentMutation({
          variables: {
            name: name,
            abbreviation: abbreviation,
            description: description,
            availability: { from: availability?.[0], to: availability?.[1] },
            categories: categories || [],
            manufacturerId: manufacturerId,
            furtherManufacturerIds: furtherManufacturerIds,
          },
        });
        handleFormErrors(
          errors,
          data?.createComponent?.errors?.map((x) => {
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
        name="createComponent"
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
        <Form.Item label="Abbreviation" name="abbreviation">
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
        <Form.Item label="Availability" name="availability">
          <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
        </Form.Item>
        <Form.Item label="Categories" name="categories">
          <Select
            mode="multiple"
            placeholder="Please select"
            options={Object.entries(ComponentCategory).map(([_key, value]) => ({
              label: value,
              value: value,
            }))}
          />
        </Form.Item>
        <Form.Item label="Further Manufacturers" name="furtherManufacturerIds">
          <SelectInstitutionId mode="multiple" />
        </Form.Item>
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={creating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
