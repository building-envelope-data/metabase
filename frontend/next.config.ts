import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  reactStrictMode: true,
  typedRoutes: true,
  allowedDevOrigins: [],
  turbopack: {
    rules: {
      "/\.(yml|yaml$)/": ["yaml-loader"],
    },
  },
  experimental: {
    typedEnv: true,
  },
  // experimental: {
  //   swcPlugins: [
  //     [
  //       '@swc-contrib/plugin-graphql-codegen-client-preset',
  //       {
  //         artifactDirectory: './__generated__/',
  //         gqlTagName: 'graphql'
  //       }
  //     ]
  //   ]
  // },
};

export default nextConfig;
