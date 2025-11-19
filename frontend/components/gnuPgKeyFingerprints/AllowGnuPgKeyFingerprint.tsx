import { useMutation } from '@apollo/client/react';
import { Button, message } from "antd";
import {
    AllowGnuPgKeyFingerprintDocument,
} from "../../queries/gnuPgKeyFingerprints.generated";
import { Scalars } from "../../__generated__/graphql";
import { useState } from "react";
import { InstitutionDocument } from "../../queries/institutions.generated";

export type AllowGnuPgKeyFingerprintProps = {
    fingerprint: string;
    institutionId: Scalars["Uuid"]["input"];
};

export default function AllowGnuPgKeyFingerprint({ fingerprint, institutionId }: AllowGnuPgKeyFingerprintProps) {
    const [allowGnuPgKeyFingerprintMutation] = useMutation(AllowGnuPgKeyFingerprintDocument, {
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
    });
    const [allowing, setAllowing] = useState(false);

    const allow = async () => {
        try {
            setAllowing(true);
            // https://www.apollographql.com/docs/react/networking/authentication/#reset-store-on-logout
            const { error, data } = await allowGnuPgKeyFingerprintMutation({
                variables: {
                    fingerprint: fingerprint,
                },
            });
            if (error) {
                console.log(error); // TODO What to do?
            } else if (data?.allowGnuPgKeyFingerprint?.errors) {
                // TODO Is this how we want to display errors?
                message.error(
                    data?.allowGnuPgKeyFingerprint?.errors.map((error) => error.message).join(" ")
                );
            }
        } catch (error) {
            // TODO Handle properly.
            console.log("Failed:", error);
        } finally {
            setAllowing(false);
        }
    };

    return (
        <Button onClick={() => allow()} loading={allowing}>
            Allow
        </Button>
    );
}
