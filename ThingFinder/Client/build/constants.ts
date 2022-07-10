import path from "path";

import { fileURLToPath } from "url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export const ProjectPath: typeof path.join = path.join.bind(this, __dirname, "..");

export const ScriptsDir = ProjectPath("src/scripts");
export const StylesDir = ProjectPath("styles");
export const OutputDir = ProjectPath("../wwwroot");