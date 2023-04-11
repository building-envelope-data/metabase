import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";
import { InputNumber, Select, Form, Input, Button, FormInstance } from "antd";
import { useState } from "react";
import {
  Standardizer,
  Standard,
  Publication,
} from "../__generated__/__types__";

const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

enum ReferenceKind {
  None = "None",
  Standard = "Standard",
  Publication = "Publication",
}

function referenceToKind(
  reference: Publication | Standard | null | undefined
): ReferenceKind {
  switch (reference?.__typename) {
    case null:
      return ReferenceKind.None;
    case "Standard":
      return ReferenceKind.Standard;
    case "Publication":
      return ReferenceKind.Publication;
    default:
      // TODO Why does this not work? For a working example see https://www.typescriptlang.org/docs/handbook/2/narrowing.html#exhaustiveness-checking
      // const _exhaustiveCheck: never = reference;
      // return _exhaustiveCheck;
      return ReferenceKind.None;
  }
}

// interface HasStandardAndPublication {
//   standard: CreateStandardInput | null | undefined;
//   publication: CreatePublicationInput | null | undefined;
// }

export type ReferenceFormProps<Values> = {
  form: FormInstance<Values>;
  initialValue?: Standard | Publication | null;
};

// TODO Why does the following not work? export function ReferenceForm<Values extends HasStandardAndPublication>({form}: ReferenceFormProps<Values>) {
export function ReferenceForm({ form, initialValue }: ReferenceFormProps<any>) {
  const initialKind = referenceToKind(initialValue);
  const [selectedReferenceOption, setSelectedReferenceOption] =
    useState(initialKind);
  if (initialValue?.__typename == "Publication") {
    form.setFieldsValue({ publication: initialValue });
  }
  if (initialValue?.__typename == "Standard") {
    form.setFieldsValue({ standard: initialValue });
  }

  const onReferenceChange = (value: ReferenceKind) => {
    switch (value) {
      case ReferenceKind.None:
        form.setFieldsValue({ standard: null });
        form.setFieldsValue({ publication: null });
        break;
      case ReferenceKind.Publication:
        form.setFieldsValue({ standard: null });
        if (initialValue?.__typename == "Publication") {
          form.setFieldsValue({ publication: initialValue });
        }
        break;
      case ReferenceKind.Standard:
        form.setFieldsValue({ publication: null });
        if (initialValue?.__typename == "Standard") {
          form.setFieldsValue({ standard: initialValue });
        }
        break;
      default:
        console.error("Impossible!");
    }
    setSelectedReferenceOption(value);
  };

  return (
    <>
      <Form.Item label="Reference" name="reference" initialValue={initialKind}>
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
      {selectedReferenceOption === ReferenceKind.Standard && (
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
          <Form.Item label="Standardizers" name={["standard", "standardizers"]}>
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
    </>
  );
}
