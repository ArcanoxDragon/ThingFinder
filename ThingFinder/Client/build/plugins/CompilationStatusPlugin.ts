import { Compiler, WebpackPluginInstance } from "webpack";

export class CompilationStatusPlugin implements WebpackPluginInstance {
	static readonly PluginName = "CompilationStatusPlugin";

	apply(compiler: Compiler): void {
		compiler.hooks.compilation.tap(CompilationStatusPlugin.PluginName, compilation => {
			console.warn("=== Compilation Started ===");

			compilation.hooks.seal.tap(CompilationStatusPlugin.PluginName, () => {
				console.warn("=== Compilation Finished ===");
			});
		});
	}
}
