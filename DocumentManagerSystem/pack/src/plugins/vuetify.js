// import '@mdi/font/css/materialdesignicons.css' // Ensure you are using css-loader
import 'material-design-icons-iconfont/dist/material-design-icons.css' // Ensure you are using css-loader
import Vue from 'vue'
import Vuetify, { VLayout } from 'vuetify/lib'
// import 'vuetify/src/stylus/app.styl'

// Vue.use(Vuetify, {
//   iconfont: 'md',
//   components: {
//     VLayout
//   }
// })

Vue.use(Vuetify)

export default new Vuetify({
  icons: {
    iconfont: 'md'
  },
  components: {
    VLayout
  }
})
