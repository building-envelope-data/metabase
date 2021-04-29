declare global {
  namespace NodeJS {
    interface ProcessEnv {
      NEXT_PUBLIC_METABASE_URL: string;
      NODE_ENV: "test" | "development" | "production";
    }
  }
}
