import type { Route } from "next";

export default {
  home: "/" as Route,
  antiforgeryToken: "/antiforgery/token" as Route,
  legalNotice: "/legal-notice" as Route,
  dataProtectionInformation: "/data-protection-information" as Route,
  databases: "/databases" as Route,
  database(uuid: string) {
    return `/databases/${encodeURIComponent(uuid)}` as Route;
  },
  institutions: "/institutions" as Route,
  institution(uuid: string) {
    return `/institutions/${encodeURIComponent(uuid)}` as Route;
  },
  institutionCreate: "/institutions/create" as Route,
  users: "/users" as Route,
  user(uuid: string) {
    return `/users/${encodeURIComponent(uuid)}` as Route;
  },
  userCurrent: "me" as Route,
  me: {
    manage: {
      home: "/me/manage" as Route,
      profile: "/me/manage/profile" as Route,
      email: "/me/manage/email" as Route,
      changePassword: "/me/manage/change-password" as Route,
      setPassword: "/me/manage/set-password" as Route,
      twoFactorAuthentication: "/me/manage/two-factor-authentication" as Route,
      enableAuthenticator: "/me/manage/enable-authenticator" as Route,
      personalData: "/me/manage/personal-data" as Route,
    },
  },
  personalUserData: "/personal-user-data" as Route,
  userLogin: "/users/login" as Route,
  userRegister: "/users/register" as Route,
  userConfirmEmail: "/users/confirm-email" as Route,
  userForgotPassword: "/users/forgot-password" as Route,
  userLoginWithTwoFactorCode: "/users/login/with-two-factor-code" as Route,
  userLoginWithRecoveryCode: "/users/login/with-recovery-code" as Route,
  userCheckYourInboxAfterRegistration:
    "/users/check-your-inbox-after-registration" as Route,
  userCheckYourInboxAfterPasswordResetRequest:
    "/users/check-your-inbox-after-password-reset-request" as Route,
  dataFormats: "/data-formats" as Route,
  dataFormat(uuid: string) {
    return `/data-formats/${encodeURIComponent(uuid)}` as Route;
  },
  methods: "/methods" as Route,
  method(uuid: string) {
    return `/methods/${encodeURIComponent(uuid)}` as Route;
  },
  components: "/components" as Route,
  component(uuid: string) {
    return `/components/${encodeURIComponent(uuid)}` as Route;
  },
  data: "/data" as Route,
  calorimetricData: "/data/calorimetric" as Route,
  hygrothermalData: "/data/hygrothermal" as Route,
  lifeCycleData: "/data/life-cycle" as Route,
  opticalData: "/data/optical" as Route,
  photovoltaicData: "/data/photovoltaic" as Route,
  geometricData: "/data/geometric" as Route,
  openIdConnectApplication(uuid: string) {
    return `/open-id-connect/application/${encodeURIComponent(uuid)}` as Route;
  },
  openIdConnectApplicationCreate:
    "/open-id-connect/application/create" as Route,
  openIdConnect: "/open-id-connect" as Route,
  openIdConnectClientLogin: "/connect/client/login" as Route,
  openIdConnectClientLogout: "/connect/client/logout" as Route,
};
