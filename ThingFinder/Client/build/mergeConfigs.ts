import { Configuration } from "webpack";
import { mergeWithRules } from "webpack-merge";

export function mergeConfigs(firstConfiguration: Configuration | Configuration[], ...configurations: Configuration[]) {
	return mergeWithRules({
		module: {
			rules: {
				test: "match",
				use: "merge",
			},
		},
	})(firstConfiguration, ...configurations);
}
