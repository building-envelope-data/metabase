import { Tag, TagProps } from "antd";

export default function EnumTag(props: TagProps) {
  return (
    <Tag
      {...props}
      style={{
        textTransform: "lowercase",
        fontWeight: "normal",
        ...props.style,
      }}
    >
      {props.children}
    </Tag>
  );
}
