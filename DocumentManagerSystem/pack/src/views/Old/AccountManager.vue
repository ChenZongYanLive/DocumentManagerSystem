<template>
  <div>
    <v-layout align-start justify-start wrap>
      <div
        style="height: 100%;
                width: 100%;
                position: absolute;
                top: 0px;
                left: 0px;
                overflow-y:auto;
                background-color: #fafafa;"
      >
        <template>
          <v-container fluid grid-list-md>
            <v-layout wrap>
              <v-flex flex-grow-1 xs12 sm12 md12 lg12 x12>
                <v-card>
                  <v-tabs
                    v-model="tab"
                    background-color="deep-purple accent-4"
                    class="elevation-2"
                    dark
                  >
                    <v-tabs-slider></v-tabs-slider>

                    <v-tab :href="`#tab-1`">AD帳號</v-tab>
                    <v-tab :href="`#tab-2`">一般帳號</v-tab>

                    <v-tab-item value="tab-2">
                      <v-btn class="ma-1" color="primary" dark @click="AddAccount">
                        <v-icon left>create</v-icon>新增帳號
                      </v-btn>

                      <v-card flat tile>
                        <v-card-title>
                          <v-spacer></v-spacer>
                          <v-text-field
                            v-model="search"
                            append-icon="mdi-magnify"
                            label="搜尋"
                            single-line
                            hide-details
                          ></v-text-field>
                        </v-card-title>
                        <v-data-table
                          :loading="isLoading"
                          :headers="headers"
                          :items="UserList"
                          :search="search"
                          :items-per-page.sync="pagination.rowsPerPage"
                          :footer-props="{
                                                    'items-per-page-options': [10,15], // -> Add this example
                                                    'items-per-page-text':'每頁筆數'
                                                }"
                          :sort-by="pagination.sortBy"
                          class="elevation-1"
                        >
                          <template
                            v-slot:item.CreateTime="{ item }"
                          >{{ formatDate(item.CreateTime) }}</template>
                          <template
                            v-slot:item.Roles="{ item }"
                          >{{ item.Roles && item.Roles.length > 0 ? item.Roles[0].Name : '' }}</template>
                          <template v-slot:item.action="{ item }">
                            <v-btn small tile outlined color="success" @click="editAccount(item)">
                              <v-icon left>create</v-icon>修改權限
                            </v-btn>
                            <v-btn small tile outlined color="red" @click="deleteAccount(item)">
                              <v-icon left>delete</v-icon>刪除帳號
                            </v-btn>
                          </template>
                          <template slot="no-data">
                            <div style="text-align: center;">沒有資料</div>
                          </template>
                          <v-alert slot="no-results" :value="true" color="error" icon="warning">
                            {{
                            '沒有資料'
                            }}
                          </v-alert>
                        </v-data-table>
                      </v-card>
                      <v-card></v-card>
                    </v-tab-item>
                    <v-tab-item value="tab-1">
                      <v-card flat tile>
                        <v-card-title>
                          <v-spacer></v-spacer>
                          <v-text-field
                            v-model="searchAD"
                            append-icon="mdi-magnify"
                            label="搜尋"
                            single-line
                            hide-details
                          ></v-text-field>
                        </v-card-title>
                        <v-data-table
                          :loading="isLoading"
                          :headers="ADheaders"
                          :items="ADUserList"
                          :search="searchAD"
                          :items-per-page.sync="pagination.rowsPerPage"
                          :footer-props="{
                                                    'items-per-page-options': [10,15], // -> Add this example
                                                    'items-per-page-text':'每頁筆數'
                                                }"
                          :sort-by="pagination.sortBy"
                          class="elevation-1"
                        >
                          <template
                            v-slot:item.CreateTime="{ item }"
                          >{{ formatDate(item.CreateTime) }}</template>
                          <template
                            v-slot:item.Roles="{ item }"
                          >{{ item.Roles && item.Roles.length > 0 ? item.Roles[0].Rule : '' }}</template>
                          <template v-slot:item.action="{ item }">
                            <v-btn small tile outlined color="success" @click="editADAccount(item)">
                              <v-icon left>create</v-icon>修改權限
                            </v-btn>
                          </template>
                          <template slot="no-data">
                            <div style="text-align: center;">沒有資料</div>
                          </template>
                          <v-alert slot="no-results" :value="true" color="error" icon="warning">
                            {{
                            '沒有資料'
                            }}
                          </v-alert>
                        </v-data-table>
                      </v-card>
                      <v-card></v-card>
                    </v-tab-item>
                  </v-tabs>
                </v-card>
              </v-flex>
            </v-layout>
          </v-container>
        </template>
      </div>
    </v-layout>
    <v-dialog v-model="AddAccountDialog" hide-overlay width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">新增帳號</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="AddAccountForm" style="width:100%">
              <v-flex xs12>
                <ValidationProvider name="AccountName" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    v-model="AccountName"
                    :error-messages="errors[0]"
                    label="帳號名稱"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="Password" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    v-model="Password"
                    type="password"
                    :error-messages="errors[0]"
                    label="密碼"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="Role" rules="required" v-slot="{ errors }">
                  <v-select
                    :items="Roles"
                    item-text="Name"
                    item-value="Name"
                    v-model="SelectRole"
                    :label="'選擇權限'"
                    persistent-hint
                    :error-messages="errors[0]"
                  ></v-select>
                </ValidationProvider>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="primary" dark @click="SaveAccount">新增帳號</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="EditAccountDialog" hide-overlay width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">修改帳號</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="EditAccountForm" style="width:100%">
              <v-flex xs12>
                <ValidationProvider name="Role" rules="required" v-slot="{ errors }">
                  <v-select
                    :items="Roles"
                    item-text="Name"
                    item-value="Name"
                    v-model="SelectEditRole"
                    :label="'選擇權限'"
                    persistent-hint
                    :error-messages="errors[0]"
                  ></v-select>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <v-checkbox
                  v-model="IsChangePassword"
                  label="修改密碼"
                  required
                ></v-checkbox>
                <template v-if="IsChangePassword">
                  <ValidationProvider name="Password" rules="required" v-slot="{ errors }">
                    <v-text-field
                      dense
                      v-model="NewPassword"
                      type="password"
                      :error-messages="errors[0]"
                      label="密碼"
                    ></v-text-field>
                  </ValidationProvider>
                </template>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="primary" dark @click="SaveAccountRole">存檔</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="EditADAccountDialog" hide-overlay width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">修改AD帳號權限</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="EditADAccountForm" style="width:100%">
              <v-flex xs12>
                <ValidationProvider name="Role" rules="required" v-slot="{ errors }">
                  <v-select
                    :items="Roles"
                    item-text="Name"
                    item-value="Name"
                    v-model="SelectEditADRole"
                    :label="'選擇權限'"
                    persistent-hint
                    :error-messages="errors[0]"
                  ></v-select>
                </ValidationProvider>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="primary" dark @click="SaveADAccountRole">存檔</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import axios from 'axios'
// import moment from 'moment'
export default {
  components: {},
  data () {
    return {
      search: '',
      searchAD: '',
      isLoading: false,
      AddAccountDialog: false,
      EditAccountDialog: false,
      EditADAccountDialog: false,
      ADUserList: [],
      UserList: [],
      AccountRules: [],
      pagination: {
        rowsPerPage: -1,
        descending: false
      },
      ADheaders: [
        {
          text: '帳號',
          align: 'start',
          sortable: false,
          value: 'Account.AccountName'
        },
        {
          text: '使用者',
          align: 'start',
          sortable: false,
          value: 'Account.FullName'
        },
        {
          text: '權限',
          align: 'start',
          sortable: false,
          value: 'Roles'
        },
        {
          text: '動作',
          align: 'start',
          sortable: false,
          value: 'action'
        }
      ],
      headers: [
        {
          text: '帳號',
          align: 'start',
          sortable: false,
          value: 'Account.UserName'
        },
        {
          text: '權限',
          align: 'start',
          sortable: false,
          value: 'Roles'
        },
        {
          text: '動作',
          align: 'start',
          sortable: false,
          value: 'action'
        }
      ],
      Roles: [],
      SelectRole: null,
      AccountName: '',
      Password: '',
      EditAccountName: '',
      SelectEditRole: '',
      IsChangePassword: false,
      NewPassword: '',
      EditADAccountName: '',
      SelectEditADRole: '',
      tab: 0
    }
  },
  created: function () {},
  mounted () {
    this.GetAdUser()
    this.GetUser()
    this.GetRoles()
  },
  directives: {},
  methods: {
    GetAdUser: function () {
      let self = this
      axios
        .get('/api/account/GetADUser', {
          params: {}
        })
        .then((response) => {
          self.ADUserList = response.data
          self.ADUserList.splice(0, 0)
        })
    },
    GetUser: function () {
      let self = this
      axios
        .get('/api/account/GetUsers', {
          params: {}
        })
        .then((response) => {
          self.UserList = response.data
        })
    },
    GetRoles: function () {
      let self = this
      axios
        .get('/api/account/GetRoles', {
          params: {}
        })
        .then((response) => {
          self.Roles = response.data
        })
    },
    AddAccount: function () {
      this.SelectRole = null
      this.AccountName = ''
      this.Password = ''
      this.AddAccountDialog = true
    },
    SaveAccount: async function () {
      let self = this

      const isValid = await self.$refs.AddAccountForm.validate()

      if (!isValid) return

      let accountData = {
        AccountName: self.AccountName,
        Password: self.Password,
        Role: self.SelectRole
      }

      axios.post('/api/account/AddAccount', accountData).then((response) => {
        if (response.data * 1 === 1000) {
        } else if (response.data * 1 === 1001) {
        }
        self.AddAccountDialog = false
        self.GetUser()
      })
    },
    editAccount: function (accountInfo) {
      let self = this
      self.IsChangePassword = false
      self.NewPassword = ''
      self.EditAccountName = accountInfo.Account.UserName
      self.SelectEditRole = accountInfo.Roles
        ? accountInfo.Roles[0].Name
        : null
      self.EditAccountDialog = true
    },
    editADAccount: function (accountInfo) {
      let self = this

      self.EditADAccountName = accountInfo.Account.AccountName
      self.SelectEditADRole =
        accountInfo.Roles && accountInfo.Roles.length > 0
          ? accountInfo.Roles[0].Rule
          : null
      self.EditADAccountDialog = true
    },
    SaveAccountRole: async function () {
      let self = this

      const isValid = await self.$refs.EditAccountForm.validate()

      if (!isValid) return

      let accountData = {
        AccountName: self.EditAccountName,
        Role: self.SelectEditRole
      }

      if (self.IsChangePassword) {
        accountData.Password = self.NewPassword
      }

      axios.post('/api/account/SetAccount', accountData).then((response) => {
        if (response.data * 1 === 1000) {
        } else if (response.data * 1 === 1001) {
        }
        self.EditAccountDialog = false
        self.GetUser()
      })
    },
    SaveADAccountRole: async function () {
      let self = this

      const isValid = await self.$refs.EditADAccountForm.validate()

      if (!isValid) return

      let accountData = {
        AccountName: self.EditADAccountName,
        Role: self.SelectEditADRole
      }
      axios.post('/api/account/SetADAccount', accountData).then((response) => {
        if (response.data * 1 === 1000) {
        } else if (response.data * 1 === 1001) {
        }
        self.EditADAccountDialog = false
        self.GetAdUser()
      })
    },
    deleteAccount: function (accountInfo) {
      let self = this
      if (confirm('確定要刪除此帳號嗎 ? ')) {
        let accountData = {
          AccountName: accountInfo.Account.UserName
        }
        axios
          .post('/api/account/DeleteAccount', accountData)
          .then((response) => {
            if (response.data * 1 === 1000) {
            } else if (response.data * 1 === 1001) {
            }
            self.EditAccountDialog = false
            self.GetUser()
          })
      }
    }
  },
  computed: {
    Language () {
      return 'zh-TW'
    }
  },
  watch: {}
}
</script>
