import axios from 'axios'
import store from '../../../store/store'
import router from '../../router'

export const HttpManager = {
  Init: function () {
    axios.defaults.baseURL = ''
    axios.defaults.withCredentials = true
    axios.defaults.headers['Pragma'] = 'no-cache'

    // Include the Authentication header when using JWT authentication
    axios.interceptors.request.use(request => {
      if (store.state.login.Jwt) {
        request.headers['Authorization'] = 'Bearer ' + store.state.login.Jwt // store.state.login.Jwt
      }
      return request
    })

    // 異常處理
    axios.interceptors.response.use(
      response => {
        return response
      },
      err => {
        console.log(err)
        if (err && err.response) {
          switch (err.response.status) {
            case 401:
              console.log('return login')
              store.commit('setLoginStatus', false)
              store.commit('setRoles', [])
              store.commit('setJwt', null)
              router.push({ name: 'login' })
              break
            case 404:
              console.log('找不到該頁面')
              break
            case 500:
              console.log('伺服器出錯')
              break
            case 503:
              console.log('服務失效')
              break
            default:
              console.log(`連接錯誤${err.response.status}`)
          }
        } else {
          console.log('連接到服務器失敗')
        }
        return Promise.resolve(err.response)
      }
    )
  },
  GetBaseUrl: function () {
    return axios.defaults.baseURL
  }
}

export function get (url, params = {}) {
  return new Promise((resolve, reject) => {
    axios
      .get(url, {
        params: params
      })
      .then(response => {
        resolve(response)
      })
      .catch(err => {
        reject(err)
      })
  })
}

export function post (url, data = {}, cancelToken = null, responseType = '') {
  return new Promise((resolve, reject) => {
    axios.post(url, data, {
      headers: {
        'content-type': 'application/json'
      },
      cancelToken: cancelToken,
      responseType: responseType
    }).then(
      response => {
        resolve(response)
      },
      err => {
        reject(err)
      }
    )
  })
}

export const HttpAccount = {
  Login: function (paramObj) {
    return post('/api/account/login', paramObj)
  },
  Logout: function (paramObj) {
    return post('/api/account/login', paramObj)
  },
  RefreshToken: function (paramObj) {
    return post('/api/account/RefreshToken', paramObj)
  },
  GetADUser: function (param) {
    return get('/api/account/GetADUser', param)
  }
}
