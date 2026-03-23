import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  ForbidGnuPgKeyFingerprintDocument,
  ForbidGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export type ForbidGnuPgKeyFingerprintProps = {
  fingerprint: string;
  institutionId: Scalars["Uuid"]["input"];
};

export default function ForbidGnuPgKeyFingerprint({
  fingerprint,
  institutionId,
}: ForbidGnuPgKeyFingerprintProps) {
  const [forbidGnuPgKeyFingerprintMutation] = useMutation(
    ForbidGnuPgKeyFingerprintDocument,
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
    useMutationHandler<ForbidGnuPgKeyFingerprintMutation>({
      getErrors: (data) => data.forbidGnuPgKeyFingerprint.errors,
    });

  const forbid = async () => {
    withMutationHandler(
      () =>
        forbidGnuPgKeyFingerprintMutation({
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
    <Button onClick={() => forbid()} loading={mutating}>
      Forbid
    </Button>
  );
}
