export default {
  home: "/",
  institutions: "/institutions",
  institution(uuid: string) {
    return `/institutions/${encodeURIComponent(uuid)}`;
  },
  institutionCreate: "/institutions/create",
  users: "/users",
  user(uuid: string) {
    return `/users/${encodeURIComponent(uuid)}`;
  },
  userCurrent: "me",
  userLogin: "/users/login",
  userRegister: "/users/register",
  userChangeEmail: "/me/change-email",
  userConfirmEmail: "/users/confirm-email",
  userForgotPassword: "/users/forgot-password",
  userLoginWithTwoFactorCode: "/users/login-with-two-factor-code",
  userLoginWithRecoveryCode: "/users/login-with-recovery-code",
  userCheckYourInbox: "/users/check-your-inbox",
  dataFormats: "/data-formats",
  dataFormat(uuid: string) {
    return `/data-formats/${encodeURIComponent(uuid)}`;
  },
  methods: "/methods",
  method(uuid: string) {
    return `/methods/${encodeURIComponent(uuid)}`;
  },
  components: "/components",
  component(uuid: string) {
    return `/components/${encodeURIComponent(uuid)}`;
  },
  openIdConnect: "/openIdConnect",
};
