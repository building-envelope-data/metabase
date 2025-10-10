import { Skeleton, Space, Table, TableProps } from "antd";
import { GnuPgKeyFingerprintPartialFragment } from "../../queries/institutions.graphql";
import Link from "next/link";
import paths from "../../paths";
import AllowGnuPgKeyFingerprint from "./AllowGnuPgKeyFingerprint"
import ForbidGnuPgKeyFingerprint from "./ForbidGnuPgKeyFingerprint";
import { Scalars } from "../../__generated__/__types__";

export type GnuPgKeyFingerprintsProps = {
    loading: boolean;
    fingerprints: GnuPgKeyFingerprintPartialFragment[];
    institutionId: Scalars["Uuid"];
};

export default function GnuPgKeyFingerprintTable({ loading, fingerprints, institutionId }: GnuPgKeyFingerprintsProps) {
    if (loading) {
        return <Skeleton active avatar title />;
    }

    const fingerprintColumns: TableProps<GnuPgKeyFingerprintPartialFragment>['columns'] = [
        {
            title: "Fingerprint",
            dataIndex: "fingerprint",
            key: "fingerprint",
        },
        {
            title: "AllowedAt",
            dataIndex: "allowedAt",
            key: "allowedAt",
        },
        {
            title: "ForbiddenAt",
            dataIndex: "forbiddenAt",
            key: "forbiddenAt",
        },
        {
            title: "User",
            dataIndex: "user",
            key: "user",
            render: (_value, record, _index) =>
                <Link href={paths.user(record.user.node.uuid)}>{record.user.node.name}</Link>
        },
        {
            title: "Action",
            key: "action",
            render: (_value, record, _index) => (
                <Space size="middle">
                    {record.canCurrentUserAllowNode && record.allowedAt == undefined
                        ? <AllowGnuPgKeyFingerprint fingerprint={record.fingerprint} institutionId={institutionId} />
                        : <></>
                    }
                    {record.canCurrentUserForbidNode && record.forbiddenAt == undefined
                        ? <ForbidGnuPgKeyFingerprint fingerprint={record.fingerprint} institutionId={institutionId} />
                        : <></>
                    }
                </Space>
            )
        },
    ];

    return <Table<GnuPgKeyFingerprintPartialFragment>
        loading={loading}
        columns={fingerprintColumns}
        dataSource={fingerprints}
    />;
}