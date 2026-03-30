import { useMutation } from "@apollo/client/react";
import { DatePicker, Select, Form, Input, Button, Divider, Modal } from "antd";
import {
  UpdateMethodDocument,
  UpdateMethodMutation,
  MethodPartialFragment,
} from "../../queries/methods.generated";
import {
  MethodCategory,
  Scalars,
  ReferenceInput,
  MethodParameterInput,
  MethodSourceInput,
} from "../../__generated__/graphql";
import { useState } from "react";
import { ReferenceSubform } from "../ReferenceSubform";
import dayjs from "dayjs";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import { MethodParametersSubform } from "./MethodParametersSubform";
import { MethodSourcesSubform } from "./MethodSourcesSubform";
import EditButton from "../EditButton";

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
  parameters: MethodParameterInput[] | null | undefined;
  sources: MethodSourceInput[] | null | undefined;
};

interface UpdateMethodProps {
  method: MethodPartialFragment;
}

export default function UpdateMethod({ method }: UpdateMethodProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateMethodMutation] = useMutation(UpdateMethodDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateMethodMutation>({
      getErrors: (data) => data.updateMethod.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set sources, parameters, and standardizers to `[]`?
        if (
          values.reference?.standard != null &&
          values.reference?.standard.standardizers == undefined
        ) {
          values.reference.standard.standardizers = [];
        }
        if (values.parameters == undefined) {
          values.parameters = [];
        }
        if (values.sources == undefined) {
          values.sources = [];
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
              parameters: values.parameters,
              sources: values.sources,
            },
          },
        });
      },
      {
        onSuccess: () => {
          setGlobalErrorMessages([]);
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
      <EditButton onClick={() => setOpen(true)} />
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
            initialValue={[
              method.validity?.from == null
                ? null
                : dayjs(method.validity.from),
              method.validity?.to == null ? null : dayjs(method.validity.to),
            ]}
          >
            <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
          </Form.Item>
          <Form.Item
            label="Availability"
            name="availability"
            initialValue={[
              method.availability?.from == null
                ? null
                : dayjs(method.availability.from),
              method.availability?.to == null
                ? null
                : dayjs(method.availability.to),
            ]}
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
          <Form.Item label="Parameter(s)">
            <MethodParametersSubform
              namespace={["parameters"]}
              initialValue={method.parameters}
            />
          </Form.Item>
          <Form.Item label="Source(s)">
            <MethodSourcesSubform
              namespace={["sources"]}
              initialValue={method.sources}
            />
          </Form.Item>
          <Divider />
          <ReferenceSubform
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
