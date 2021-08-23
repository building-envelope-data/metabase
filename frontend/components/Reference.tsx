import { Descriptions, Typography } from "antd";
import {
  Maybe,
  Numeration,
  Publication,
  Standard,
} from "../__generated__/__types__";

export type ReferenceProps = {
  reference:
    | Maybe<
        | ({ __typename?: "Publication" | undefined } & Pick<
            Publication,
            | "title"
            | "arXiv"
            | "authors"
            | "doi"
            | "urn"
            | "webAddress"
            | "abstract"
            | "section"
          >)
        | ({ __typename?: "Standard" | undefined } & Pick<
            Standard,
            | "title"
            | "abstract"
            | "section"
            | "locator"
            | "standardizers"
            | "year"
          > & {
              numeration: { __typename?: "Numeration" | undefined } & Pick<
                Numeration,
                "mainNumber" | "prefix" | "suffix"
              >;
            })
      >
    | undefined;
};

export function Reference({ reference }: ReferenceProps) {
  return reference == null ? (
    <Typography.Text>None</Typography.Text>
  ) : (
    <Descriptions column={1}>
      <>
        <Descriptions.Item label="Title">{reference?.title}</Descriptions.Item>
        <Descriptions.Item label="Abstract">
          {reference?.abstract}
        </Descriptions.Item>
        <Descriptions.Item label="Section">
          {reference?.section}
        </Descriptions.Item>
      </>
      {reference.__typename === "Standard" && (
        <>
          <Descriptions.Item label="Numeration">{`${reference.numeration.prefix} ${reference.numeration.mainNumber} ${reference.numeration.suffix}`}</Descriptions.Item>
          <Descriptions.Item label="Year">{reference.year}</Descriptions.Item>
          <Descriptions.Item label="Locator">
            <Typography.Link href={reference.locator}>
              {reference.locator}
            </Typography.Link>
          </Descriptions.Item>
          <Descriptions.Item label="Standardizers">
            {reference.standardizers.join(", ")}
          </Descriptions.Item>
        </>
      )}
      {reference.__typename === "Publication" && (
        <>
          <Descriptions.Item label="arXiv">{reference.arXiv}</Descriptions.Item>
          <Descriptions.Item label="DOI">{reference.doi}</Descriptions.Item>
          <Descriptions.Item label="URN">{reference.urn}</Descriptions.Item>
          <Descriptions.Item label="Web Address">
            {reference.webAddress}
          </Descriptions.Item>
          <Descriptions.Item label="Authors">
            {reference.authors?.join(", ")}
          </Descriptions.Item>
        </>
      )}
    </Descriptions>
  );
}
