import { useMutation } from "@apollo/client/react";
import { DatePicker, Form, Input, Button, Divider, App, Modal } from "antd";
import {
  CreateComponentDocument,
  ComponentsDocument,
  CreateComponentMutation,
} from "../../queries/components.generated";
import {
  ComponentCategory,
  DescriptionOrReferenceInput,
  Scalars,
} from "../../__generated__/graphql";
import { useState } from "react";
import dayjs from "dayjs";
import InstitutionIdSelect from "../institutions/InstitutionIdSelect";
import ReferenceSubform from "../ReferenceSubform";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import NewButton from "../NewButton";
import ComponentSummary from "./ComponentSummary";
import RepresentedInstitutionIdSelect from "../institutions/RepresentedInstitutionIdSelect";
import EnumSelect from "../EnumSelect";
import { createPaginatedIdSelectOption } from "../PaginatedIdSelect";

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
  manufacturerId: { value: Scalars["Uuid"]["input"]; label: string };
  managerId: { value: Scalars["Uuid"]["input"]; label: string };
};

interface CreateComponentProps {
  initialManager: { uuid: Scalars["Uuid"]["input"]; name: string };
  initialManufacturer: { uuid: Scalars["Uuid"]["input"]; name: string };
}

export default function CreateComponent({
  initialManager,
  initialManufacturer,
}: CreateComponentProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const { notification } = App.useApp();
  const [form] = Form.useForm<FormValues>();

  const [createComponentMutation] = useMutation(CreateComponentDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [ComponentsDocument],
  });

  const {
    mutating,
    withMutationHandler,
    augmentFormWithErrors,
    messageMissingModel,
  } = useMutationHandler<CreateComponentMutation>({
    getErrors: (data) => data.createComponent.errors,
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
        return createComponentMutation({
          variables: {
            input: {
              name: values.name,
              abbreviation: values.abbreviation,
              description: values.description,
              availability: {
                from: values.availability?.[0],
                to: values.availability?.[1],
              },
              categories: values.categories || [],
              primeSurface: values.primeSurface,
              primeDirection: values.primeDirection,
              switchableLayers: values.switchableLayers,
              managerId: values.managerId.value,
              manufacturerId: values.manufacturerId.value,
            },
          },
        });
      },
      {
        onSuccess: (data) => {
          const model = data?.createComponent?.component;
          if (model == null) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Created Component",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              description: (
                <ComponentSummary hideInputControls entity={model} />
              ),
            });
          }
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
      <NewButton onClick={() => setOpen(true)}>Component</NewButton>
      <Modal
        open={open}
        title="New Component"
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
            <EnumSelect
              enumObject={ComponentCategory}
              mode="multiple"
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item
            label="Manufacturer"
            name="manufacturerId"
            rules={[{ required: true }]}
            initialValue={createPaginatedIdSelectOption(initialManufacturer)}
          >
            <InstitutionIdSelect labelInValue />
          </Form.Item>
          <Form.Item
            label="Manager"
            name="managerId"
            rules={[{ required: true }]}
            initialValue={createPaginatedIdSelectOption(initialManager)}
          >
            <RepresentedInstitutionIdSelect labelInValue />
          </Form.Item>
          <Divider />
          <Form.Item label="Prime Surface">
            <Form.Item
              label="Description"
              name={["primeSurface", "description"]}
            >
              <Input />
            </Form.Item>
            <ReferenceSubform
              form={form}
              namespace={["primeSurface", "reference"]}
            />
          </Form.Item>
          <Form.Item label="Prime Direction">
            <Form.Item
              label="Description"
              name={["primeDirection", "description"]}
            >
              <Input />
            </Form.Item>
            <ReferenceSubform
              form={form}
              namespace={["primeDirection", "reference"]}
            />
          </Form.Item>
          <Form.Item label="Switchable Layers">
            <Form.Item
              label="Description"
              name={["switchableLayers", "description"]}
            >
              <Input />
            </Form.Item>
            <ReferenceSubform
              form={form}
              namespace={["switchableLayers", "reference"]}
            />
          </Form.Item>
          <Form.Item {...tailLayout}>
            <Button type="primary" htmlType="submit" loading={mutating}>
              Create
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
