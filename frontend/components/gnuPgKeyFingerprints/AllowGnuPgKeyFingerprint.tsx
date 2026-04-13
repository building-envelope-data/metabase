import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  AllowGnuPgKeyFingerprintDocument,
  AllowGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface AllowGnuPgKeyFingerprintProps {
  fingerprint: string;
  institutionId: Scalars["Uuid"]["input"];
};

export default function AllowGnuPgKeyFingerprint({
  fingerprint,
  institutionId,
}: AllowGnuPgKeyFingerprintProps) {
  const [allowGnuPgKeyFingerprintMutation] = useMutation(
    AllowGnuPgKeyFingerprintDocument,
    {
      refetchQueries: [
        {
          query: InstitutionDocument,
          variables: {
            uuid: institutionId,
          },
        },
      ],
    },
  );

  const { mutating, withMutationHandler, messageErrors } =
    useMutationHandler<AllowGnuPgKeyFingerprintMutation>({
      getErrors: (data) => data.allowGnuPgKeyFingerprint.errors,
    });

  const allow = async () => {
    withMutationHandler(
      () =>
        allowGnuPgKeyFingerprintMutation({
          variables: {
            fingerprint: fingerprint,
          },
        }),
      {
        onError: messageErrors,
      },
    );
  };

  return (
    <Button onClick={allow} loading={mutating}>
      Allow
    </Button>
  );
}
