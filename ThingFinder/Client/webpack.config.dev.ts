import { Configuration } from "webpack";

export const devConfig: Configuration = {
    mode: "development",
    devtool: "eval-source-map",

    output: {
        chunkFilename: "chunks/[name].js",
    },

    module: {
        rules: [
            {
                test: /\.scss$/,
                use: {
                    loader: "sass-loader",
                    options: {
                        sourceMap: false,
                        sassOptions: {
                            sourceMapContents: true,
                            sourceMapEmbed: true,
                        },
                    },
                },
            }
        ],
    },
};