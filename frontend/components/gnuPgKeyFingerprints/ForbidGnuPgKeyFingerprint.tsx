import { useMutation } from '@apollo/client/react';
import { Button, message } from "antd";
import {
    ForbidGnuPgKeyFingerprintDocument,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";

export type ForbidGnuPgKeyFingerprintProps = {
    fingerprint: string;
    institutionId: Scalars["Uuid"];
};

export default function ForbidGnuPgKeyFingerprint({ fingerprint, institutionId }: ForbidGnuPgKeyFingerprintProps) {
    const [forbidGnuPgKeyFingerprintMutation] = useMutation(ForbidGnuPgKeyFingerprintDocument, {
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
    });
    const [forbidding, setForbidding] = useState(false);

    const forbid = async () => {
        try {
            setForbidding(true);
            // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
            const { error, data } = await forbidGnuPgKeyFingerprintMutation({
                variables: {
                    fingerprint: fingerprint,
                },
            });
            if (error) {
                console.log(error); // TODO What to do?
            } else if (data?.forbidGnuPgKeyFingerprint?.errors) {
                // TODO Is this how we want to display errors?
                message.error(
                    data?.forbidGnuPgKeyFingerprint?.errors.map((error) => error.message).join(" ")
                );
            }
        } catch (error) {
            // TODO Handle properly.
            console.log("Failed:", error);
        } finally {
            setForbidding(false);
        }
    };

    return (
        <Button onClick={() => forbid()} loading={forbidding}>
            Forbid
        </Button>
    );
}
