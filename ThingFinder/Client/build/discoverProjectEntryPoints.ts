import path from "path";

import { globby } from "globby";
import { EntryObject } from "webpack";
import { ScriptsDir, StylesDir } from "./constants.js";

export async function discoverProjectEntryPoints(): Promise<EntryObject> {
	const scriptEntries = await discoverScriptEntryPoints();
	const styleEntries = await discoverStyleEntryPoints();

	return { ...scriptEntries, ...styleEntries };
}

async function discoverScriptEntryPoints(): Promise<EntryObject> {
	const scriptFiles = await globby("**/*.ts", { cwd: ScriptsDir });
	const entries: EntryObject = {};

	for (const file of scriptFiles) {
		const dir = path.dirname(file);
		const base = path.basename(file, ".ts");
		const entryName = `${dir}/${base}`;
		const entryPath = `./src/scripts/${file}`;

		entries[entryName] = entryPath;
	}

	return entries;
}

async function discoverStyleEntryPoints(): Promise<EntryObject> {
	const styleFiles = await globby([
		"**/*.scss",
		"!**/_*.scss",
	], { cwd: StylesDir });

	return {
		styles: {
			import: styleFiles.map(file => `./styles/${file}`),
		},
	};
}
