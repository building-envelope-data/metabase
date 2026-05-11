import { useMutation } from "@apollo/client/react";
import { DatePicker, Form, Input, Button, Divider, App, Modal } from "antd";
import {
  CreateMethodDocument,
  CreateMethodMutation,
  MethodsDocument,
} from "../../queries/methods.generated";
import {
  MethodCategory,
  Scalars,
  ReferenceInput,
  MethodParameterInput,
  MethodSourceInput,
} from "../../__generated__/graphql";
import { useState } from "react";
import InstitutionIdSelect from "../institutions/InstitutionIdSelect";
import UserIdSelect from "../users/UserIdSelect";
import ReferenceSubform from "../ReferenceSubform";
import dayjs from "dayjs";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import ErrorAlert from "../ErrorAlert";
import MethodParametersSubform from "./MethodParametersSubform";
import MethodSummary from "./MethodSummary";
import NewButton from "../NewButton";
import RepresentedInstitutionIdSelect from "../institutions/RepresentedInstitutionIdSelect";
import EnumSelect from "../EnumSelect";

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
  parameters: MethodParameterInput[] | null | undefined;
  sources: MethodSourceInput[] | null | undefined;
  categories: MethodCategory[] | null | undefined;
  institutionDeveloperIds: Scalars["Uuid"]["input"][] | null | undefined;
  userDeveloperIds: Scalars["Uuid"]["input"][] | null | undefined;
  managerId: Scalars["Uuid"]["input"];
};

interface CreateMethodProps {
  initialManagerId: Scalars["Uuid"]["input"];
  initialInstitutionDeveloperIds?: Scalars["Uuid"]["input"][];
  initialUserDeveloperIds?: Scalars["Uuid"]["input"][];
}

export default function CreateMethod({
  initialManagerId,
  initialInstitutionDeveloperIds = [],
  initialUserDeveloperIds = [],
}: CreateMethodProps) {
  const [open, setOpen] = useState(false);
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();
  const { notification } = App.useApp();

  const [createMethodMutation] = useMutation(CreateMethodDocument, {
    // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-updates
    // See https://www.apollographql.com/docs/react/data/mutations/#options
    refetchQueries: [MethodsDocument],
  });

  const {
    mutating,
    withMutationHandler,
    augmentFormWithErrors,
    messageMissingModel,
  } = useMutationHandler<CreateMethodMutation>({
    getErrors: (data) => data.createMethod.errors,
  });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set sources, parameters, and standardizers to `[]`?
        if (
          values.reference?.standard != null &&
          values.reference.standard.standardizers == undefined
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
        return createMethodMutation({
          variables: {
            input: {
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
              parameters: values.parameters,
              sources: values.sources,
              categories: values.categories || [],
              managerId: values.managerId,
              institutionDeveloperIds: values.institutionDeveloperIds || [],
              userDeveloperIds: values.userDeveloperIds || [],
            },
          },
        });
      },
      {
        onSuccess: (data) => {
          const model = data?.createMethod?.method;
          if (!model) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            setOpen(false);
            notification.success({
              title: "Created Method",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              duration: 0,
              style: {
                width: "max-content",
                minWidth: "384px",
              },
              description: <MethodSummary hideInputControls entity={model} />,
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
      <NewButton onClick={() => setOpen(true)}>Method</NewButton>
      <Modal
        open={open}
        title="New Method"
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
            <EnumSelect
              enumObject={MethodCategory}
              mode="multiple"
              placeholder="Please select"
            />
          </Form.Item>
          <Form.Item label="Parameter(s)">
            <MethodParametersSubform namespace={["parameters"]} />
          </Form.Item>
          <Form.Item
            label="Institution Developers"
            name="institutionDeveloperIds"
            initialValue={initialInstitutionDeveloperIds}
          >
            <InstitutionIdSelect mode="multiple" />
          </Form.Item>
          <Form.Item
            label="User Developers"
            name="userDeveloperIds"
            initialValue={initialUserDeveloperIds}
          >
            <UserIdSelect mode="multiple" />
          </Form.Item>
          <Form.Item
            label="Manager"
            name="managerId"
            rules={[{ required: true }]}
            initialValue={initialManagerId}
          >
            <RepresentedInstitutionIdSelect />
          </Form.Item>
          <Divider />
          <ReferenceSubform form={form} namespace={["reference"]} />
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
