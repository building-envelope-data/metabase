import * as dotenv from 'dotenv';
import * as Joi from '@hapi/joi';
import * as fs from 'fs';

export interface EnvConfig {
  [key: string]: string; // TODO Is `: string` correct here? Shouldn't it rather be `any`?
}

export class ConfigService {
  private readonly envConfig: EnvConfig;

  constructor(filePath: string) {
    const config = dotenv.parse(fs.readFileSync(filePath));
    this.envConfig = this.validateInput(config);
  }

  getString(key: string): string {
    return this.envConfig[key]
  }

  getInteger(key: string): number {
    return Number(this.envConfig[key])
  }

  getBoolean(key: string): boolean {
    return Boolean(this.envConfig[key])
  }

  // get isApiAuthEnabled(): boolean {
  //   return Boolean(this.envConfig.API_AUTH_ENABLED);
  // }

  /**
   * Ensures all needed variables are set, and returns the validated JavaScript object
   * including the applied default values.
   */
  private validateInput(envConfig: EnvConfig): EnvConfig {
    const envVarsSchema: Joi.ObjectSchema = Joi.object({
      TYPEORM_CONNECTION: Joi.string().valid('postgres', 'mysql').required(),
      TYPEORM_USERNAME: Joi.string().required(),
      TYPEORM_PASSWORD: Joi.string().required(),
      TYPEORM_HOST: Joi.string().required(),
      TYPEORM_PORT: Joi.number().required(),
      TYPEORM_DATABASE: Joi.string().required(),
      TYPEORM_LOGGING: Joi.boolean().required(),
      TYPEORM_SYNCHRONIZE: Joi.boolean().required(),
    });

    const { error, value: validatedEnvConfig } = envVarsSchema.validate(
      envConfig,
    );
    if (error) {
      throw new Error(`Config validation error: ${error.message}`);
    }
    return validatedEnvConfig;
  }
}
