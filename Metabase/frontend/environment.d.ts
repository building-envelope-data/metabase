declare global {
  namespace NodeJS {
    interface ProcessEnv {
      NEXT_PUBLIC_METABASE_URL: string;
      NEXTAUTH_URL: string;
      NODE_ENV: "test" | "development" | "production";
      AUTH_CLIENT_ID: string;
      AUTH_CLIENT_SECRET: string;
      AUTH_SECRET: string;
      AUTH_JWT_SECRET: string;
    }
  }
}
