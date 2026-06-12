import { useMutation } from "@apollo/client/react";
import {
  UpdateComponentDocument,
  UpdateComponentMutation,
  ComponentPartialFragment,
} from "../../queries/components.generated";
import dayjs from "dayjs";
import { Form, Input, Button, Modal, DatePicker, Divider } from "antd";
import { useState } from "react";
import {
  ComponentCategory,
  DescriptionOrReferenceInput,
} from "../../__generated__/graphql";
import ReferenceSubform from "../ReferenceSubform";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import EditButton from "../EditButton";
import EnumSelect from "../EnumSelect";

type FormValues = {
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  availability:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  categories: ComponentCategory[] | null | undefined;
  primeSurface: DescriptionOrReferenceInput | null | undefined;
  primeDirection: DescriptionOrReferenceInput | null | undefined;
  switchableLayers: DescriptionOrReferenceInput | null | undefined;
};

interface UpdateComponentProps {
  component: ComponentPartialFragment;
}

export default function UpdateComponent({ component }: UpdateComponentProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [updateComponentMutation] = useMutation(UpdateComponentDocument);

  const { mutating, withMutationHandler, augmentFormWithErrors } =
    useMutationHandler<UpdateComponentMutation>({
      getErrors: (data) => data.updateComponent.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (
          values.primeSurface?.reference?.standard != null &&
          values.primeSurface.reference.standard.standardizers == undefined
        ) {
          values.primeSurface.reference.standard.standardizers = [];
        }
        if (
          values.primeDirection?.reference?.standard != null &&
          values.primeDirection.reference.standard.standardizers == undefined
        ) {
          values.primeDirection.reference.standard.standardizers = [];
        }
        if (
          values.switchableLayers?.reference?.standard != null &&
          values.switchableLayers.reference.standard.standardizers == undefined
        ) {
          values.switchableLayers.reference.standard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        return updateComponentMutation({
          variables: {
            input: {
              componentId: component.uuid,
              name: values.name,
              abbreviation: values.abbreviation,
              description: values.description,
              availability: {
                from: values.availability?.[0]?.toISOString(),
                to: values.availability?.[1]?.toISOString(),
              },
              categories: values.categories || [],
              primeSurface: values.primeSurface,
              primeDirection: values.primeDirection,
              switchableLayers: values.switchableLayers,
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
        title="Edit Component"
        // onOk={handleOk}
        onCancel={() => {
          setGlobalErrorMessages([]);
          form.resetFields();
          setOpen(false);
        }}
        footer={false}
      >
        <ErrorAlert messages={globalErrorMessages} />
        <Form
          {...layout}
          form={form}
          name="updateComponent"
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
              {
                whitespace: true,
              },
            ]}
            initialValue={component.name}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Abbreviation"
            name="abbreviation"
            initialValue={component.abbreviation}
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
              {
                whitespace: true,
              },
            ]}
            initialValue={component.description}
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Availability"
            name="availability"
            initialValue={[
              component.availability?.from == null
                ? null
                : dayjs(component.availability.from),
              component.availability?.to == null
                ? null
                : dayjs(component.availability.to),
            ]}
          >
            <DatePicker.RangePicker allowEmpty={[true, true]} showTime />
          </Form.Item>
          <Form.Item
            label="Categories"
            name="categories"
            initialValue={component.categories}
          >
            <EnumSelect
              enumObject={ComponentCategory}
              mode="multiple"
              placeholder="Please select"
            />
          </Form.Item>
          <Divider />
          <Form.Item label="Prime Surface">
            <Form.Item
              label="Description"
              name={["primeSurface", "description"]}
              initialValue={component.prime?.surface?.description}
            >
              <Input />
            </Form.Item>
            <ReferenceSubform
              form={form}
              namespace={["primeSurface", "reference"]}
              initialValue={component.prime?.surface?.reference}
            />
          </Form.Item>
          <Form.Item label="Prime Direction">
            <Form.Item
              label="Description"
              name={["primeDirection", "description"]}
              initialValue={component.prime?.direction?.description}
            >
              <Input />
            </Form.Item>
            <ReferenceSubform
              form={form}
              namespace={["primeDirection", "reference"]}
              initialValue={component.prime?.direction?.reference}
            />
          </Form.Item>
          <Form.Item label="Switchable Layers">
            <Form.Item
              label="Description"
              name={["switchableLayers", "description"]}
              initialValue={component.switchableLayers?.description}
            >
              <Input />
            </Form.Item>
            <ReferenceSubform
              form={form}
              namespace={["switchableLayers", "reference"]}
              initialValue={component.switchableLayers?.reference}
            />
          </Form.Item>
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
