import { Scalars } from "../../__generated__/graphql";
import { GnuPgKeyFingerprintDocument } from "../../queries/gnuPgKeyFingerprints.generated";
import { Skeleton, Result, Card, Button } from "antd";
import { useQuery } from "@apollo/client/react";
import { useQueryHandler } from "../../lib/hooks/useQueryHandler";
import GnuPgKeySummary from "./GnuPgKeySummary";
import QueryToolbar from "../QueryToolbar";

interface GnuPgKeyProps {
  fingerprint: Scalars["String"]["output"];
}

export default function GnuPgKey({ fingerprint }: GnuPgKeyProps) {
  const queryVariables = {
    fingerprint: fingerprint,
  };
  const { loading, error, data, refetch } = useQuery(
    GnuPgKeyFingerprintDocument,
    {
      variables: queryVariables,
    },
  );
  useQueryHandler({ error });
  const gnuPgKey = data?.gnuPgKeyFingerprint;

  if (loading) {
    return <Skeleton active avatar title />;
  }

  if (!gnuPgKey) {
    return (
      <Result
        status="500"
        title="500"
        subTitle="Sorry, something went wrong."
        extra={
          <Button loading={loading} onClick={() => refetch()}>
            Reload
          </Button>
        }
      />
    );
  }

  return (
    <div>
      <Card style={{ marginBottom: "1em" }}>
        <GnuPgKeySummary entity={gnuPgKey} />
      </Card>
      <QueryToolbar
        query={GnuPgKeyFingerprintDocument}
        variables={queryVariables}
      />
    </div>
  );
}
