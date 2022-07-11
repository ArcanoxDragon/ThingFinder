import webpack, { Chunk, Module } from "webpack";

import { Configuration, PathData } from "webpack";
import { OutputDir, ProjectPath } from "./build/constants.js";
import { getEntryPoints } from "./build/getEntryPoints.js";
import { LibraryEntryPoints } from "./build/LibraryEntryPoints.js";
import { CompilationStatusPlugin } from "./build/plugins/CompilationStatusPlugin.js";

export const baseConfig: Configuration = {
    entry: getEntryPoints,
    context: ProjectPath("."),

    output: {
        clean: true,
        path: OutputDir,
        filename: "js/[name].js",
        assetModuleFilename: "[path][base][query]",
    },

    optimization: {
        runtimeChunk: {
            name: "webpack.runtime",
        },

        splitChunks: {
            cacheGroups: {
                defaultVendors: {
                    test: /[\\/]node_modules[\\/]/,
                    name: "vendor",
                    chunks: chunk => !(chunk.name in LibraryEntryPoints),
                },
            },
        },
    },

    plugins: [
        new CompilationStatusPlugin(),
    ],

    module: {
        rules: [
            // TypeScript files
            {
                test: /\.tsx?$/,
                use: [
                    {
                        loader: "ts-loader",
                        options: {
                            configFile: ProjectPath("src/tsconfig.json"),
                        },
                    },
                ],
            },

            // SCSS files
            {
                test: /\.scss$/,
                type: "asset/resource",
                use: {
                    loader: "sass-loader",
                },
                generator: {
                    filename({ filename }: PathData) {
                        return filename?.replace(/^styles\//, "css/").replace(/\.scss$/, ".css");
                    },
                },
            },

            // Expose modules as globals
            {
                test: /[\\/]node_modules[\\/]jquery[\\/]/,
                use: {
                    loader: "expose-loader",
                    options: {
                        exposes: ["$", "jQuery"],
                    },
                },
            }
        ],
    },
};