import * as React from "react";
import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";
import {
  DatePicker,
  Select,
  Alert,
  Form,
  Input,
  Button,
  InputNumber,
  Divider,
} from "antd";
import {
  useCreateMethodMutation,
  MethodsDocument,
} from "../../queries/methods.graphql";
import {
  CreatePublicationInput,
  CreateStandardInput,
  MethodCategory,
  Scalars,
  Standardizer,
} from "../../__generated__/__types__";
import { useState } from "react";
import { handleFormErrors } from "../../lib/form";
import {
  InstitutionDocument,
  useInstitutionsQuery,
} from "../../queries/institutions.graphql";
import { useUsersQuery } from "../../queries/users.graphql";
import { notEmpty } from "../../lib/array";
import { SelectMultipleViaSearch } from "../SelectMultipleViaSearch";

const layout = {
  labelCol: { span: 8 },
  wrapperCol: { span: 16 },
};
const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

export type CreateMethodProps = {
  managerId: Scalars["Uuid"];
};

enum Reference {
  None = "None",
  Standard = "Standard",
  Publication = "Publication",
}

export default function CreateMethod({ managerId }: CreateMethodProps) {
  const [createMethodMutation] = useCreateMethodMutation({
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
  const [form] = Form.useForm();
  const [creating, setCreating] = useState(false);

  const [selectedReferenceOption, setSelectedReferenceOption] = useState(
    Reference.None
  );

  // TODO Only fetch `name` and `uuid` because nothing more is needed.
  // TODO Use search instead of drop-down with all users/institutions preloaded. Be inspired by https://ant.design/components/select/#components-select-demo-select-users
  const usersQuery = useUsersQuery();
  const users = usersQuery.data?.users?.nodes;
  const institutionsQuery = useInstitutionsQuery();
  const institutions =
    institutionsQuery.data?.institutions?.nodes?.filter(notEmpty);

  const onFinish = ({
    name,
    description,
    validity,
    availability,
    standard,
    publication,
    calculationLocator,
    categories,
    institutionDeveloperIds,
    userDeveloperIds,
  }: {
    name: string;
    description: string;
    validity:
      | [moment.Moment | null | undefined, moment.Moment | null | undefined]
      | null
      | undefined;
    availability:
      | [moment.Moment | null | undefined, moment.Moment | null | undefined]
      | null
      | undefined;
    standard: CreateStandardInput | undefined;
    publication: CreatePublicationInput | undefined;
    calculationLocator: Scalars["Url"] | undefined;
    categories: MethodCategory[] | undefined;
    institutionDeveloperIds: Scalars["Uuid"][] | undefined;
    userDeveloperIds: Scalars["Uuid"][] | undefined;
  }) => {
    const create = async () => {
      try {
        setCreating(true);
        // TODO Why does `initialValue` not set standardizers to `[]`?
        if (standard != null && standard.standardizers == undefined) {
          standard.standardizers = [];
        }
        // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
        const { errors, data } = await createMethodMutation({
          variables: {
            name: name,
            description: description,
            validity: { from: validity?.[0], to: validity?.[1] },
            availability: { from: availability?.[0], to: availability?.[1] },
            standard: standard,
            publication: publication,
            calculationLocator: calculationLocator,
            categories: categories || [],
            managerId: managerId,
            institutionDeveloperIds: institutionDeveloperIds || [],
            userDeveloperIds: userDeveloperIds || [],
          },
        });
        handleFormErrors(
          errors,
          data?.createMethod?.errors?.map((x) => {
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

  const onReferenceChange = (value: Reference) => {
    switch (value) {
      case Reference.None:
        form.setFieldsValue({ standard: null });
        form.setFieldsValue({ publication: null });
        break;
      case Reference.Publication:
        form.setFieldsValue({ standard: null });
        break;
      case Reference.Standard:
        form.setFieldsValue({ publication: null });
        break;
      default:
        console.error("Impossible!");
    }
    setSelectedReferenceOption(value);
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
          <Select mode="multiple" placeholder="Please select">
            <Select.Option value={MethodCategory.Measurement}>
              Measurement
            </Select.Option>
            <Select.Option value={MethodCategory.Calculation}>
              Calculation
            </Select.Option>
          </Select>
        </Form.Item>
        <Form.Item
          label="Institution Developers"
          name="institutionDeveloperIds"
          initialValue={[]}
        >
          {/* TODO Why can't this component not be used as component but just as method? If we use it as component `*DeveloperIds` is always `[]` when the form is submitted. */}
          {SelectMultipleViaSearch({
            options:
              institutions?.map((institution) => ({
                label: institution.name,
                value: institution.uuid,
              })) || [],
          })}
        </Form.Item>
        <Form.Item
          label="User Developers"
          name="userDeveloperIds"
          initialValue={[]}
        >
          {/* TODO Why can't this component not be used as component but just as method? If we use it as component `*DeveloperIds` is always `[]` when the form is submitted. */}
          {SelectMultipleViaSearch({
            options:
              users?.map((user) => ({
                label: user.name,
                value: user.uuid,
              })) || [],
          })}
        </Form.Item>
        <Divider />
        <Form.Item
          label="Reference"
          name="reference"
          initialValue={Reference.None}
        >
          <Select
            options={[
              { label: "None", value: Reference.None },
              { label: "Standard", value: Reference.Standard },
              { label: "Publication", value: Reference.Publication },
            ]}
            onChange={onReferenceChange}
          />
        </Form.Item>
        {selectedReferenceOption === Reference.Publication && (
          <>
            <Form.Item label="Title" name={["publication", "title"]}>
              <Input />
            </Form.Item>
            <Form.Item label="Abstract" name={["publication", "abstract"]}>
              <Input />
            </Form.Item>
            <Form.Item label="Section" name={["publication", "section"]}>
              <Input />
            </Form.Item>
            <Form.Item label="arXiv" name={["publication", "arXiv"]}>
              <Input />
            </Form.Item>
            <Form.Item label="DOI" name={["publication", "doi"]}>
              <Input />
            </Form.Item>
            <Form.Item label="URN" name={["publication", "urn"]}>
              <Input />
            </Form.Item>
            <Form.Item
              label="WebAddress"
              name={["publication", "webAddress"]}
              rules={[
                {
                  type: "url",
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.List name={["publication", "authors"]}>
              {(fields, { add, remove }, { errors }) => (
                <>
                  {fields.map((field, index) => (
                    <Form.Item
                      key={field.key}
                      label={index === 0 ? "Authors" : " "}
                    >
                      <Input.Group>
                        <Form.Item {...field} noStyle>
                          <Input style={{ width: "90%" }} />
                        </Form.Item>
                        <MinusCircleOutlined
                          style={{ width: "10%" }}
                          onClick={() => remove(field.name)}
                        />
                      </Input.Group>
                    </Form.Item>
                  ))}
                  <Form.Item {...tailLayout}>
                    <Button
                      type="dashed"
                      onClick={() => add()}
                      style={{ width: "100%" }}
                      icon={<PlusOutlined />}
                    >
                      Add author
                    </Button>
                    <Form.ErrorList errors={errors} />
                  </Form.Item>
                </>
              )}
            </Form.List>
          </>
        )}
        {selectedReferenceOption === Reference.Standard && (
          <>
            <Form.Item label="Title" name={["standard", "title"]}>
              <Input />
            </Form.Item>
            <Form.Item label="Abstract" name={["standard", "abstract"]}>
              <Input />
            </Form.Item>
            <Form.Item label="Section" name={["standard", "section"]}>
              <Input />
            </Form.Item>
            <Form.Item label="Numeration">
              <Input.Group>
                <Form.Item
                  noStyle
                  name={["standard", "numeration", "mainNumber"]}
                  rules={[
                    {
                      required: true,
                    },
                  ]}
                >
                  <Input placeholder="Main Number" />
                </Form.Item>
                <Form.Item noStyle name={["standard", "numeration", "prefix"]}>
                  <Input placeholder="Prefix" />
                </Form.Item>
                <Form.Item noStyle name={["standard", "numeration", "suffix"]}>
                  <Input placeholder="Suffix" />
                </Form.Item>
              </Input.Group>
            </Form.Item>
            <Form.Item label="Year" name={["standard", "year"]}>
              <InputNumber />
            </Form.Item>
            <Form.Item
              label="Locator"
              name={["standard", "locator"]}
              rules={[
                {
                  type: "url",
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Standardizers"
              name={["standard", "standardizers"]}
              initialValue={[]}
            >
              <Select
                mode="multiple"
                placeholder="Please select"
                options={Object.entries(Standardizer).map(([_key, value]) => ({
                  label: value,
                  value: value,
                }))}
              />
            </Form.Item>
          </>
        )}
        <Form.Item {...tailLayout}>
          <Button type="primary" htmlType="submit" loading={creating}>
            Create
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}
