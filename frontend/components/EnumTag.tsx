import { Tag, TagProps } from "antd";
import { humanize } from "../lib/string";

export default function EnumTag(props: TagProps) {
  return (
    <Tag
      {...props}
      style={{
        fontWeight: "normal",
        ...props.style,
      }}
    >
      {props.children && humanize(props.children.toString(), "none-upper")}
    </Tag>
  );
}
