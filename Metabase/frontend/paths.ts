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
  userLogin: "/user/login",
  userRegister: "/users/register",
  userChangeEmail: "/user/change-email",
  userConfirmEmail: "/users/confirm-email",
  userForgotPassword: "/users/forgot-password",
  userSendTwoFactorCode: "/users/send-two-factor-code",
  userVerifyTwoFactorCode: "/users/verify-two-factor-code",
};
