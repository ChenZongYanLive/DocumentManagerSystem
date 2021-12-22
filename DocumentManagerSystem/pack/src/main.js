import '@babel/polyfill'
import Vue from 'vue'
import vuetify from './plugins/vuetify'
import i18n from './i18n'
import { ValidationProvider, ValidationObserver, localize, extend } from 'vee-validate'
import { required } from 'vee-validate/dist/rules'
import tw from 'vee-validate/dist/locale/zh_TW.json'
import en from 'vee-validate/dist/locale/en.json'
import twVeeValidateDic from './locales/tw.json'
import enVeeValidateDic from './locales/en.json'
import App from './App.vue'
import router from './router'
import store from '../store/store'
import jquery from 'jquery'
import moment from 'moment'
import { isRoleOk } from './js/http/identity/IdentityRole'
import identity from './js/http/identity/Identity'
import { HttpManager } from './js/http/http-Manager'

window.$ = jquery
window.jQuery = jquery

Vue.config.devtools = false
Vue.config.productionTip = false
Vue.prototype.$moment = moment
Vue.prototype.$isRoleOk = isRoleOk
Vue.prototype.$identity = identity

HttpManager.Init()

// Install required rule.
extend('required', required)

localize({
  en: {
    messages: en.messages,
    names: enVeeValidateDic
  },
  tw: {
    messages: tw.messages,
    names: twVeeValidateDic
  }
})

Vue.component('ValidationProvider', ValidationProvider)
Vue.component('ValidationObserver', ValidationObserver)

let LOCALE = 'tw'
localize(LOCALE)

// A simple get/set interface to manage our locale in components.
// This is not reactive, so don't create any computed properties/watchers off it.
Object.defineProperty(Vue.prototype, 'locale', {
  get () {
    return LOCALE
  },
  set (val) {
    LOCALE = val
    localize(val)
  }
})

new Vue({
  router,
  store,
  i18n,
  vuetify,
  render: h => h(App)
}).$mount('#app')
