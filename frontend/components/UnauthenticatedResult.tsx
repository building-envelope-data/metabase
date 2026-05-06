import { Result } from "antd";
import paths from "../paths";
import Link from "next/link";

export default function UnauthenticatedResult() {
  return (
    <Result
      status="403"
      title="401"
      subTitle="Sorry, you need to be logged in to access this page."
      extra={<Link href={paths.openIdConnectClientLogin}>Login</Link>}
    />
  );
}
