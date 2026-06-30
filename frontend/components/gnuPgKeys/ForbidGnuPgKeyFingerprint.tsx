import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  ForbidGnuPgKeyFingerprintDocument,
  ForbidGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface ForbidGnuPgKeyFingerprintProps {
  fingerprint: string;
}

export default function ForbidGnuPgKeyFingerprint({
  fingerprint,
}: ForbidGnuPgKeyFingerprintProps) {
  const [forbidGnuPgKeyFingerprintMutation] = useMutation(
    ForbidGnuPgKeyFingerprintDocument,
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
    <Button onClick={forbid} loading={mutating}>
      Forbid
    </Button>
  );
}
