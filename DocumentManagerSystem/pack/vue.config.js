// const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
  // baseUrl: 'dist',
  transpileDependencies: [/node_modules[/\\\\]vuetify[/\\\\]/],
  publicPath: process.env.CORDOVA_PLATFORM ? '' : '/v1',
  outputDir: '../wwwroot/v1',
  indexPath: 'index.html',
  chainWebpack: config => {
    config.entry('polyfill').add('@babel/polyfill')
    config.module.rule('eslint').use('eslint-loader').options({
      fix: true
    })
  },
  devServer: {
    https: false
  },
  configureWebpack: {
    // name:'EGK',
    devtool: 'source-map'
    /*
    ,plugins: [
      new HtmlWebpackPlugin({ //根据模板插入css/js等生成最终HTML
        title:'EGK',
        inject: 'body',
        //template:'../wwwroot/index.html',
        //favicon: '../wwwroot/img/egk_logo.ico',//../wwwroot/img/egk_logo.png
        minify: { //压缩HTML文件
          removeComments: true, //移除HTML中的注释
          collapseWhitespace: false //删除空白符与换行符
        }
      })
    ] */
  },
  filenameHashing: false,
  pluginOptions: {
    i18n: {
      locale: 'en',
      fallbackLocale: 'en',
      localeDir: 'locales',
      enableInSFC: false
    },
    cordovaPath: 'src-cordova'
  }
}
