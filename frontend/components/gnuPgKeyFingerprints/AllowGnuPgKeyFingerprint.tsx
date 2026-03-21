import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  AllowGnuPgKeyFingerprintDocument,
  AllowGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { InstitutionDocument } from "../../queries/institutions.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

export type AllowGnuPgKeyFingerprintProps = {
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
      // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-allows
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
    <Button onClick={() => allow()} loading={mutating}>
      Allow
    </Button>
  );
}
