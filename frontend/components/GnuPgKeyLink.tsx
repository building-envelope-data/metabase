import Link from "next/link";
import Copyable from "./Copyable";
import paths from "../paths";
import CopyableBlock from "./CopyableBlock";

export default function GnuPgKeyLink({
  fingerprint,
  block = false,
}: {
  fingerprint: string;
  block?: boolean;
}) {
  const link = (
    <Link href={paths.gnuPgKey(fingerprint)}>
      <code>{fingerprint}</code>
    </Link>
  );

  return block ? (
    <CopyableBlock text={fingerprint}>{link}</CopyableBlock>
  ) : (
    <Copyable onlyIcon text={fingerprint}>
      {link}
    </Copyable>
  );
}
