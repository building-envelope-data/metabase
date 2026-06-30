import { Space, Typography } from "antd";
import {
  GlobalOutlined,
  BookOutlined,
  SafetyCertificateOutlined,
} from "@ant-design/icons";
import { Iconize } from "./Iconize";
import { intersperse, isTruthy } from "../lib/array";
import IdentifierItem from "./IdentifierItem";
import { ReferencePartialFragment } from "../queries/common.generated";

interface ReferenceProps {
  data: ReferencePartialFragment;
}

export default function Reference({ data }: ReferenceProps) {
  const Icon =
    data.__typename === "Standard" ? SafetyCertificateOutlined : BookOutlined;

  return (
    <div>
      <Iconize icon={<Icon />}>{data.__typename}</Iconize>{" "}
      <span>
        {data.__typename === "Publication" &&
          intersperse(
            [
              data.authors && data.authors.length > 0 && (
                <Typography.Text strong key="authors">
                  {data.authors.join(", ")}
                </Typography.Text>
              ),
              data.title && (
                <Typography.Text key="title" italic style={{ marginLeft: 4 }}>
                  {data.title},
                </Typography.Text>
              ),
              data.section && (
                <span key="section">
                  Section{" "}
                  <Typography.Text italic>{data.section}</Typography.Text>.
                </span>
              ),
              (data.arXiv || data.doi || data.urn || data.webAddress) && (
                <Space wrap size="small" key="identifier">
                  {(["arXiv", "doi", "urn", "webAddress"] as const)
                    .map(
                      (key) =>
                        data[key] && (
                          <IdentifierItem
                            key={key}
                            type={key}
                            value={data[key]}
                          />
                        ),
                    )
                    .filter(isTruthy)}
                </Space>
              ),
            ].filter(isTruthy),
          )}
        {data.__typename === "Standard" &&
          intersperse(
            [
              data.standardizers && data.standardizers.length > 0 && (
                <Typography.Text strong key="standardizers">
                  {data.standardizers?.join(", ")}
                </Typography.Text>
              ),
              <span key="numeration">
                {data.numeration.prefix ?? ""}
                {data.numeration.mainNumber}
                {data.numeration.suffix ? `-${data.numeration.suffix}` : ""}
              </span>,
              data.year && <span key="year">({data.year}).</span>,
              data.title && (
                <Typography.Text italic key="title">
                  {data.title}.
                </Typography.Text>
              ),
              data.section && (
                <span key="section">
                  Section{" "}
                  <Typography.Text italic>{data.section}</Typography.Text>.
                </span>
              ),
              data.locator && (
                <Typography.Link
                  href={data.locator}
                  target="_blank"
                  key="locator"
                >
                  <Iconize icon={<GlobalOutlined />}>Web</Iconize>
                </Typography.Link>
              ),
            ].filter(isTruthy),
          )}
      </span>
      {data.abstract && (
        <Typography.Paragraph
          type="secondary"
          ellipsis={{ rows: 3, expandable: true, symbol: "more" }}
        >
          {data.abstract}
        </Typography.Paragraph>
      )}
    </div>
  );
}
