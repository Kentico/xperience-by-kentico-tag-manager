const TerserPlugin = require("terser-webpack-plugin");
const path = require("path");

const isProduction = process.env.NODE_ENV == "production";

const config = {
    entry: "./src/index.js",
    output: {
        path: path.resolve(__dirname, "../.././wwwroot/scripts"),
        filename: "ktc-tagmanager.js",
        module: true,
    },
    experiments: {
        outputModule: true,
    },

    module: {
        rules: [
            {
                test: /\.(js|jsx)$/i,
                loader: "babel-loader",
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2|png|jpg|gif)$/i,
                type: "asset",
            },
        ],
    },
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin()],
    },
};

module.exports = () => {
    if (isProduction) {
        config.mode = "production";
    } else {
        config.mode = "development";
    }
    return config;
};