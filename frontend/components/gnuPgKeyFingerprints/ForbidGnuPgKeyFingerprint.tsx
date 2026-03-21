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
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-forbids
      // See https://www.apollographql.com/docs/react/data/mutations/#options
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
