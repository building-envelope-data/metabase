import { NextApiHandler } from "next";
import NextAuth from "next-auth";

// @ts-ignore: TODO Ask `@types/next-auth` to extend their types? For example, to allow the callback `session` getting a `token` instead of a `user` as second parameter. See https://github.com/DefinitelyTyped/DefinitelyTyped/tree/master/types/next-auth
const authHandler: NextApiHandler = (req, res) => NextAuth(req, res, options);
export default authHandler;

interface ProviderJsonWebToken {
  name: string;
  email: string;
  picture: number | null;
  sub: string;
}

interface StoredJsonWebToken {
  refreshToken: string;
  accessToken: string;
  accessTokenExpires: number;
  error?: string;
  user: User;
}

interface SessionJsonWebToken extends StoredJsonWebToken {
  iat: number;
  exp: number;
}

// extends `SessionBase` from `@types/next-auth`, see https://github.com/DefinitelyTyped/DefinitelyTyped/blob/461c4b4dd7783684c16dbf1e9ef28b33a5455026/types/next-auth/_utils.d.ts#L26
interface Session {
  user?: User;
  accessToken?: string;
  expires?: number;
  error?: string;
}

interface ProviderAccount {
  provider: string;
  type: string;
  id: string;
  accessToken: string;
  accessTokenExpires: number | null;
  refreshToken: string;
  idToken: string;
  access_token: string;
  token_type: "Bearer";
  expires_in: number;
  scope: string;
  id_token: string;
  refresh_token: string;
}

interface ProviderProfile {
  sub: string;
  name: string;
  oi_au_id: string;
  azp: string;
  at_hash: string;
  oi_tkn_id: string;
  aud: string;
  exp: number;
  iss: string;
  iat: number;
}

interface User {
  id: string;
  name: string;
  email: string;
  image: string | null;
}

/**
 * Takes a token, and returns a new token with updated
 * `accessToken` and `accessTokenExpires`. If an error occurs,
 * returns the old token and an error property
 *
 * See https://next-auth.js.org/tutorials/refresh-token-rotation
 */
async function refreshAccessToken(
  token: StoredJsonWebToken
): Promise<StoredJsonWebToken> {
  try {
    const url =
      `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/token?` +
      new URLSearchParams({
        client_id: process.env.AUTH_CLIENT_ID || "", // TODO Throw an error if environment variable is missing!
        client_secret: process.env.AUTH_CLIENT_SECRET || "", // TODO Throw an error if environment variable is missing!
        grant_type: "refresh_token",
        refresh_token: token.refreshToken,
      });
    const response = await fetch(url, {
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      method: "POST",
    });
    const refreshedTokens = await response.json();
    if (!response.ok) {
      throw refreshedTokens;
    }
    return {
      ...token,
      accessToken: refreshedTokens.access_token,
      accessTokenExpires: Date.now() + refreshedTokens.expires_in * 1000,
      refreshToken: refreshedTokens.refresh_token ?? token.refreshToken, // Fall back to old refresh token
    };
  } catch (error) {
    console.log(error);
    return {
      ...token,
      error: "RefreshAccessTokenError", // TODO Use global constant instead of string!
    };
  }
}

// https://next-auth.js.org/configuration/options
const options = {
  // database: {
  //   type: "sqlite",
  //   database: ":memory:",
  //   synchronize: true,
  // },
  // database: {
  //   type: "postgres",
  //   host: "database",
  //   port: 5432,
  //   username: "postgres",
  //   password: "postgres",
  //   database: "xbase_development",
  //   entityPrefix: "nextauth_",
  //   synchronize: process.env.NODE_ENV !== "production",
  //   // uri:
  //   //   process.env.NODE_ENV === "production"
  //   //     ? "Host=database; Port=5432; Database=xbase; User Id=postgres; Passfile=/run/secrets/pgpass;"
  //   //     : "Host=database; Port=5432; Database=xbase_development; User Id=postgres; Password=postgres;",
  // },
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
      accessTokenUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/token`,
      requestTokenUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/authorize`,
      authorizationUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/authorize?response_type=code`,

      profileUrl: `${process.env.NEXT_PUBLIC_METABASE_URL}/connect/userinfo`,
      protection: "pkce", // https://security.stackexchange.com/questions/214980/does-pkce-replace-state-in-the-authorization-code-oauth-flow/215027#215027
      idToken: true,
      headers: {},
      async profile(profile: ProviderProfile): Promise<User> {
        // You can use the tokens, in case you want to fetch more profile information
        // For example several OAuth provider does not return e-mail by default.
        // Depending on your provider, will have tokens like `access_token`, `id_token` and or `refresh_token`
        return {
          id: profile.sub,
          name: profile.name, // TODO Include `name` in token?
          email: profile.name,
          image: null, // TODO Include `image` in token?
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
  callbacks: {
    // See https://next-auth.js.org/tutorials/refresh-token-rotation
    /**
     * This JSON Web Token callback is called whenever a JSON Web Token is
     * created (i.e. at sign in) or updated (i.e whenever a session is accessed
     * in the client).
     *
     * @param  {object}  token     Decrypted JSON Web Token
     * @param  {object}  user      User object      (only available on sign in)
     * @param  {object}  account   Provider account (only available on sign in)
     * @param  {object}  profile   Provider profile (only available on sign in)
     * @param  {boolean} isNewUser True if new user (only available on sign in)
     * @return {object}            JSON Web Token that will be saved
     */
    async jwt(
      token: ProviderJsonWebToken | StoredJsonWebToken,
      user: User | undefined,
      account: ProviderAccount | undefined,
      _profile: ProviderProfile | undefined,
      _isNewUser: boolean | undefined
    ): Promise<StoredJsonWebToken> {
      // Initial sign in
      if (user && account) {
        return {
          accessToken: account.accessToken,
          accessTokenExpires: Date.now() + account.expires_in * 1000,
          refreshToken: "", // account.refreshToken, // TODO When `accessToken` and `refreshToken` are set, then signing in does not work, there just is no session after signing in. Why? I first thought that it is a problem with the cookie size but it's not because the problem persists when I switch to a database for session storage.
          user,
        };
      }
      // Type Guard: https://basarat.gitbook.io/typescript/type-system/typeguard#in
      if ("accessToken" in token) {
        if (Date.now() < token.accessTokenExpires) {
          // Return previous token if the access token has not expired yet
          return token;
        }
        // Access token has expired, try to update it
        return refreshAccessToken(token);
      }
      throw new Error("Impossible!");
    },

    /**
     * The session callback is called whenever a session is checked. By
     * default, only a subset of the token is returned for increased security.
     * If you want to make something available you added to the token through
     * the jwt() callback, you have to explicitely forward it here to make it
     * available to the client.
     *
     * @param  {object} session      Session object
     * @param  {object} token        User object    (if using database sessions)
     *                               JSON Web Token (if not using database sessions)
     * @return {object}              Session that will be returned to the client
     */
    async session(
      _session: {
        user: { name?: string; email?: string; image?: string };
        accessToken?: string;
        expires: number;
      },
      token: SessionJsonWebToken
    ): Promise<Session> {
      if (token) {
        return {
          user: token.user,
          accessToken: token.accessToken,
          expires: token.accessTokenExpires,
          error: token.error,
        };
      }
      return {};
    },

    /**
     * Use the signIn() callback to control if a user is allowed to sign in.
     *
     * @param  {object} user     User object
     * @param  {object} account  Provider account
     * @param  {object} profile  Provider profile
     * @return {boolean|string}  Return `true` to allow sign in
     *                           Return `false` to deny access
     *                           Return `string` to redirect to (eg.: "/unauthorized")
     */
    // async signIn(
    //   user: User,
    //   account: ProviderAccount,
    //   profile: ProviderProfile
    // ): Promise<boolean> {
    //   const isAllowedToSignIn = true;
    //   if (isAllowedToSignIn) {
    //     return true;
    //   } else {
    //     // Return false to display a default error message
    //     return false;
    //     // Or you can return a URL to redirect to:
    //     // return '/unauthorized'
    //   }
    // },

    /**
     * The redirect callback is called anytime the user is redirected to a callback URL (e.g. on signin or signout).
     *
     * By default only URLs on the same URL as the site are allowed, you can use the redirect callback to customise that behaviour.
     * @param  {string} url      URL provided as callback URL by the client
     * @param  {string} baseUrl  Default base URL of site (can be used as fallback)
     * @return {string}          URL the client will be redirect to
     */
    // async redirect(url: string, baseUrl: string) {
    //   return url.startsWith(baseUrl) ? url : baseUrl;
    // },
  },
  // pages: {
  //   signIn: '/auth/signin',
  //   signOut: '/auth/signout',
  //   error: '/auth/error', // Error code passed in query string as ?error=
  //   verifyRequest: '/auth/verify-request', // (used for check email message)
  //   newUser: null // If set, new users will be directed here on first sign in
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
