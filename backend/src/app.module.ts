import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { AuthModule } from './auth/auth.module';
import { UsersModule } from './users/users.module';
import { GraphQLModule } from '@nestjs/graphql';
import { ProductsModule } from './products/products.module';
import { ProductsController } from './products/products.controller';
import { ProductsService } from './products/products.service';
import { TypeOrmModule, TypeOrmModuleOptions } from '@nestjs/typeorm';
import { ConfigModule } from './config/config.module';
import { ConfigService } from './config/config.service';

// TODO I had to run `ALTER USER postgres PASSWORD 'postgres';` manually in
// `psql postgres postgres` in running container `database`; `docker ps` and
// `docker exec -it ${DATABASE_CONTAINER_ID} bash`. Why?
@Module({
  imports: [
    TypeOrmModule.forRootAsync({
      imports: [ConfigModule],
      useFactory: async (configService: ConfigService) => ({
        type: configService.getString('TYPEORM_CONNECTION'), // Use `as 'postgres'` to cast type to union if you do not want to cast the whole thing to `TypeOrmModuleOptions` as done below; see https://github.com/nestjs/nest/issues/1119#issuecomment-424504139
        host: configService.getString('TYPEORM_HOST'),
        port: configService.getInteger('TYPEORM_PORT'),
        username: configService.getString('TYPEORM_USERNAME'),
        password: configService.getString('TYPEORM_PASSWORD'),
        database: configService.getString('TYPEORM_DATABASE'),
        logging: configService.getBoolean('TYPEORM_LOGGING'),
        synchronize: configService.getBoolean('TYPEORM_SYNCHRONIZE'),
        entities: [__dirname + '/**/*.entity{.ts,.js}'],
      }) as TypeOrmModuleOptions, // https://github.com/nestjs/nest/issues/1119#issuecomment-459982798
      inject: [ConfigService],
    }),
    ConfigModule,
    AuthModule,
    UsersModule,
    GraphQLModule.forRoot({
      debug: true,
      playground: true,
      include: [ProductsModule],
      context: ({ req }) => ({ req }),
      autoSchemaFile: 'schema.gql',
    }), // For a list of settings see https://www.apollographql.com/docs/apollo-server/v2/api/apollo-server.html#constructor-options-lt-ApolloServer-gt
    ProductsModule,
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
