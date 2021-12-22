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
              <v-btn class="ml-1" color="primary" dark @click.stop="addDialog = true">新增</v-btn>
              <v-flex flex-grow-1 xs12 sm12 md12 lg12 x12>
                <v-card>
                  <v-data-table
                    :loading="isLoading"
                    :headers="headers"
                    :items="DocumentLibList"
                    :items-per-page.sync="pagination.rowsPerPage"
                    :footer-props="{
                                        'items-per-page-options': [20, 30], // -> Add this example
                                        'items-per-page-text':'每頁筆數'
                                    }"
                    class="elevation-1"
                    dense
                  >
                    <template v-slot:item.action="{ item }">
                      <v-btn small tile outlined color="success" @click="edit(item)">
                        <v-icon left>create</v-icon>修改
                      </v-btn>
                      <v-btn
                        small
                        tile
                        outlined
                        color="red"
                        @click="deleteDocument(item)"
                        v-if="$isRoleOk($identity.DeleteDocument)"
                      >
                        <v-icon left>delete</v-icon>刪除
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
              </v-flex>
            </v-layout>
          </v-container>
        </template>
      </div>
    </v-layout>
    <v-dialog v-model="addDialog" hide-overlay persistent width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">新增文件</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="addForm" style="width:100%">
              <v-flex xs12>
                <ValidationProvider name="DocumentName" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    v-model="DocumentName"
                    :error-messages="errors[0]"
                    label="文件名稱"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentNumber" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    v-model="DocumentNumber"
                    :error-messages="errors[0]"
                    label="文件編號"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentOwner" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    v-model="DocumentOwner"
                    :error-messages="errors[0]"
                    label="權責人員"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="addDialog = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click="addDocument">存檔</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="editDialog" hide-overlay persistent width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">修改文件</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="EditForm" style="width:100%">
              <v-flex xs12>
                <ValidationProvider name="DocumentName" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentName"
                    label="文件名稱"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentNumber" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentNumber"
                    label="文件編號"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentOwner" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentOwner"
                    label="權責人員"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="editDialog = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click="setDocument">存檔</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="IsOpenAlertDialog" width="500">
      <v-card>
        <v-alert text prominent type="error" class="mb-0">{{AlertDialogMessage}}</v-alert>
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
      addDialog: false,
      editDialog: false,
      isLoading: false,
      pagination: {
        rowsPerPage: -1,
        descending: false
      },
      headers: [
        {
          text: '文件編號',
          align: 'start',
          sortable: false,
          value: 'DocumentNumber'
        },
        {
          text: '文件名稱',
          align: 'start',
          sortable: false,
          value: 'DocumentName'
        },
        {
          text: '權責人員',
          align: 'start',
          sortable: false,
          value: 'DocumentOwner'
        },
        {
          text: '動作',
          align: 'start',
          sortable: false,
          value: 'action'
        }
      ],
      DocumentList: [],
      DocumentLibList: [],
      DocumentName: '',
      DocumentNumber: '',
      DocumentOwner: '',
      Edit: {
        Id: -1,
        DocumentName: '',
        DocumentNumber: '',
        DocumentOwner: ''
      },
      IsOpenAlertDialog: false,
      AlertDialogMessage: ''
    }
  },
  created: function () {},
  mounted () {
    this.GetDocumentList()
  },
  directives: {},
  methods: {
    edit: function (item) {
      this.Edit.Id = item.Id
      this.Edit.DocumentName = item.DocumentName
      this.Edit.DocumentNumber = item.DocumentNumber
      this.Edit.DocumentOwner = item.DocumentOwner
      this.editDialog = true
    },
    addDocument: async function () {
      let self = this

      const isValid = await self.$refs.addForm.validate()

      if (!isValid) return

      let documentName = this.DocumentName
      let documentNumber = this.DocumentNumber
      let documentOwner = this.DocumentOwner

      let checkIsSameDocumentNumberResponse = await axios.post('/api/Document/CheckIsSameDocumentlibNumber', { DocumentNumber: documentNumber })
      let isSameDocumentNumber = false

      isSameDocumentNumber = checkIsSameDocumentNumberResponse.data

      if (isSameDocumentNumber) {
        self.AlertDialogMessage = '文件編號重複，請重新輸入!!'
        self.IsOpenAlertDialog = true
        setTimeout(() => {
          self.IsOpenAlertDialog = false
        }, 1000)
        return
      }

      axios.post('/api/Document/AddDocumentIndex', {
        DocumentName: documentName,
        DocumentNumber: documentNumber,
        DocumentOwner: documentOwner
      }).then((response) => {
        self.GetDocumentList()
      }).finally(function () {
        self.addDialog = false
      })
    },
    setDocument: async function () {
      let self = this

      const isValid = await self.$refs.EditForm.validate()

      if (!isValid) return

      let checkIsSameDocumentNumberResponse = await axios.post(`/api/Document/CheckIsSameDocumentlibNumber/${self.Edit.Id}`, { DocumentNumber: self.Edit.DocumentNumber })
      let isSameDocumentNumber = false

      isSameDocumentNumber = checkIsSameDocumentNumberResponse.data

      if (isSameDocumentNumber) {
        self.AlertDialogMessage = '文件編號重複，請重新輸入!!'
        self.IsOpenAlertDialog = true
        setTimeout(() => {
          self.IsOpenAlertDialog = false
        }, 1000)
        return
      }

      axios.post('/api/Document/SetDocumentIndex', self.Edit).then((response) => {
        self.GetDocumentList()
      }).finally(function () {
        self.editDialog = false
      })
    },
    deleteDocument: function (item) {
      if (confirm('確定要刪除此文件嗎 ? ')) {
        let self = this
        axios.post('/api/Document/DeleteDocumentIndex', item).then((response) => {
          self.GetDocumentList()
        })
      }
    },
    GetDocumentList: function () {
      let self = this
      self.isLoading = true
      // console.log(self.DocumentLibList)
      axios.get('/api/Document/GetDocumentIndex').then((response) => {
        self.DocumentLibList = response.data
        // console.log(self.DocumentLibList)
      })
        .finally(() => {
          self.isLoading = false
        })
    }
  },
  computed: {
  },
  watch: {
  }
}
</script>
