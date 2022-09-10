import { Configuration } from "webpack";
import { Environment, DefaultEnvironment } from "./build/environment.js";
import { mergeConfigs } from "./build/mergeConfigs.js";
import { baseConfig } from "./webpack.config.base.js";
import { devConfig } from "./webpack.config.dev.js";
import { prodConfig } from "./webpack.config.prod.js";

export default async function (env: Partial<Environment>): Promise<Configuration> {
	const { prod }: Environment = { ...DefaultEnvironment, ...env };

	if (prod) {
		return mergeConfigs(baseConfig, prodConfig);
	} else {
		return mergeConfigs(baseConfig, devConfig);
	}
}
