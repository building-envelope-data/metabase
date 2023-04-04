import * as React from "react";
import {
  DatePicker,
  Select,
  Alert,
  Form,
  Input,
  Button,
  Divider,
  Modal,
} from "antd";
import {
  useUpdateMethodMutation,
  MethodsDocument,
} from "../../queries/methods.graphql";
import {
  UpdatePublicationInput,
  UpdateStandardInput,
  MethodCategory,
  Scalars,
  OpenEndedDateTimeRange,
  Reference,
} from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import { InstitutionDocument } from "../../queries/institutions.graphql";
import { ReferenceForm } from "../ReferenceForm";
import * as dayjs from "dayjs";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

type FormValues = {
  newName: string;
  newDescription: string;
  newValidity:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  newAvailability:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  standard: UpdateStandardInput | null | undefined;
  publication: UpdatePublicationInput | null | undefined;
  newCalculationLocator: Scalars["Url"] | null | undefined;
  newCategories: MethodCategory[] | null | undefined;
};

export type UpdateMethodProps = {
  methodId: Scalars["Uuid"];
  name: string;
  description: string;
  validity: OpenEndedDateTimeRange | null | undefined;
  availability: OpenEndedDateTimeRange | null | undefined;
  reference: Reference | null | undefined;
  calculationLocator: Scalars["Url"] | null | undefined;
  categories: MethodCategory[] | null | undefined;
  managerId: Scalars["Uuid"];
};

export default function UpdateMethod({
  methodId,
  name,
  description,
  validity,
  availability,
  reference,
  calculationLocator,
  categories,
  managerId,
}: UpdateMethodProps) {
  const [open, setOpen] = useState(false);
  const [updateMethodMutation] = useUpdateMethodMutation({
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
  const [updating, setUpdating] = useState(false);

  const onFinish = ({
    newName,
    newDescription,
    newValidity,
    newAvailability,
    standard: newStandard,
    publication: newPublication,
    newCalculationLocator,
    newCategories,
  }: FormValues) => {
    const update = async () => {
      try {
        setUpdating(true);
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (newStandard != null && newStandard.standardizers == undefined) {
          newStandard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await updateMethodMutation({
          variables: {
            methodId: methodId,
            name: newName,
            description: newDescription,
            validity: { from: newValidity?.[0], to: newValidity?.[1] },
            availability: {
              from: newAvailability?.[0],
              to: newAvailability?.[1],
            },
            standard: newStandard,
            publication: newPublication,
            calculationLocator: newCalculationLocator,
            categories: newCategories || [],
          },
        });
        handleFormErrors(
          errors,
          data?.updateMethod?.errors?.map((x) => {
            return { code: x.code, message: x.message, path: x.path };
          }),
          setGlobalErrorMessages,
          form
        );
        if (!errors && !data?.updateMethod?.errors) {
          form.resetFields();
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
        title="Edit Method"
        // onOk={handleOk}
        onCancel={() => setOpen(false)}
        footer={false}
      >
        {globalErrorMessages.length > 0 ? (
          <Alert type="error" message={globalErrorMessages.join(" ")} />
        ) : (
          <></>
        )}
        <Form
          {...layout}
          form={form}
          name="updateMethod"
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
            label="Validity"
            name="newValidity"
            initialValue={validity}
          >
            <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
          </Form.Item>
          <Form.Item
            label="Availability"
            name="newAvailability"
            initialValue={availability}
          >
            <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
          </Form.Item>
          <Form.Item
            label="Calculation Locator"
            name="newCalculationLocator"
            rules={[
              {
                required: false,
              },
              {
                type: "url",
              },
            ]}
            initialValue={calculationLocator}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Categories"
            name="newCategories"
            initialValue={categories}
          >
            <Select
              mode="multiple"
              placeholder="Please select"
              options={Object.entries(MethodCategory).map(([_key, value]) => ({
                label: value,
                value: value,
              }))}
            />
          </Form.Item>
          <Divider />
          <ReferenceForm form={form} />
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
