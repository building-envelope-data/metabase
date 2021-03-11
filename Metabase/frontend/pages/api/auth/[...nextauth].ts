import { NextApiHandler } from "next";
import NextAuth from "next-auth";

const authHandler: NextApiHandler = (req, res) => NextAuth(req, res, options);
export default authHandler;

// https://next-auth.js.org/configuration/options
const options = {
  providers: [
    {
      // https://next-auth.js.org/configuration/providers#using-a-custom-provider
      // https://next-auth.js.org/configuration/providers#oauth-provider-options
      // https://buildingenvelopedata.org/.well-known/openid-configuration
      id: "metabase",
      name: "Metabase",
      type: "oauth",
      version: "2.0",
      scope: "openid email profile roles api:read api:write offline_access",
      params: { grant_type: "authorization_code" },
      authorizationParams: {},

      // Note that the host of `authorizationUrl`, `accessTokenUrl`, and
      // `requestTokenUrl` must be the same (supposedly for security). And that
      // `authorizationUrl` must be a valid URL outside of the Docker network
      // (as it is requested within the web browser). That is why we cannot use
      // http://backend:8080/connect/token as `accessTokenUrl` (although it is
      // the correct URL within the Docker network and only used within that
      // network from the `frontend` service).
      accessTokenUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/token`, // ,
      requestTokenUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/authorize`,
      authorizationUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/authorize?response_type=code`,

      profileUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/userinfo`,
      protection: "pkce", // https://security.stackexchange.com/questions/214980/does-pkce-replace-state-in-the-authorization-code-oauth-flow/215027#215027
      idToken: true,
      headers: {},
      async profile(
        profile: { sub: string; email: string }
        // tokens: { access_token: string, id_token: string, refresh_token: string }
      ) {
        // You can use the tokens, in case you want to fetch more profile information
        // For example several OAuth provider does not return e-mail by default.
        // Depending on your provider, will have tokens like `access_token`, `id_token` and or `refresh_token`
        return {
          id: profile.sub,
          name: profile.email, // TODO Include `name` in token?
          email: profile.email,
        };
      },
      clientId: process.env.AUTH_CLIENT_ID,
      clientSecret: process.env.AUTH_CLIENT_SECRET,
    },
  ],
  secret: process.env.AUTH_SECRET,
  session: {
    // Use JSON Web Tokens for session instead of database sessions.
    // This option can be used with or without a database for users/accounts.
    // Note: `jwt` is automatically set to `true` if no database is specified.
    jwt: true,

    // Seconds - How long until an idle session expires and is no longer valid.
    maxAge: 30 * 24 * 60 * 60, // 30 days

    // Seconds - Throttle how frequently to write to database to extend a session.
    // Use it to limit write operations. Set to 0 to always update the database.
    // Note: This option is ignored if using JSON Web Tokens
    // updateAge: 24 * 60 * 60, // 24 hours
  },
  jwt: {
    // A secret to use for key generation - you should set this explicitly
    // Defaults to NextAuth.js secret if not explicitly specified.
    secret: process.env.AUTH_JWT_SECRET,

    // Set to true to use encryption. Defaults to false (signing only).
    encryption: true,

    // You can define your own encode/decode functions for signing and encryption
    // if you want to override the default behaviour.
    // async encode({ secret, token, maxAge }) {},
    // async decode({ secret, token, maxAge }) {},
  },
  debug: process.env.NODE_ENV !== "production",
  theme: "auto",
  // pages: {
  //   signIn: '/auth/signin',
  //   signOut: '/auth/signout',
  //   error: '/auth/error', // Error code passed in query string as ?error=
  //   verifyRequest: '/auth/verify-request', // (used for check email message)
  //   newUser: null // If set, new users will be directed here on first sign in
  // },
  // callbacks: {
  //   async signIn(user, account, profile) {
  //     return true
  //   },
  //   async redirect(url, baseUrl) {
  //     return baseUrl
  //   },
  //   async session(session, user) {
  //     return session
  //   },
  //   async jwt(token, user, account, profile, isNewUser) {
  //     return token
  //   }
  // },
  // events: {
  //   async signIn(message) { /* on successful sign in */ },
  //   async signOut(message) { /* on signout */ },
  //   async createUser(message) { /* user created */ },
  //   async linkAccount(message) { /* account linked to a user */ },
  //   async session(message) { /* session is active */ },
  //   async error(message) { /* error in authentication flow */ },
  // },
  // logger: {
  //   error(code, ...message) { log.error(code, message) },
  //   warn(code, ...message) { log.warn(code, message) },
  //   debug(code, ...message) { log.debug(code, message) },
  // },
};
