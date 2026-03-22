import { Form, Input, Space, Select, Button } from "antd";
import { PlusOutlined } from "@ant-design/icons";

const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

enum Negator {
  Is = "is",
  IsNot = "isNot",
}

const negatorOptions = [
  { value: Negator.Is, label: "Is" },
  { value: Negator.IsNot, label: "Is" },
];

export enum UuidPropositionComparator {
  EqualTo = "equalTo",
}

export type UuidPropositionFormListProps = {
  name: string;
  label: string;
};

export function UuidPropositionFormList({
  name,
  label,
}: UuidPropositionFormListProps) {
  return (
    <Form.List name={name}>
      {(fields, { add, remove }, { errors }) => (
        <>
          {fields.map(({ key, name, ...restField }, index) => (
            <Form.Item key={key} label={index === 0 ? label : " "}>
              <Space.Compact block>
                <Form.Item
                  {...restField}
                  key={`negator${key}`}
                  name={[name, "negator"]}
                  noStyle
                  initialValue={Negator.Is}
                >
                  <Select
                    style={{ width: "10%" }}
                    defaultValue={Negator.Is}
                    options={negatorOptions}
                  />
                </Form.Item>
                <Form.Item
                  {...restField}
                  key={`comparator${key}`}
                  name={[name, "comparator"]}
                  noStyle
                  initialValue={UuidPropositionComparator.EqualTo}
                >
                  <Select
                    style={{ width: "20%" }}
                    options={[
                      {
                        label: "equal to",
                        value: UuidPropositionComparator.EqualTo,
                      },
                    ]}
                  />
                </Form.Item>
                <Form.Item
                  {...restField}
                  key={`value${key}`}
                  name={[name, "value"]}
                  noStyle
                >
                  <Input
                    style={{
                      float: "none",
                      display: "inline-block",
                      width: "60%",
                    }}
                  />
                </Form.Item>
                <Button danger onClick={() => remove(name)}>
                  Remove
                </Button>
              </Space.Compact>
            </Form.Item>
          ))}
          <Form.Item {...tailLayout}>
            <Button
              type="dashed"
              onClick={() => add()}
              style={{ width: "100%" }}
              icon={<PlusOutlined />}
            >
              {`Add ${label.toLocaleLowerCase("en")} proposition`}
            </Button>
            <Form.ErrorList errors={errors} />
          </Form.Item>
        </>
      )}
    </Form.List>
  );
}
