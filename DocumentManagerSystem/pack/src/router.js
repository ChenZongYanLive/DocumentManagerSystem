import Vue from 'vue'
import Router from 'vue-router'
import i18n from './i18n'
import store from '../store/store'

import StageDocumentManagement from './views/StageDocumentManagement.vue'
import DocumentRecordManagement from './views/DocumentRecordManagement.vue'
import StageDocumentSearch from './views/StageDocumentSearch.vue'
import AccountManager from './views/AccountManager.vue'
import DocumentRecordSearch from './views/DocumentRecordSearch.vue'
import login from './views/Login.vue'
import DocumentLibrary from './views/DocumentLibrary.vue'
import OtherDocumentManagement from './views/OtherDocumentManagement.vue'

Vue.use(Router)

const router = new Router({
  mode: process.env.CORDOVA_PLATFORM ? 'hash' : 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/StageDocumentSearch',
      name: 'StageDocumentSearch',
      alias: '/',
      components: {
        default: StageDocumentSearch
      },
      meta: {
        title: i18n.t('L1 ~ L4 文件查詢')
      }
    },
    {
      path: '/DocumentRecordSearch',
      name: 'DocumentRecordSearch',
      alias: '/',
      components: {
        default: DocumentRecordSearch
      },
      meta: {
        title: i18n.t('L1 ~ L4 文件查詢')
      }
    },
    {
      path: '/StageDocumentManagement',
      name: 'StageDocumentManagement',
      components: {
        default: StageDocumentManagement
      },
      meta: {
        title: i18n.t('L1 ~ L4 文件管理')
      }
    },
    {
      path: '/DocumentRecordManagement',
      name: 'DocumentRecordManagement',
      components: {
        default: DocumentRecordManagement
      },
      meta: {
        title: i18n.t('紀錄文件管理')
      }
    },
    {
      path: '/AccountManager',
      name: 'AccountManager',
      alias: '/',
      components: {
        default: AccountManager
      },
      meta: {
        title: i18n.t('權限管理')
      }
    },
    {
      path: '/login',
      name: 'login',
      components: {
        default: StageDocumentSearch,
        login: login
      },
      meta: {
        title: i18n.t('login'),
        permission: null
      }
    },
    {
      path: '/DocumentLibrary',
      name: 'DocumentLibrary',
      components: {
        default: DocumentLibrary
      },
      meta: {
        title: i18n.t('文件資料庫'),
        permission: null
      }
    },
    {
      path: '/OtherDocumentManagement',
      name: 'OtherDocumentManagement',
      components: {
        default: OtherDocumentManagement
      },
      meta: {
        title: i18n.t('機關外來文件'),
        permission: null
      }
    }
  ]
})

router.beforeEach((to, from, next) => {
  let identity = window.localStorage.getItem('Jwt')
  store.commit('setJwt', identity)

  if (!identity) {
    store.commit('setLoginStatus', false)
    if (to.path !== '/login') {
      return next({ path: '/login' })
    } else {
      next()
    }
  } else {
    store.commit('setLoginStatus', true)
    if (to.meta.title) {
      document.title = to.meta.title
    }
    next()
  }
})

export default router
