/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  allowedDevOrigins: [new URL(process.env.NEXT_PUBLIC_METABASE_URL).hostname],
  turbopack: {
    rules: {
      '/\.(yml|yaml$)/': [
        'yaml-loader',
      ],
    },
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
}

module.exports = nextConfig;
