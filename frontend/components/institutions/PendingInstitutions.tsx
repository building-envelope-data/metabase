import { useMutation } from "@apollo/client/react";
import { useQuery } from "@apollo/client/react";
import { List, Button, message } from "antd";
import { useEffect, useState } from "react";
import {
  InstitutionDocument,
  InstitutionsDocument,
  PendingInstitutionsDocument,
  VerifyInstitutionDocument,
} from "../../queries/institutions.generated";
import { Scalars } from "../../__generated__/graphql";
import Link from "next/link";
import paths from "../../paths";
import { stringifyApolloError } from "../../lib/apollo";

export type PendingInstitutionsProps = {};

export default function PendingInstitutions({}: PendingInstitutionsProps) {
  const { data, loading, error } = useQuery(PendingInstitutionsDocument);
  const [messageApi, contextHolder] = message.useMessage();

  useEffect(() => {
    if (error) {
      messageApi.error(stringifyApolloError(error));
    }
  }, [error]);

  const [verifyInstitutionMutation] = useMutation(VerifyInstitutionDocument);
  const [verifyingInstitution, setVerifyingInstitution] = useState(false);

  const verifyInstitution = async (institutionId: Scalars["Uuid"]["input"]) => {
    try {
      setVerifyingInstitution(true);
      const { error, data } = await verifyInstitutionMutation({
        variables: {
          institutionId: institutionId,
        },
        refetchQueries: [
          {
            query: InstitutionsDocument,
          },
          {
            query: PendingInstitutionsDocument,
          },
          {
            query: InstitutionDocument,
            variables: {
              uuid: institutionId,
            },
          },
        ],
      });
      if (error) {
        console.log(error); // TODO What to do?
      } else if (data?.verifyInstitution?.errors) {
        // TODO Is this how we want to display errors?
        message.error(
          data?.verifyInstitution?.errors
            .map((error) => error.message)
            .join(" "),
        );
      }
    } finally {
      setVerifyingInstitution(false);
    }
  };

  return (
    <>
      {contextHolder}
      <List
        size="small"
        loading={loading}
        dataSource={data?.pendingInstitutions?.edges?.map((e) => e.node) || []}
        renderItem={(item) => (
          <List.Item>
            <Link href={paths.institution(item?.uuid)} legacyBehavior>
              {item?.name}
            </Link>
            <Button
              onClick={() => verifyInstitution(item?.uuid)}
              loading={verifyingInstitution}
            >
              Verify
            </Button>
          </List.Item>
        )}
      />
    </>
  );
}
