import { Form, Input, Select, Button } from "antd";
import { MinusCircleOutlined, PlusOutlined } from "@ant-design/icons";

const tailLayout = {
  wrapperCol: { offset: 8, span: 16 },
};

enum Negator {
  Is = "is",
  IsNot = "isNot",
}

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
          {fields.map(({ key, name, fieldKey, ...restField }, index) => (
            <Form.Item key={key} label={index === 0 ? label : " "}>
              <Input.Group>
                <Form.Item
                  {...restField}
                  key={`negator${key}`}
                  name={[name, "negator"]}
                  fieldKey={[fieldKey ?? -1, "negator"]}
                  noStyle
                  initialValue={Negator.Is}
                >
                  <Select style={{ width: "10%" }}>
                    <Select.Option value={Negator.Is}>Is</Select.Option>
                    <Select.Option value={Negator.IsNot}>Is not</Select.Option>
                  </Select>
                </Form.Item>
                <Form.Item
                  {...restField}
                  key={`comparator${key}`}
                  name={[name, "comparator"]}
                  fieldKey={[fieldKey ?? -1, "comparator"]}
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
                  fieldKey={[fieldKey ?? -1, "value"]}
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
                <MinusCircleOutlined
                  style={{ width: "10%" }}
                  onClick={() => remove(name)}
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
              {`Add ${label.toLocaleLowerCase("en")} proposition`}
            </Button>
            <Form.ErrorList errors={errors} />
          </Form.Item>
        </>
      )}
    </Form.List>
  );
}
