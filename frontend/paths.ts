import type { Route } from "next";
import { DataKind, Scalars } from "./__generated__/graphql";

export default {
  home: "/" as Route,
  antiforgeryToken: "/antiforgery/token" as Route,
  legalNotice: "/legal-notice" as Route,
  dataProtectionInformation: "/data-protection-information" as Route,
  databases: "/databases" as Route,
  database(id: Scalars["Uuid"]["output"]) {
    return `/databases/${encodeURIComponent(id)}` as Route;
  },
  institutions: "/institutions" as Route,
  institution(id: Scalars["Uuid"]["output"]) {
    return `/institutions/${encodeURIComponent(id)}` as Route;
  },
  users: "/users" as Route,
  user(id: Scalars["Uuid"]["output"]) {
    return `/users/${encodeURIComponent(id)}` as Route;
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
  userRegister: "/users/register" as Route,
  userForgotPassword: "/users/forgot-password" as Route,
  userLogin: "/users/login" as Route,
  userLoginWithTwoFactorCode: "/users/login/with-two-factor-code" as Route,
  userLoginWithRecoveryCode: "/users/login/with-recovery-code" as Route,
  userResendEmailConfirmation: "/users/resend-email-confirmation" as Route,
  userCheckYourInboxAfterRegistration:
    "/users/check-your-inbox-after-registration" as Route,
  userCheckYourInboxAfterResendingEmailConfirmation:
    "/users/check-your-inbox-after-resending-email-confirmation" as Route,
  userCheckYourInboxAfterPasswordResetRequest:
    "/users/check-your-inbox-after-password-reset-request" as Route,
  dataFormats: "/data-formats" as Route,
  dataFormat(id: Scalars["Uuid"]["output"]) {
    return `/data-formats/${encodeURIComponent(id)}` as Route;
  },
  methods: "/methods" as Route,
  method(id: Scalars["Uuid"]["output"]) {
    return `/methods/${encodeURIComponent(id)}` as Route;
  },
  components: "/components" as Route,
  component(id: Scalars["Uuid"]["output"]) {
    return `/components/${encodeURIComponent(id)}` as Route;
  },
  gnuPgKeys: "/gnupg-keys" as Route,
  gnuPgKey(fingerprint: string) {
    return `/gnupg-keys/${encodeURIComponent(fingerprint)}` as Route;
  },
  allData: "/data" as Route,
  data(
    databaseId: Scalars["Uuid"]["output"],
    dataKind: DataKind,
    id: Scalars["Uuid"]["output"],
  ) {
    switch (dataKind) {
      case DataKind.CalorimetricData:
        return this.calorimetricData(databaseId, id);
      case DataKind.GeometricData:
        return this.geometricData(databaseId, id);
      case DataKind.HygrothermalData:
        return this.hygrothermalData(databaseId, id);
      case DataKind.LifeCycleData:
        return this.lifeCycleData(databaseId, id);
      case DataKind.OpticalData:
        return this.opticalData(databaseId, id);
      case DataKind.PhotovoltaicData:
        return this.photovoltaicData(databaseId, id);
      default:
        return assertNever(dataKind);
    }
  },
  allCalorimetricData: "/data/calorimetric" as Route,
  calorimetricData(
    databaseId: Scalars["Uuid"]["output"],
    id: Scalars["Uuid"]["output"],
  ) {
    return `/databases/${encodeURIComponent(databaseId)}/data/calorimetric/${encodeURIComponent(id)}` as Route;
  },
  allHygrothermalData: "/data/hygrothermal" as Route,
  hygrothermalData(
    databaseId: Scalars["Uuid"]["output"],
    id: Scalars["Uuid"]["output"],
  ) {
    return `/databases/${encodeURIComponent(databaseId)}/data/hygrothermal/${encodeURIComponent(id)}` as Route;
  },
  allLifeCycleData: "/data/life-cycle" as Route,
  lifeCycleData(
    databaseId: Scalars["Uuid"]["output"],
    id: Scalars["Uuid"]["output"],
  ) {
    return `/databases/${encodeURIComponent(databaseId)}/data/life-cycle/${encodeURIComponent(id)}` as Route;
  },
  allOpticalData: "/data/optical" as Route,
  opticalData(
    databaseId: Scalars["Uuid"]["output"],
    id: Scalars["Uuid"]["output"],
  ) {
    return `/databases/${encodeURIComponent(databaseId)}/data/optical/${encodeURIComponent(id)}` as Route;
  },
  allPhotovoltaicData: "/data/photovoltaic" as Route,
  photovoltaicData(
    databaseId: Scalars["Uuid"]["output"],
    id: Scalars["Uuid"]["output"],
  ) {
    return `/databases/${encodeURIComponent(databaseId)}/data/photovoltaic/${encodeURIComponent(id)}` as Route;
  },
  allGeometricData: "/data/geometric" as Route,
  geometricData(
    databaseId: Scalars["Uuid"]["output"],
    id: Scalars["Uuid"]["output"],
  ) {
    return `/databases/${encodeURIComponent(databaseId)}/data/geometric/${encodeURIComponent(id)}` as Route;
  },
  openIdConnectApplication(id: Scalars["Uuid"]["output"]) {
    return `/open-id-connect/application/${encodeURIComponent(id)}` as Route;
  },
  openIdConnect: "/open-id-connect" as Route,
  openIdConnectClientLogin: "/connect/client/login" as Route,
  openIdConnectClientLogout: "/connect/client/logout" as Route,
};
