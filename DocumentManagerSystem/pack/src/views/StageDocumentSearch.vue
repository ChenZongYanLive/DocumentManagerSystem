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
                  <v-card-title class="blue light-2 white--text">
                    篩選
                    <v-btn tile color="success" style="position: absolute;right: 10px;" @click="cleanFilter">
                      <v-icon>filter_list</v-icon> 重置篩選條件
                    </v-btn>
                  </v-card-title>
                  <v-container>
                    <v-layout row wrap>
                      <v-flex xs2>
                        <v-text-field
                          v-model="filter.DocumentName"
                          @change="GetDocumentList"
                          name="DocumentName"
                          label="文件名稱"
                        ></v-text-field>
                      </v-flex>
                      <v-flex xs2>
                        <v-text-field
                          v-model="filter.DocumentNumber"
                          @change="GetDocumentList"
                          name="DocumentNumber"
                          label="文件編號"
                        ></v-text-field>
                      </v-flex>
                      <v-flex xs2>
                        <v-text-field
                          v-model="filter.DocumentVersion"
                          @change="GetDocumentList"
                          name="DocumentVersion"
                          label="版號"
                        ></v-text-field>
                      </v-flex>
                      <v-flex xs2>
                        <v-text-field
                          v-model="filter.DocumentOwner"
                          @change="GetDocumentList"
                          name="DocumentOwner"
                          label="權責人員"
                        ></v-text-field>
                      </v-flex>
                      <v-flex xs2>
                        <v-menu
                          v-model="isShowStartDatePicker"
                          :close-on-content-click="false"
                          transition="scale-transition"
                          offset-y
                        >
                          <template v-slot:activator="{ on }">
                            <v-text-field
                              v-model="filterStartEffectiveDate"
                              :label="'查詢生效日期 (起)'"
                              readonly
                              v-on="on"
                            ></v-text-field>
                          </template>
                          <v-date-picker
                            v-model="filter.StartEffectiveDate"
                            locale="Language"
                            @change="GetDocumentList"
                            :year-format="yearFormat"
                            :title-date-format="yearFormat"
                            :header-date-format="yearFormat"
                            @input="isShowStartDatePicker = false"
                          ></v-date-picker>
                        </v-menu>
                      </v-flex>
                      <v-flex xs2>
                        <v-menu
                          v-model="isShowEndDatePicker"
                          :close-on-content-click="false"
                          transition="scale-transition"
                          offset-y
                        >
                          <template v-slot:activator="{ on }">
                            <v-text-field
                              v-model="filterEndEffectiveDate"
                              :label="'查詢生效日期 (迄)'"
                              readonly
                              v-on="on"
                            ></v-text-field>
                          </template>
                          <v-date-picker
                            v-model="filter.EndEffectiveDate"
                            locale="Language"
                            @change="GetDocumentList"
                            :year-format="yearFormat"
                            :title-date-format="yearFormat"
                            :header-date-format="yearFormat"
                            @input="isShowEndDatePicker = false"
                          ></v-date-picker>
                        </v-menu>
                      </v-flex>
                      <!-- <v-flex xs2>
                                    <v-text-field v-model="filter.FileName"
                                                @change="GetDocumentList"
                                                Name="FileName"
                                                label="檔案名稱"></v-text-field>
                      </v-flex>-->
                    </v-layout>
                  </v-container>
                </v-card>
              </v-flex>
              <v-spacer></v-spacer>
              <v-flex flex-grow-1 xs12 sm12 md12 lg12 x12>
                <v-card>
                  <v-data-table
                    :loading="isLoading"
                    :headers="headers"
                    :items="DocumentList"
                    :items-per-page.sync="pagination.rowsPerPage"
                    :footer-props="{
                                        'items-per-page-options': [20, 30], // -> Add this example
                                        'items-per-page-text':'每頁筆數'
                                    }"
                    :sort-by="'DocumentNumber'"
                    class="elevation-1"
                    dense
                  >
                    <template
                      v-slot:item.DocumentType="{ item }"
                    >{{ GetDocumentTypeString(item.DocumentType) }}</template>
                    <template
                      v-slot:item.EffectiveDate="{ item }"
                    >{{ formatDate(item.EffectiveDate) }}</template>
                    <template v-slot:item.CreateTime="{ item }">{{ formatDate(item.CreateTime) }}</template>
                    <template v-slot:item.action="{ item }">
                      <v-btn small tile outlined color="primary" @click="download(item)">
                        <v-icon left style="cursor:pointer;">archive</v-icon>下載
                      </v-btn>
                    </template>
                    <template slot="no-data">
                      <div style="text-align: center;">沒有資料</div>
                    </template>
                    <v-alert slot="no-results" :value="true" color="error" icon="warning">
                      {{
                      ''
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
  </div>
</template>

<script>
import axios from 'axios'
import moment from 'moment'
export default {
  components: {},
  data () {
    return {
      isLoading: false,
      DocumentList: [],
      pagination: {
        rowsPerPage: -1,
        descending: false
      },
      headers: [
        {
          text: '文件名稱',
          align: 'start',
          sortable: false,
          value: 'DocumentName'
        },
        {
          text: '文件編號',
          align: 'start',
          sortable: true,
          value: 'DocumentNumber'
        },
        {
          text: '版號',
          align: 'start',
          sortable: false,
          value: 'DocumentVersion'
        },
        {
          text: '生效日期',
          align: 'start',
          sortable: false,
          value: 'EffectiveDate'
        },
        {
          text: '權責人員',
          align: 'start',
          sortable: false,
          value: 'DocumentOwner'
        },
        // {
        //   text: '檔案名稱',
        //   align: 'start',
        //   sortable: false,
        //   value: 'FileName'
        // },
        // {
        //   text: '附檔名',
        //   align: 'start',
        //   sortable: true,
        //   value: 'FileType'
        // },
        // {
        //   text: '文件階層',
        //   align: 'start',
        //   sortable: true,
        //   value: 'DocumentType'
        // },
        // {
        //   text: '上傳時間',
        //   align: 'start',
        //   sortable: true,
        //   value: 'CreateTime'
        // },
        {
          text: '動作',
          align: 'start',
          sortable: false,
          value: 'action'
        }
      ],
      loadingDialog: false,
      uploadDialog: false,

      filter: {
        FileName: '',
        StartDate: '',
        EndDate: '',
        StartEffectiveDate: '',
        EndEffectiveDate: '',
        DocumentType: -1,
        DocumentName: '',
        DocumentNumber: '',
        IsPublish: 'Y',
        DocumentOwner: '',
        DocumentVersion: ''
      },
      isShowStartDatePicker: false,
      isShowEndDatePicker: false,
      isShowEffectiveDatePicker: false,
      isShowPublishDatePicker: false,

      DocumentTypes: [
        {
          Id: 1,
          Name: 'L1'
        },
        {
          Id: 2,
          Name: 'L2'
        },
        {
          Id: 3,
          Name: 'L3'
        },
        {
          Id: 4,
          Name: 'L4'
        }
      ],
      EffectiveDate: '',
      PublishDate: '',
      DocumentType: 0,
      DocumentName: '',
      DocumentNumber: '',
      currentUploadFile: null,

      editDialog: false,
      Edit: {
        Id: -1,
        EffectiveDate: '',
        PublishDate: '',
        DocumentType: 0,
        DocumentName: '',
        DocumentNumber: '',
        IsReUploadFile: false,
        currentUploadFile: null
      }
    }
  },
  created: function () {},
  mounted () {
    let id = this.$route.query.id
    if (id > 0) {
      this.filter.DocumentType = id * 1
    }
    this.GetDocumentList()
  },
  directives: {},
  methods: {
    download: function (item) {
      axios
        .post('/api/Document/DownloadDocument', item, {
          headers: {
            'content-type': 'application/json'
          },
          responseType: 'blob'
        })
        .then((response) => {
          var content = response.data
          var blob = new Blob([content])
          var fileName = item.FileName + '.' + item.FileType
          if ('download' in document.createElement('a')) {
            var elink = document.createElement('a')
            elink.download = fileName
            elink.style.display = 'none'
            elink.href = URL.createObjectURL(blob)
            document.body.appendChild(elink)
            elink.click()
            URL.revokeObjectURL(elink.href)
            document.body.removeChild(elink)
          } else {
            navigator.msSaveBlob(blob, fileName)
          }
        })
    },
    previewFiles: function (files) {
      this.currentUploadFile = files[0]
    },
    previewEditFiles: function (files) {
      this.Edit.currentUploadFile = files[0]
    },
    edit: function (item) {
      this.Edit.Id = item.Id
      this.Edit.EffectiveDate =
        item.EffectiveDate === '0001-01-01T00:00:00'
          ? ''
          : item.EffectiveDate.split('T')[0]
      this.Edit.PublishDate = item.PublishDate
      this.Edit.DocumentType = item.DocumentType
      this.Edit.DocumentName = item.DocumentName
      this.Edit.DocumentNumber = item.DocumentNumber
      this.Edit.IsReUploadFile = false

      this.editDialog = true
    },
    upload: function () {
      let self = this
      let uploadFile = this.currentUploadFile

      let effectiveDate = this.EffectiveDate
      let publishDate = this.PublishDate
      let documentType = this.DocumentType
      let documentName = this.DocumentName
      let documentNumber = this.DocumentNumber

      self.loadingDialog = true

      let formData = new FormData()

      formData.append('file', uploadFile)
      formData.append('EffectiveDate', effectiveDate)
      formData.append('PublishDate', publishDate)
      formData.append('DocumentName', documentName)
      formData.append('DocumentNumber', documentNumber)
      formData.append('DocumentType', documentType)

      axios
        .post('/api/Document/UploadFile', formData, {
          headers: {
            'content-type': 'multipart/form-data'
          }
        })
        .then((response) => {
          self.GetDocumentList()
        })
        .catch(function () {})
        .finally(function () {
          self.loadingDialog = false
          self.uploadDialog = false
        })
    },
    setDocument: function () {
      let self = this
      let uploadFile = this.Edit.currentUploadFile

      let effectiveDate = this.Edit.EffectiveDate
      let publishDate = this.Edit.PublishDate
      let documentType = this.Edit.DocumentType
      let documentName = this.Edit.DocumentName
      let documentNumber = this.Edit.DocumentNumber
      let id = this.Edit.Id

      self.loadingDialog = true

      let formData = new FormData()

      if (this.Edit.IsReUploadFile) {
        formData.append('file', uploadFile)
      }

      formData.append('EffectiveDate', effectiveDate)
      formData.append('PublishDate', publishDate)
      formData.append('DocumentName', documentName)
      formData.append('DocumentNumber', documentNumber)
      formData.append('DocumentType', documentType)
      formData.append('Id', id)

      axios
        .post('/api/Document/SetFile', formData, {
          headers: {
            'content-type': 'multipart/form-data'
          }
        })
        .then((response) => {
          self.GetDocumentList()
        })
        .catch(function () {})
        .finally(function () {
          self.loadingDialog = false
          self.editDialog = false
        })
    },
    publish: function (item) {
      let self = this
      axios.post('/api/Document/Publish', item).then((response) => {
        self.GetDocumentList()
      })
    },
    unPublish: function (item) {
      let self = this
      axios.post('/api/Document/UnPublish', item).then((response) => {
        self.GetDocumentList()
      })
    },
    formatDate (dateTime) {
      let date = moment(dateTime).format('YYYY/MM/DD')
      let dateArray = date.split('/')
      if (dateArray.length === 3) {
        return `${dateArray[0] * 1 - 1911}/${dateArray[1]}/${dateArray[2]}`
      }
      return moment(dateTime).format('YYYY/MM/DD')
    },
    GetDocumentList: function (item) {
      let self = this
      self.isLoading = true
      axios
        .post('/api/Document/GetDocumentList', self.filter)
        .then((response) => {
          self.DocumentList = response.data
        })
        .finally(() => {
          self.isLoading = false
        })
    },
    GetDocumentTypeString: function (TypeId) {
      let type = this.DocumentTypes.find(function (item, index, array) {
        return item.Id === TypeId
      })

      if (type) {
        return type.Name
      }

      return ''
    },
    yearFormat: function (yearMonth) {
      if (yearMonth && yearMonth !== '') {
        let dateArray = yearMonth.split('-')
        if (dateArray.length === 3) {
          return `民國${dateArray[0] * 1 - 1911}年${dateArray[1]}月${
            dateArray[2]
          }日`
        } else if (dateArray.length === 2) {
          return `民國${dateArray[0] * 1 - 1911}年${dateArray[1]}月`
          // return (dateArray[0] * 1 - 1911) + '-' + dateArray[1]
        } else if (dateArray.length === 1) {
          return `民國${dateArray[0] * 1 - 1911}年`
        }
      }
      return yearMonth
    },
    cleanFilter () {
      this.filter.FileName = ''
      this.filter.StartDate = ''
      this.filter.EndDate = ''
      this.filter.StartEffectiveDate = ''
      this.filter.EndEffectiveDate = ''
      this.filter.DocumentType = -1
      this.filter.DocumentName = ''
      this.filter.DocumentNumber = ''
      this.filter.IsPublish = 'Y'
      this.filter.DocumentOwner = ''
      this.filter.DocumentVersion = ''
      this.GetDocumentList()
    }
  },
  computed: {
    Language () {
      return 'zh-TW'
    },
    ParamDocumentType () {
      return this.$route.query.id
    },
    filterStartEffectiveDate: {
      get () {
        return this.yearFormat(this.filter.StartEffectiveDate)
      }
    },
    filterEndEffectiveDate: {
      get () {
        return this.yearFormat(this.filter.EndEffectiveDate)
      }
    }
  },
  watch: {
    uploadDialog (isOpen) {
      if (!isOpen) {
        this.EffectiveDate = ''
        this.PublishDate = ''
        this.DocumentType = 0
        this.DocumentName = ''
        this.DocumentNumber = ''
      }
    },
    ParamDocumentType (val) {
      if (val > 0) {
        this.filter.DocumentType = val * 1
      } else {
        this.filter.DocumentType = -1
      }
      this.GetDocumentList()
    }
  }
}
</script>
