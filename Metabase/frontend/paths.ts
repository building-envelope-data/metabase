export default {
  home: "/",
  institutions: "/institutions",
  institution(uuid: string) {
    return `/institutions/${encodeURIComponent(uuid)}`;
  },
  institutionCreate: "/institutions/create",
  user(uuid: string) {
    return `/users/${encodeURIComponent(uuid)}`;
  },
  userLogin: "/users/login",
  userRegister: "/users/register",
  userChangeEmail: "/user/change-email",
  userConfirmEmail: "/user/confirm-email",
  userForgotPassword: "/user/forgot-password",
  userSendTwoFactorCode: "/user/send-two-factor-code",
  userVerifyTwoFactorCode: "/user/verify-two-factor-code",
};
