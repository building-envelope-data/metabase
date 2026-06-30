declare global {
  namespace NodeJS {
    interface ProcessEnv {
      HOST: string | undefined;
    }
  }
}

export {};
