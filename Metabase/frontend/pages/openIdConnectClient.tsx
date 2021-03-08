import { Skeleton } from "antd";
import { signIn, signOut, useSession } from "next-auth/client";
import Layout from "../components/Layout";

const OpenIdConnectClient = () => {
  const [session, loading] = useSession();

  if (loading) {
    return (
      <Layout>
        <Skeleton />
      </Layout>
    );
  }

  return (
    <Layout>
      {!session && (
        <>
          Not signed in <br />
          <button onClick={() => signIn()}>Sign in</button>
        </>
      )}
      {session && (
        <>
          Signed in as {session.user.email} <br />
          <button onClick={() => signOut()}>Sign out</button>
        </>
      )}
    </Layout>
  );
};

export default OpenIdConnectClient;
