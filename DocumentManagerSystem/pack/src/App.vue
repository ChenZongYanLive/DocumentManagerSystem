<template>
  <div id="app">
    <v-app id="inspire">
      <v-navigation-drawer
        v-model="drawer"
        :clipped="$vuetify.breakpoint.lgAndUp"
        app
      >
      </v-navigation-drawer>

      <v-app-bar
        :clipped-left="$vuetify.breakpoint.lgAndUp"
        app
        color="blue darken-3"
        dark
      >
        <v-toolbar-title style="width: 300px" class="ml-0 pl-3">
          <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
          <span class="hidden-sm-and-down">文管系統</span>
        </v-toolbar-title>
        <v-spacer></v-spacer>
        帳戶: {{ account }}
        <v-chip color="cyan" class="ml-1" label text-color="white" v-on:click="Logout">登出</v-chip>
      </v-app-bar>
      <v-content>
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
