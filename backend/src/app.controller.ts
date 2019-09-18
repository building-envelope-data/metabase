import { Controller, Get, Request, Post, UseGuards } from '@nestjs/common';
import { AuthGuard } from '@nestjs/passport';
import { AuthService } from './auth/auth.service';

// TODO https://docs.nestjs.com/techniques/authentication#graphql
//      Put the following somewhere else!
// @Injectable()
// export class GqlAuthGuard extends AuthGuard('jwt') {
//   getRequest(context: ExecutionContext) {
//     const gqlContext = GqlExecutionContext.create(context);
//     return gqlContext.getContext().req;
//   }
// }

// import { createParamDecorator } from '@nestjs/common';

// export const CurrentUser = createParamDecorator(
//   (data, [root, args, ctx, info]) => ctx.req.user,
// );

// @Query(returns => User)
// @UseGuards(GqlAuthGuard)
// whoAmI(@CurrentUser() user: User) {
//   return this.userService.findById(user.id);
// }

@Controller('api')
export class AppController {
  constructor(private readonly authService: AuthService) {}

  @UseGuards(AuthGuard('local'))
  @Post('login')
  async login(@Request() req) {
    return this.authService.login(req.user);
  }

  @UseGuards(AuthGuard('jwt'))
  @Get('me')
  getProfile(@Request() req) {
    return req.user;
  }
}
