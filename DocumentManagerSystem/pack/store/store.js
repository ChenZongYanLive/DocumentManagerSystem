import Vue from 'vue'
import Vuex from 'vuex'
import login from './login'

Vue.use(Vuex)
Vue.config.devtools = false

export default new Vuex.Store({
  modules: {
    login: login
  },
  strict: false,
  state: {
    isLogin: false
  },
  mutations: {
    setLoginStatus (state, isLogin) {
      state.isLogin = isLogin
    }
  }
})
