import { useMutation } from "@apollo/client/react";
import { DatePicker, Select, Form, Input, Button, Divider, Modal } from "antd";
import {
  UpdateMethodDocument,
  UpdateMethodMutation,
  MethodsDocument,
} from "../../queries/methods.generated";
import {
  MethodCategory,
  Scalars,
  ReferenceInput,
  Method,
} from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { ReferenceForm } from "../ReferenceForm";
import dayjs from "dayjs";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";

type FormValues = {
  name: string;
  description: string;
  validity:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  availability:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  reference: ReferenceInput | null | undefined;
  calculationLocator: Scalars["Url"]["input"] | null | undefined;
  categories: MethodCategory[] | null | undefined;
};

export type UpdateMethodProps = {
  method: Pick<
    Method,
    | "uuid"
    | "name"
    | "description"
    | "validity"
    | "availability"
    | "reference"
    | "calculationLocator"
    | "categories"
  >;
  managerId: Scalars["Uuid"]["input"];
};

export default function UpdateMethod({ method, managerId }: UpdateMethodProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateMethodMutation] = useMutation(UpdateMethodDocument, {
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

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateMethodMutation>({
      getErrors: (data) => data.updateMethod.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (
          values.reference?.standard != null &&
          values.reference?.standard.standardizers == undefined
        ) {
          values.reference.standard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        return updateMethodMutation({
          variables: {
            input: {
              methodId: method.uuid,
              name: values.name,
              description: values.description,
              validity: {
                from: values.validity?.[0],
                to: values.validity?.[1],
              },
              availability: {
                from: values.availability?.[0],
                to: values.availability?.[1],
              },
              reference: values.reference,
              calculationLocator: values.calculationLocator,
              categories: values.categories || [],
              parameters: [],
              sources: [],
            },
          },
        });
      },
      {
        onSuccess: () => {
          setOpen(false);
        },
        onError: (graphQlErrors, userErrors) =>
          setGlobalErrorMessages(
            augmentFormWithErrors(graphQlErrors, userErrors, form),
          ),
      },
    );
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
        <ErrorAlert messages={globalErrorMessages} />
        <Form
          {...layout}
          form={form}
          name="updateMethod"
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
            initialValue={method.name}
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
            initialValue={method.description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Validity"
            name="validity"
            initialValue={method.validity}
          >
            <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
          </Form.Item>
          <Form.Item
            label="Availability"
            name="availability"
            initialValue={method.availability}
          >
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
            initialValue={method.calculationLocator}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Categories"
            name="categories"
            initialValue={method.categories}
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
          <ReferenceForm
            form={form}
            namespace={["reference"]}
            initialValue={method.reference}
          />
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Update
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
