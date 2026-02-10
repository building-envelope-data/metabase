declare global {
  namespace NodeJS {
    interface ProcessEnv {
      NEXT_WEBPACK_USEPOLLING: string;
      NODE_ENV: "development" | "production";
    }
  }
}
