import { Typography } from "antd";
import { DescriptionOrReferencePartialFragment } from "../queries/common.generated";
import Reference from "./Reference";

interface ReferenceProps {
  title: string;
  data: DescriptionOrReferencePartialFragment;
}

export default function DescriptionOrReference({
  title,
  data,
}: ReferenceProps) {
  if (!data.description && !data.reference) {
    return null;
  }
  return (
    <div>
      <Typography.Title level={5}>{title}</Typography.Title>
      {data.description && (
        <Typography.Paragraph type={data.reference ? "secondary" : undefined}>
          {data.description}
        </Typography.Paragraph>
      )}
      {data.reference && <Reference data={data.reference} />}
    </div>
  );
}
