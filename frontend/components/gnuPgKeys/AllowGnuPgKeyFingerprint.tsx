import { useMutation } from "@apollo/client/react";
import { Button } from "antd";
import {
  AllowGnuPgKeyFingerprintDocument,
  AllowGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { useMutationHandler } from "../../lib/hooks/useMutationHandler";

interface AllowGnuPgKeyFingerprintProps {
  fingerprint: string;
}

export default function AllowGnuPgKeyFingerprint({
  fingerprint,
}: AllowGnuPgKeyFingerprintProps) {
  const [allowGnuPgKeyFingerprintMutation] = useMutation(
    AllowGnuPgKeyFingerprintDocument,
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
