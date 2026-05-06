import { PlusOutlined } from "@ant-design/icons";
import { Form, Input, Button, Flex } from "antd";
import DeleteButton from "../DeleteButton";
import { MethodParameter } from "../../__generated__/graphql";
import TextArea from "antd/es/input/TextArea";

export type MethodParametersSubformProps = {
  initialValue?: MethodParameter[] | null;
  namespace: string[];
};

export function MethodParametersSubform({
  initialValue,
  namespace,
}: MethodParametersSubformProps) {
  return (
    <>
      <Form.List name={namespace} initialValue={initialValue ?? undefined}>
        {(fields, { add, remove }, { errors }) => (
          <Flex vertical gap="small">
            <Form.ErrorList errors={errors} />
            {fields.map(({ key, name, ...restField }) => (
              <Flex vertical key={key} gap="small" align="baseline">
                <Form.Item
                  {...restField}
                  name={[name, "name"]}
                  noStyle
                  style={{ flex: 1 }}
                  rules={[
                    {
                      required: true,
                    },
                  ]}
                >
                  <Input placeholder="Name" style={{ width: "100%" }} />
                </Form.Item>
                <Form.Item
                  {...restField}
                  name={[name, "type"]}
                  noStyle
                  style={{ flex: 1 }}
                  // Convert JSON Object (State) -> String (UI)
                  getValueProps={(value: object | null | undefined) => {
                    return {
                      value:
                        typeof value === "object"
                          ? JSON.stringify(value, null, 2)
                          : value,
                    };
                  }}
                  // Convert String (UI) -> JSON Object (State)
                  getValueFromEvent={(e) => {
                    const rawValue = e.target.value.trim();
                    if (rawValue === "true") return true;
                    if (rawValue === "false") return false;
                    try {
                      return JSON.parse(rawValue);
                    } catch (err) {
                      return rawValue;
                    }
                  }}
                  rules={[
                    {
                      required: true,
                    },
                    {
                      validator: (_, value) => {
                        // Let the "required" rule handle it
                        if (value === undefined || value === null)
                          return Promise.resolve();
                        if (typeof value === "string") {
                          return Promise.reject(
                            new Error("Invalid JSON Schema"),
                          );
                        }
                        return Promise.resolve();
                      },
                    },
                  ]}
                >
                  <TextArea
                    autoSize={{ minRows: 2 }}
                    placeholder="Type (JSON Schema)"
                    style={{ width: "100%" }}
                  />
                </Form.Item>
                <DeleteButton
                  type="default"
                  kind="remove"
                  style={{ width: "100%" }}
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
              Add Parameter
            </Button>
          </Flex>
        )}
      </Form.List>
    </>
  );
}
