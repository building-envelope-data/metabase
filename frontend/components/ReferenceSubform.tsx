import { PlusOutlined } from "@ant-design/icons";
import {
  InputNumber,
  Select,
  Form,
  Input,
  Space,
  Button,
  FormInstance,
  Flex,
} from "antd";
import { useState } from "react";
import { Standardizer, Standard, Publication } from "../__generated__/graphql";
import DeleteButton from "./DeleteButton";
import TextArea from "antd/es/input/TextArea";
import EnumSelect from "./EnumSelect";

enum ReferenceKind {
  None = "None",
  Standard = "Standard",
  Publication = "Publication",
}

function referenceToKind(
  reference: Publication | Standard | null | undefined,
): ReferenceKind {
  switch (reference?.__typename) {
    case null:
      return ReferenceKind.None;
    case undefined:
      return ReferenceKind.None;
    case "Standard":
      return ReferenceKind.Standard;
    case "Publication":
      return ReferenceKind.Publication;
    default:
      return assertNever(reference);
  }
}

function removeTypenames(
  reference: Publication | Standard | null | undefined,
): Publication | Standard | null {
  if (reference == null) {
    return null;
  }
  const { __typename, ...referenceWithoutTypename } = reference;
  if ("numeration" in referenceWithoutTypename) {
    const { __typename, ...numerationWithoutTypename } =
      referenceWithoutTypename.numeration;
    referenceWithoutTypename.numeration = numerationWithoutTypename;
  }
  return referenceWithoutTypename;
}

export type ReferenceSubformProps<Values> = {
  form: FormInstance<Values>;
  initialValue?: Standard | Publication | null;
  namespace: string[];
};

// TODO Harden types: export default function ReferenceForm<Values extends ReferenceInput>({form}: ReferenceFormProps<Values>) {
export default function ReferenceSubform({
  form,
  initialValue,
  namespace,
}: ReferenceSubformProps<any>) {
  const initialKind = referenceToKind(initialValue);
  const initialReference = removeTypenames(initialValue);
  const [selectedReferenceOption, setSelectedReferenceOption] =
    useState(initialKind);

  const onReferenceChange = (value: ReferenceKind) => {
    if (value != selectedReferenceOption) {
      switch (value) {
        case ReferenceKind.None:
          form.setFieldValue(namespace.concat("publication"), null);
          form.setFieldValue(namespace.concat("standard"), null);
          break;
        case ReferenceKind.Publication:
          form.setFieldValue(namespace.concat("standard"), null);
          if (initialKind == ReferenceKind.Publication) {
            form.setFieldValue(
              namespace.concat("publication"),
              initialReference,
            );
          }
          break;
        case ReferenceKind.Standard:
          form.setFieldValue(namespace.concat("publication"), null);
          if (initialKind == ReferenceKind.Standard) {
            form.setFieldValue(namespace.concat("standard"), initialReference);
          }
          break;
        default:
          console.error("Impossible!");
      }
      setSelectedReferenceOption(value);
    }
  };

  return (
    <>
      <Form.Item
        label="Reference"
        name={["unmapped"].concat(namespace)}
        initialValue={initialKind}
      >
        <Select
          options={[
            { label: "None", value: ReferenceKind.None },
            { label: "Standard", value: ReferenceKind.Standard },
            { label: "Publication", value: ReferenceKind.Publication },
          ]}
          onChange={onReferenceChange}
        />
      </Form.Item>
      {selectedReferenceOption === ReferenceKind.Publication && (
        <>
          <Form.Item
            label="Title"
            name={namespace.concat("publication", "title")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.title
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Abstract"
            name={namespace.concat("publication", "abstract")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.abstract
                : null
            }
          >
            <TextArea autoSize={{ minRows: 2 }} />
          </Form.Item>
          <Form.Item
            label="Section"
            name={namespace.concat("publication", "section")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.section
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="arXiv"
            name={namespace.concat("publication", "arXiv")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.arXiv
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="DOI"
            name={namespace.concat("publication", "doi")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.doi
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="URN"
            name={namespace.concat("publication", "urn")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.urn
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Web Address"
            name={namespace.concat("publication", "webAddress")}
            initialValue={
              initialValue?.__typename == "Publication"
                ? initialValue.webAddress
                : null
            }
            rules={[
              {
                type: "url",
              },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item label="Author(s)">
            <Form.List
              name={namespace.concat("publication", "authors")}
              initialValue={
                initialValue?.__typename == "Publication"
                  ? initialValue.authors == null
                    ? undefined
                    : initialValue.authors
                  : undefined
              }
            >
              {(fields, { add, remove }, { errors }) => (
                <Flex vertical gap="small">
                  <Form.ErrorList errors={errors} />
                  {fields.map(({ key, name, ...restField }) => (
                    <Flex key={key} gap="small" align="baseline">
                      <Form.Item
                        {...restField}
                        name={name}
                        noStyle
                        style={{ flex: 1 }}
                        rules={[
                          {
                            required: true,
                          },
                        ]}
                      >
                        <Input style={{ width: "100%" }} />
                      </Form.Item>
                      <DeleteButton
                        type="icon"
                        kind="remove"
                        onClick={() => remove(name)}
                      />
                    </Flex>
                  ))}
                  <Button
                    block
                    type="default"
                    onClick={() => add(null)}
                    icon={<PlusOutlined />}
                  >
                    Add Author
                  </Button>
                </Flex>
              )}
            </Form.List>
          </Form.Item>
        </>
      )}
      {selectedReferenceOption === ReferenceKind.Standard && (
        <>
          <Form.Item
            label="Title"
            name={namespace.concat("standard", "title")}
            initialValue={
              initialValue?.__typename == "Standard" ? initialValue.title : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Abstract"
            name={namespace.concat("standard", "abstract")}
            initialValue={
              initialValue?.__typename == "Standard"
                ? initialValue.abstract
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item
            label="Section"
            name={namespace.concat("standard", "section")}
            initialValue={
              initialValue?.__typename == "Standard"
                ? initialValue.section
                : null
            }
          >
            <Input />
          </Form.Item>
          <Form.Item label="Numeration">
            <Space>
              <Form.Item
                noStyle
                name={namespace.concat("standard", "numeration", "mainNumber")}
                initialValue={
                  initialValue?.__typename == "Standard"
                    ? initialValue.numeration.mainNumber
                    : null
                }
                rules={[
                  {
                    required: true,
                  },
                ]}
              >
                <Input placeholder="Main Number" />
              </Form.Item>
              <Form.Item
                noStyle
                name={namespace.concat("standard", "numeration", "prefix")}
                initialValue={
                  initialValue?.__typename == "Standard"
                    ? initialValue.numeration.prefix
                    : null
                }
              >
                <Input placeholder="Prefix" />
              </Form.Item>
              <Form.Item
                noStyle
                name={namespace.concat("standard", "numeration", "suffix")}
                initialValue={
                  initialValue?.__typename == "Standard"
                    ? initialValue.numeration.suffix
                    : null
                }
              >
                <Input placeholder="Suffix" />
              </Form.Item>
            </Space>
          </Form.Item>
          <Form.Item
            label="Year"
            name={namespace.concat("standard", "year")}
            initialValue={
              initialValue?.__typename == "Standard" ? initialValue.year : null
            }
          >
            <InputNumber />
          </Form.Item>
          <Form.Item
            label="Locator"
            name={namespace.concat("standard", "locator")}
            initialValue={
              initialValue?.__typename == "Standard"
                ? initialValue.locator
                : null
            }
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
            name={namespace.concat("standard", "standardizers")}
            initialValue={
              initialValue?.__typename == "Standard"
                ? initialValue.standardizers
                : null
            }
          >
            <EnumSelect
              enumObject={Standardizer}
              mode="multiple"
              placeholder="Please select"
            />
          </Form.Item>
        </>
      )}
    </>
  );
}
