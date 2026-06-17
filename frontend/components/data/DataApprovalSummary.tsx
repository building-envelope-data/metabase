import * as graphql from "graphql";
import { Button, Drawer, Typography } from "antd";
import paths from "../../paths";
import {
  DataApprovalPartialFragment,
  DataPartialFragment,
} from "../../queries/data.generated";
import DateTimeX from "../DateTimeX";
import EntityLink from "../entities/EntityLink";
import GnuPgKeyLink from "../GnuPgKeyLink";
import Reference from "../Reference";
import CodeView from "../CodeView";
import JsonView from "../JsonView";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { SignatureOutlined } from "@ant-design/icons";

const nameFallback = (id: Scalars["Uuid"]["output"]) => ({
  uuid: id,
  name: id,
});

export default function DataApprovalSummary({
  approval,
  data,
}: {
  approval: DataApprovalPartialFragment;
  data: DataPartialFragment;
}) {
  const [isProofOpen, setIsProofOpen] = useState(false);

  return (
    <div>
      <Button
        style={{ float: "right" }}
        size="small"
        icon={
          <SignatureOutlined /> /* <AuditOutlined /> <VerifiedOutlined /> */
        }
        onClick={() => setIsProofOpen(true)}
      >
        Show Formal Signature
      </Button>
      The institution{" "}
      <EntityLink
        entity={approval.approver ?? nameFallback(approval.approverId)}
        route={paths.institution}
      />{" "}
      approved on <DateTimeX value={approval.timestamp} /> that the data
      satisfies the statement given by the reference
      <Reference data={approval.statement} />
      <Drawer
        title="Formal Signature"
        placement="right"
        onClose={() => setIsProofOpen(false)}
        size="large"
        open={isProofOpen}
      >
        This is proven by the GnuPG signature
        <CodeView code={approval.signature} style={{ marginTop: "1em" }} />
        generated with the institution's GnuPG key{" "}
        <GnuPgKeyLink fingerprint={approval.keyFingerprint} /> of the message
        <CodeView code={approval.message} style={{ marginTop: "1em" }} />
        The message contains the statement and the response of the query
        <CodeView
          code={graphql.print(graphql.parse(approval.query))}
          style={{ marginTop: "1em" }}
        />
        with the variables
        <JsonView data={approval.variables} style={{ marginTop: "1em" }} />
        against the GraphQL endpoint of the database{" "}
        <EntityLink
          entity={data.database ?? nameFallback(data.databaseId)}
          route={paths.database}
        />
        {data.database?.locator && (
          <>
            {" "}
            located at{" "}
            <Typography.Link href={data.database.locator}>
              {data.database.locator}
            </Typography.Link>
          </>
        )}
        .
      </Drawer>
    </div>
  );
}
