const merge = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = merge(common, {
  mode: 'development',
  watch: true,
  watchOptions: {
    ignored: /node_modules/
  },
  devtool: 'inline-source-map',
  devServer: {
    contentBase: './wwwroot/assets/',
  },
});
