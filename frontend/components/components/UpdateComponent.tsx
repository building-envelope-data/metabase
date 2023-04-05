import {
  ComponentsDocument,
  useUpdateComponentMutation,
} from "../../queries/components.graphql";
import dayjs from "dayjs";
import { Alert, Form, Input, Button, Modal, DatePicker, Select } from "antd";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import {
  ComponentCategory,
  OpenEndedDateTimeRange,
  Scalars,
} from "../../__generated__/__types__";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  newName: string;
  newAbbreviation: string | null | undefined;
  newDescription: string;
  newAvailability:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  newCategories: ComponentCategory[] | null | undefined;
};

export type UpdateComponentProps = {
  componentId: Scalars["Uuid"];
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  availability: OpenEndedDateTimeRange | null | undefined;
  categories: ComponentCategory[] | null | undefined;
};

export default function UpdateComponent({
  componentId,
  name,
  abbreviation,
  description,
  availability,
  categories,
}: UpdateComponentProps) {
  const [open, setOpen] = useState(false);
  const [updateComponentMutation] = useUpdateComponentMutation({
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [
      {
        query: ComponentsDocument,
      },
    ],
  });
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>()
  );
  const [form] = Form.useForm<FormValues>();
  const [updating, setUpdating] = useState(false);

  const onFinish = ({
    newName,
    newAbbreviation,
    newDescription,
    newAvailability,
    newCategories,
  }: FormValues) => {
    const update = async () => {
      try {
        setUpdating(true);
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await updateComponentMutation({
          variables: {
            componentId: componentId,
            name: newName,
            abbreviation: newAbbreviation,
            description: newDescription,
            availability: {
              from: newAvailability?.[0],
              to: newAvailability?.[1],
            },
            categories: newCategories || [],
          },
        });
        handleFormErrors(
          errors,
          data?.updateComponent?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (
          !errors &&
          !data?.updateComponent?.errors &&
          data?.updateComponent?.component
        ) {
          setOpen(false);
        }
      } catch (error) {
        // TODO Handle properly.
        console.log("Failed:", error);
      } finally {
        setUpdating(false);
      }
    };
    update();
  };

  const onFinishFailed = () => {
    setGlobalErrorMessages(["Fix the errors below."]);
  };

  return (
    <>
      <Button onClick={() => setOpen(true)}>Edit</Button>
      <Modal
        open={open}
        title="Edit Component"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
        footer={false}
      >
        {/* TODO Display error messages in a list? */}
        {globalErrorMessages.length > 0 ? (
          <Alert type="error" message={globalErrorMessages.join(" ")} />
        ) : (
          <></>
        )}
        <Form
          {...layout}
          form={form}
          name="basic"
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            label="Name"
            name="newName"
            rules={[
              {
                required: true,
              },
            ]}
            initialValue={name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Abbreviation"
            name="newAbbreviation"
            initialValue={abbreviation}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Description"
            name="newDescription"
            rules={[
              {
                required: true,
              },
            ]}
            initialValue={description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Availability"
            name="newAvailability"
            initialValue={[
              availability?.from == null ? null : dayjs(availability.from),
              availability?.to == null ? null : dayjs(availability.to),
            ]}
          >
            <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
          </Form.Item>
          <Form.Item
            label="Categories"
            name="newCategories"
            initialValue={categories}
          >
            <Select
              mode="multiple"
              placeholder="Please select"
              options={Object.entries(ComponentCategory).map(
                ([_key, value]) => ({
                  label: value,
                  value: value,
                })
              )}
            />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={updating}>
              Update
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
