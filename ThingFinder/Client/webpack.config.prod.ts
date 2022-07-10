import { Configuration } from "webpack";

export const prodConfig: Configuration = {
    mode: "production",
    devtool: false,

    output: {
        chunkFilename: "chunks/[id].[chunkhash].js",
    },

    module: {
        rules: [
            {
                test: /\.scss$/,
                use: {
                    loader: "sass-loader",
                    options: {
                        sassOptions: {
                            outputStyle: "compressed",
                        },
                    },
                },
            }
        ],
    },
};