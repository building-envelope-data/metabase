import { useMutation } from "@apollo/client/react";
import {
  DatePicker,
  Select,
  Form,
  Input,
  Button,
  Divider,
  App,
  Typography,
} from "antd";
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
import { InstitutionDocument } from "../../queries/institutions.generated";
import { ReferenceForm } from "../ReferenceForm";
import { SelectInstitutionId } from "../SelectInstitutionId";
import ErrorAlert from "../ErrorAlert";
import { layout, tailLayout } from "../../lib/form";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";
import Link from "next/link";
import paths from "../../paths";
import { pluralize, pluralizeIrregular } from "../../lib/array";
import Copyable from "../Copyable";
import Id from "../Id";

type FormValues = {
  name: string;
  abbreviation: string | null | undefined;
  description: string;
  manufacturerId: Scalars["Uuid"]["input"];
  availability:
    | [dayjs.Dayjs | null | undefined, dayjs.Dayjs | null | undefined]
    | null
    | undefined;
  categories: ComponentCategory[] | null | undefined;
  primeSurface: DescriptionOrReferenceInput | null | undefined;
  primeDirection: DescriptionOrReferenceInput | null | undefined;
  switchableLayers: DescriptionOrReferenceInput | null | undefined;
};

interface CreateComponentProps {
  managerId: Scalars["Uuid"]["input"];
  initialManufacturerId: Scalars["Uuid"]["input"];
}

export default function CreateComponent({
  managerId,
  initialManufacturerId,
}: CreateComponentProps) {
  const [globalErrorMessages, setGlobalErrorMessages] = useState(
    new Array<string>(),
  );
  const { notification } = App.useApp();
  const [form] = Form.useForm<FormValues>();

  const [createComponentMutation] = useMutation(CreateComponentDocument, {
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
        query: ComponentsDocument,
      },
    ],
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
              managerId: managerId,
              manufacturerId: values.manufacturerId,
            },
          },
        });
      },
      {
        onSuccess: (data) => {
          const component = data?.createComponent?.component;
          if (component == null) {
            messageMissingModel();
          } else {
            setGlobalErrorMessages([]);
            form.resetFields();
            notification.success({
              title: "Created Component",
              placement: "top",
              showProgress: true,
              pauseOnHover: true,
              description: (
                <>
                  <Typography.Paragraph>
                    <Copyable text={component.uuid}>
                      <Link href={paths.component(component.uuid)}>
                        <Id value={component.uuid} />
                      </Link>{" "}
                    </Copyable>
                  </Typography.Paragraph>
                  {component?.pendingManufacturers != null &&
                    component.pendingManufacturers.totalCount >= 1 && (
                      <Typography.Paragraph>
                        The{" "}
                        {pluralize(
                          component.pendingManufacturers.totalCount,
                          "manufacturer",
                        )}{" "}
                        {component.pendingManufacturers.edges
                          .map((x) => (
                            <Link href={paths.institution(x.node.uuid)}>
                              {x.node.name}
                            </Link>
                          ))
                          .join(", ")}{" "}
                        {pluralizeIrregular(
                          component.pendingManufacturers.totalCount,
                          "is",
                          "are",
                        )}{" "}
                        are waiting for confirmation.
                      </Typography.Paragraph>
                    )}
                </>
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
        <Form.Item
          label="Manufacturer"
          name="manufacturerId"
          rules={[{ required: true }]}
          initialValue={initialManufacturerId}
        >
          <SelectInstitutionId />
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
        <Divider />
        <Form.Item label="Prime Surface" name="primeSurface">
          <Form.Item label="Description" name={["primeSurface", "description"]}>
            <Input />
          </Form.Item>
          <ReferenceForm
            form={form}
            namespace={["primeSurface", "reference"]}
          />
        </Form.Item>
        <Form.Item label="Prime Direction" name="primeDirection">
          <Form.Item
            label="Description"
            name={["primeDirection", "description"]}
          >
            <Input />
          </Form.Item>
          <ReferenceForm
            form={form}
            namespace={["primeDirection", "reference"]}
          />
        </Form.Item>
        <Form.Item label="Switchable Layers" name="switchableLayers">
          <Form.Item
            label="Description"
            name={["switchableLayers", "description"]}
          >
            <Input />
          </Form.Item>
          <ReferenceForm
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
    </>
  );
}
