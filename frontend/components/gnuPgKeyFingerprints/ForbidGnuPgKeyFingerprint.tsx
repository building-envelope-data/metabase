import * as React from "react";
import { Button, message } from "antd";
import {
    useForbidGnuPgKeyFingerprintMutation,
} from "../../queries/gnuPgKeyFingerprints.graphql";
import { Scalars } from "../../__generated__/__types__";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.graphql";

export type ForbidGnuPgKeyFingerprintProps = {
    fingerprint: string;
    institutionId: Scalars["Uuid"];
};

export default function ForbidGnuPgKeyFingerprint({ fingerprint, institutionId }: ForbidGnuPgKeyFingerprintProps) {
    const [revokeGnuPgKeyFingerprintMutation] = useForbidGnuPgKeyFingerprintMutation({
        // TODO Update the cache more efficiently as explained on https://www.apollographql.com/docs/react/caching/cache-interaction/ and https://www.apollographql.com/docs/react/data/mutations/#making-all-other-cache-revokes
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
    const [revokeing, setForbiding] = useState(false);

    const revoke = async () => {
        try {
            setForbiding(true);
            // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
            const { errors, data } = await revokeGnuPgKeyFingerprintMutation({
                variables: {
                    fingerprint: fingerprint,
                },
            });
            if (errors) {
                console.log(errors); // TODO What to do?
            } else if (data?.revokeGnuPgKeyFingerprint?.errors) {
                // TODO Is this how we want to display errors?
                message.error(
                    data?.revokeGnuPgKeyFingerprint?.errors.map((error) => error.message).join(" ")
                );
            }
        } catch (error) {
            // TODO Handle properly.
            console.log("Failed:", error);
        } finally {
            setForbiding(false);
        }
    };

    return (
        <Button onClick={() => revoke()} loading={revokeing}>
            Forbid
        </Button>
    );
}
