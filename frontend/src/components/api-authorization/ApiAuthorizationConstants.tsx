export const ApplicationName = 'icon';

export enum QueryParameterNames {
  ReturnUrl = 'returnUrl',
  Message = 'message'
};

export enum LogoutActions {
  LogoutCallback = 'logout-callback',
  Logout = 'logout',
  LoggedOut = 'logged-out'
};

export enum LoginActions {
  Login = 'login',
  LoginCallback = 'login-callback',
  LoginFailed = 'login-failed',
  Profile = 'profile',
  Register = 'register'
};

const prefix = '/authentication';

export const ApplicationPaths = {
  DefaultLoginRedirectPath: '/',
  ApiAuthorizationClientConfigurationUrl: `/_configuration/${ApplicationName}`,
  ApiAuthorizationPrefix: prefix,
  Login: `${prefix}/${LoginActions.Login}`,
  LoginFailed: `${prefix}/${LoginActions.LoginFailed}`,
  LoginCallback: `${prefix}/${LoginActions.LoginCallback}`,
  Register: `${prefix}/${LoginActions.Register}`,
  Profile: `${prefix}/${LoginActions.Profile}`,
  LogOut: `${prefix}/${LogoutActions.Logout}`,
  LoggedOut: `${prefix}/${LogoutActions.LoggedOut}`,
  LogOutCallback: `${prefix}/${LogoutActions.LogoutCallback}`,
  RegisterPath: '/Account/Register',
  ManagePath: '/Account/Manage'
};
