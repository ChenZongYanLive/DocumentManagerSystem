import axios from 'axios'

const state = {
  profile: {},
  Roles: [],
  Jwt: null,
  LoginTime: null
}
const getters = {

}
const actions = {
// Login methods. Either use cookie-based auth or jwt-based auth
  loginToken ({ commit, dispatch }, credentials) {
    let postConfig = {
      headers: {
        'Access-Control-Allow-Origin': window.location.origin,
        'content-type': 'application/json'
      }
    }

    return axios.post('/api/account/login', credentials, postConfig).then(res => {
      const profile = res.data
      const Jwt = res.data.token
      delete profile.token
      commit('setProfile', profile)
      commit('setJwt', Jwt)
      commit('setLoginStatus', true)
      dispatch('connectToTrackHub')
    })
  },
  logout ({ commit, state, dispatch }) {
    const logoutAction = state.Jwt
      ? Promise.resolve()
      : axios.post('/api/account/logout')

    return logoutAction.then(() => {
      commit('setProfile', {})
      commit('setJwt', null)
      commit('setLoginStatus', false)
      dispatch('stopTrackHub')
    })
  },
  parseJwt ({ commit, state, dispatch }) {
    var base64Url = state.Jwt.split('.')[1]
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
    }).join(''))

    return JSON.parse(jsonPayload)
  }
}
const mutations = {
  setJwt (state, Jwt) {
    state.Jwt = Jwt
    if (Jwt) window.localStorage.setItem('Jwt', Jwt)
    else window.localStorage.removeItem('Jwt')
  },
  setRoles (state, roles) {
    state.Roles = roles
    if (roles) window.localStorage.setItem('roles', roles)
    else window.localStorage.removeItem('roles')
  }
}

export default {
  state,
  getters,
  actions,
  mutations
}
