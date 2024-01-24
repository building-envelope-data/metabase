// File Format: https://the-guild.dev/graphql/codegen/docs/config-reference/codegen-config
import type { CodegenConfig } from '@graphql-codegen/cli'

const config: CodegenConfig = {
  schema: './type-defs.graphqls',
  generates: {
    '__generated__/fragmentMatcherTypes.ts': {
      plugins: [
        // https://the-guild.dev/graphql/codegen/plugins/other/fragment-matcher
        'fragment-matcher',
      ],
      config: {
        module: 'es2015',
        apolloClientVersion: 3,
        useExplicitTyping: false
      },
    },
    '__generated__/__types__.tsx': {
      plugins: [
        {
          // https://the-guild.dev/graphql/codegen/plugins/other/add
          // https://github.com/dotansimha/graphql-code-generator/issues/9498
          add: {
            content: '// @ts-ignore'
          }
        },
        'typescript'
      ]
    }
  },
}
export default config
