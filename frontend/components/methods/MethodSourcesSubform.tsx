import { PlusOutlined } from "@ant-design/icons";
import { Form, Input, Button, Flex } from "antd";
import DeleteButton from "../DeleteButton";
import { MethodSource } from "../../__generated__/graphql";
import TextArea from "antd/es/input/TextArea";

export type MethodSourcesSubformProps = {
  initialValue?: MethodSource[] | null;
  namespace: string[];
};

export default function MethodSourcesSubform({
  initialValue,
  namespace,
}: MethodSourcesSubformProps) {
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
                    {
                      whitespace: true,
                    },
                  ]}
                >
                  <Input placeholder="Name" style={{ width: "100%" }} />
                </Form.Item>
                <Form.Item
                  {...restField}
                  name={[name, "description"]}
                  noStyle
                  style={{ flex: 1 }}
                  rules={[
                    {
                      required: true,
                    },
                  ]}
                >
                  <TextArea
                    autoSize={{ minRows: 2 }}
                    placeholder="Description"
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
              Add Source
            </Button>
          </Flex>
        )}
      </Form.List>
    </>
  );
}
