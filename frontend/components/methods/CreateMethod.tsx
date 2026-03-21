import { useMutation } from "@apollo/client/react";
import { DatePicker, Select, Form, Input, Button, Divider } from "antd";
import {
  CreateMethodDocument,
  CreateMethodMutation,
  MethodsDocument,
} from "../../queries/methods.generated";
import {
  MethodCategory,
  Scalars,
  ReferenceInput,
} from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { SelectInstitutionId } from "../SelectInstitutionId";
import { SelectUserId } from "../SelectUserId";
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
  institutionDeveloperIds: Scalars["Uuid"]["input"][] | null | undefined;
  userDeveloperIds: Scalars["Uuid"]["input"][] | null | undefined;
};

export type CreateMethodProps = {
  managerId: Scalars["Uuid"]["input"];
};

export default function CreateMethod({ managerId }: CreateMethodProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const [form] = Form.useForm<FormValues>();

  const [createMethodMutation] = useMutation(CreateMethodDocument, {
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
    useMutationHandler<CreateMethodMutation>({
      getErrors: (data) => data.createMethod.errors,
    });

  const onFinish = (values: FormValues) => {
    withMutationHandler(
      () => {
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (
          values.reference?.standard != null &&
          values.reference.standard.standardizers == undefined
        ) {
          values.reference.standard.standardizers = [];
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
              parameters: [],
              sources: [],
              categories: values.categories || [],
              managerId: managerId,
              institutionDeveloperIds: values.institutionDeveloperIds || [],
              userDeveloperIds: values.userDeveloperIds || [],
            },
          },
        });
      },
      {
        onSuccess: () => {
          form.resetFields();
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
          <Select
            mode="multiple"
            placeholder="Please select"
            options={Object.entries(MethodCategory).map(([_key, value]) => ({
              label: value,
              value: value,
            }))}
          />
        </Form.Item>
        <Form.Item
          label="Institution Developers"
          name="institutionDeveloperIds"
          initialValue={[]}
        >
          <SelectInstitutionId mode="multiple" />
        </Form.Item>
        <Form.Item
          label="User Developers"
          name="userDeveloperIds"
          initialValue={[]}
        >
          <SelectUserId mode="multiple" />
        </Form.Item>
        <Divider />
        <ReferenceForm form={form} namespace={["reference"]} />
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={mutating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
