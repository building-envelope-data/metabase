import { useRouter } from "next/router";
import GnuPgKey from "../../components/gnuPgKeys/GnuPgKey";
import Layout from "../../components/Layout";

function Page() {
  const router = useRouter();

  if (!router.isReady) {
    // Otherwise `fingerprint`, aka, `router.query`, is null on first render, see https://github.com/vercel/next.js/discussions/11484
    return null;
  }

  const { fingerprint } = router.query;

  return (
    <Layout>
      <GnuPgKey fingerprint={String(fingerprint)} />
    </Layout>
  );
}

export default Page;
