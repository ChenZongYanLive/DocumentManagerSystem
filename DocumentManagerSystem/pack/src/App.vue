<template>
  <div id="app">
    <v-app id="inspire">
      <v-navigation-drawer
        v-model="drawer"
        v-if="isLogin"
        :clipped="$vuetify.breakpoint.lgAndUp"
        app
      >
        <v-list dense>
          <!-- <v-list-group
            sub-group
            no-action
            >
                <template v-slot:activator>
                    <v-list-item-content>
                        <v-list-item-title>{{ $t('ISMS文件查詢') }}</v-list-item-title>
                    </v-list-item-content>
                </template>
          </v-list-group>-->

          <v-list-item
            color="success"
            :to="{ path: '/StageDocumentSearch' }"
            v-if="$isRoleOk($identity.StageDocumentSearch)"
          >
            <v-list-item-action>
              <v-icon>home</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('ISMS文件查詢') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentSearch?id=1&L1' }"
            v-if="$isRoleOk($identity.StageDocumentSearch)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L1 - 資訊安全政策') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentSearch?id=2&L2' }"
            v-if="$isRoleOk($identity.StageDocumentSearch)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L2 - 程序書') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentSearch?id=3&L3' }"
            v-if="$isRoleOk($identity.StageDocumentSearch)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L3 - 說明書') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentSearch?id=4&L4' }"
            v-if="$isRoleOk($identity.StageDocumentSearch)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L4 - 各項表單') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="success"
            :to="{ path: '/DocumentRecordSearch' }"
            v-if="$isRoleOk($identity.DocumentRecordSearch)"
          >
            <v-list-item-action>
              <v-icon>search</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('紀錄文件查詢') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="success"
            :to="{ path: '/StageDocumentManagement' }"
            v-if="$isRoleOk($identity.StageDocumentManagement)"
          >
            <v-list-item-action>
              <v-icon>insert_drive_file</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('管理文件') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentManagement?id=1&L1' }"
            v-if="$isRoleOk($identity.StageDocumentManagement)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L1 - 資訊安全政策') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentManagement?id=2&L2' }"
            v-if="$isRoleOk($identity.StageDocumentManagement)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L2 - 程序書') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentManagement?id=3&L3' }"
            v-if="$isRoleOk($identity.StageDocumentManagement)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L3 - 說明書') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item
            color="blue"
            class="pl-12"
            :to="{ path: '/StageDocumentManagement?id=4&L4' }"
            v-if="$isRoleOk($identity.StageDocumentManagement)"
          >
            <v-list-item-action>
              <v-icon>folder</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('L4 - 各項表單') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="success"
            :to="{ path: '/DocumentRecordManagement' }"
            v-if="$isRoleOk($identity.DocumentRecordManagement)"
          >
            <v-list-item-action>
              <v-icon>assignment</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('管理紀錄文件') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="success"
            :to="{ path: '/OtherDocumentManagement' }"
          >
            <v-list-item-action>
              <v-icon>assignment</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('機關外來文件') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="success"
            :to="{ path: '/DocumentLibrary' }"
            v-if="$isRoleOk($identity.DocumentLibrary)"
          >
            <v-list-item-action>
              <v-icon>menu_book</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('文件資料庫') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="mx-3"></v-divider>

          <v-list-item
            color="success"
            :to="{ path: '/AccountManager' }"
            v-if="$isRoleOk($identity.AccountManager)"
          >
            <v-list-item-action>
              <v-icon>account_circle</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>{{ $t('權限管理') }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-navigation-drawer>

      <v-app-bar
        :clipped-left="$vuetify.breakpoint.lgAndUp"
        app
        color="blue darken-3"
        dark
        v-if="isLogin"
      >
        <v-toolbar-title style="width: 300px" class="ml-0 pl-3">
          <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
          <span class="hidden-sm-and-down">文管系統</span>
        </v-toolbar-title>
        <v-spacer></v-spacer>
        <!-- <v-btn
            icon
            large
            v-on:click="Logout">
            <v-avatar
            size="32px"
            item
            >
            <v-icon>logout</v-icon>
            </v-avatar>
        </v-btn>-->
        帳戶: {{ account }}
        <v-chip color="cyan" class="ml-1" label text-color="white" v-on:click="Logout">登出</v-chip>
      </v-app-bar>
      <v-content v-if="isLogin">
        <v-container fluid fill-height>
          <v-layout align-center justify-center>
            <router-view />
          </v-layout>
        </v-container>
      </v-content>

      <router-view name="login"></router-view>
    </v-app>
  </div>
</template>

<script>
// import { HttpAccount } from '../src/js/http/http-Manager'

export default {
  name: 'App',
  components: {},
  data: () => ({
    dialog: false,
    drawer: null,
    account: ''
  }),
  created: function () {},
  async mounted () {
    if (this.isLogin) {
      await this.GetAccountName()
    }
  },
  directives: {},
  methods: {
    Logout: function () {
      let self = this
      try {
        const logoutAction = this.$store.state.login.Jwt
          ? Promise.resolve()
          : Promise.resolve()

        return logoutAction.then(() => {
          self.$store.commit('setLoginStatus', false)
          self.$store.commit('setRoles', [])
          self.$store.commit('setJwt', null)
          self.$router.push({ name: 'login' })
        })
      } catch (error) {}
    },
    async GetAccountName () {
      if (this.$store.state.login.Jwt) {
        let jwt = await this.$store.dispatch('parseJwt')
        this.account = jwt['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      }
    }
  },
  computed: {
    Language () {
      return 'zh-TW'
    },
    isLogin () {
      return this.$store.state.isLogin
    }
  },
  watch: {
    isLogin: async function () {
      if (this.isLogin) {
        await this.GetAccountName()
      }
    }
  }
}
</script>
