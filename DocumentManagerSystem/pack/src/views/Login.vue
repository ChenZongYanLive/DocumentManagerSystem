<template>
  <v-container fluid fill-height>
      <v-layout align-center justify-center>
          <v-flex xs12 sm6 md6 lg5>
              <v-layout justify-center class="display-2 font-weight-bold blue--text text--darken-3">
                <p class="font-weight-black">桃園市政府地方稅務局</p>
              </v-layout>
              <v-layout justify-center class="display-2 font-weight-bold blue--text text--darken-2">
                <p class="font-weight-black">ISMS 文件管理系統</p>
              </v-layout>
              <v-spacer class="mt-4"></v-spacer>
              <v-spacer></v-spacer>
              <v-form class="ml-4 mr-4">
                  <ValidationObserver ref="form">
                      <ValidationProvider name="account" rules="required" v-slot="{ errors }">
                      <v-text-field v-model="account"
                                      prepend-icon="person"
                                      :placeholder="$t('帳號')"
                                      type="text"
                                      @keyup.enter="submit"
                                      :error-messages="errors[0]"
                                      color="#2DA7E0"
                                      required></v-text-field>
                      </ValidationProvider>
                      <ValidationProvider name="password" rules="required" v-slot="{  }">
                      <v-text-field id="password"
                                      v-model="password"
                                      prepend-icon="lock"
                                      :placeholder="$t('密碼')"
                                      type="password"
                                      @keyup.enter="submit"
                                      :error-messages="'同開機帳密'"
                                      color="#2DA7E0"
                                      required></v-text-field>
                      </ValidationProvider>
                  </ValidationObserver>
              </v-form>
              <v-spacer></v-spacer>
              <v-spacer></v-spacer>
              <v-layout justify-center>
                  <v-flex xs12 sm12 md12 lg12 style="padding-left: 55px;padding-right: 22px;">
                      <v-btn class="mt-4" block rounded dark color="#2DA7E0" :disabled="!valid" v-on:click="submit">{{ $t('登入') }}</v-btn>
                  </v-flex>
              </v-layout>
              <v-layout justify-center>
                  <v-flex xs12 sm12 md12 lg12 style="padding-left: 55px;padding-right: 22px;">
                      <v-alert
                          :value="alert"
                          color="warning"
                          icon="priority_high"
                          outlined
                          >
                          {{$t('登入失敗請確認您的帳號密碼')}}
                      </v-alert>
                  </v-flex>
              </v-layout>
          </v-flex>
      </v-layout>
  </v-container>
</template>
<script>
import axios from 'axios'

export default {
  data () {
    return {
      isShowLoginDialog: true,
      drawer: null,
      valid: true,
      isBackendValidFail: false,
      resultCode: 0,
      account: '',
      password: '',
      alert: false
    }
  },
  created: function () {
  },
  methods: {
    async submit () {
      let self = this

      const isValid = await self.$refs.form.validate()
      if (isValid) {
        // Native form submission is not yet supported
        let data = {
          Account: self.account,
          Password: self.password,
          RememberMe: false
        }

        let postConfig = {
          headers: {
            'Access-Control-Allow-Origin': window.location.origin,
            'content-type': 'application/json'
          }
        }
        axios.post('/api/account/login', data, postConfig)
          .then((response) => {
            self.current = 'response : ' + JSON.stringify(response)

            if (response.data.Status.Code === 1000) {
              self.$store.commit('setLoginStatus', true)
              self.$store.commit('setRoles', JSON.stringify(response.data.Data.Value.Roles))
              self.$store.commit('setJwt', response.data.Data.Value.JWT)

              self.$router.push('/StageDocumentSearch')
            } else {
              // TODO handle error
              self.alert = true
              setTimeout(() => {
                self.alert = false
              }, 3000)
            }
          })
          .catch(function (error) {
            console.error('login   error:\n' + error)
          })
          .finally(function () {

          })
      }
    }
  }
}
</script>
