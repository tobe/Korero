const ExtractTextPlugin = require('extract-text-webpack-plugin');
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const path = require('path');

const ROOT = path.resolve(__dirname);
console.log('::: Webpack started :::');

module.exports = {
    devtool: 'cheap-module-eval-source-map',
    entry: {
        'app': './ClientApp/main.ts',
        'vendor': './ClientApp/vendors.ts',
        'polyfills': './ClientApp/polyfills.ts'
    },
    performance: {
        hints: false
    },
    resolve: {
        extensions: ['.ts', '.js', '.json']
    },
    output: {
        path: ROOT + '/wwwroot/',
        filename: 'dist/[name].bundle.js',
        chunkFilename: 'dist/[id].chunk.js',
        publicPath: '/'
    },

    module: {
        rules: [
            {
                test: /\.ts$/,
                use: [
                    'awesome-typescript-loader',
                    'angular-router-loader',
                    'angular2-template-loader',
                    'source-map-loader',
                    'tslint-loader'
                ]
            },
            {
                test: /\.html$/,
                use: 'html-loader'
            },
            {
                test: /\.(png|jpg|gif|woff|woff2|ttf|svg|eot)$/,
                use: 'file-loader?name=assets/[name].[ext]',
            },
            {
                test: /favicon.ico$/,
                use: 'file-loader?name=/[name].[ext]'
            },
            // Use raw-loader for Angular's styles
            {
                test: /\.css$/,
                include: path.join(ROOT, 'ClientApp/app'),
                use: [
                    'raw-loader',
                    //'style-loader',
                    //'css-loader',
                ]
            },
            // Use a normal loader for global CSS (styles.css)
            {
                test: /\.css$/,
                exclude: path.join(ROOT, 'ClientApp/app'),
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: 'css-loader'
                })
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin('dist/[name].bundle.css'),
        new webpack.optimize.ModuleConcatenationPlugin(),
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills']
        }),
        new CleanWebpackPlugin(
            [
                './wwwroot/dist',
                './wwwroot/assets'
            ],
            { root: ROOT }
        ),
        new webpack.ContextReplacementPlugin(
            /angular(\\|\/)core(\\|\/)@angular/,
            path.resolve(__dirname, '../src')
        ),
        new webpack.ProvidePlugin({
            jQuery: 'jquery',
            $: 'jquery',
            jquery: 'jquery'
        }),
        new CopyWebpackPlugin([
            { from: './ClientApp/assets/*.*', to: 'assets/', flatten: true }
        ]),
    ],
    devServer: {
        historyApiFallback: true,
        stats: 'minimal'
    }
};
