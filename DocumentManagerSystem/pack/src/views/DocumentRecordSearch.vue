<template>
    <div>
        <v-layout align-start justify-start wrap>
          <div style="height: 100%;
                width: 100%;
                position: absolute;
                top: 0px;
                left: 0px;
                overflow-y:auto;
                background-color: #fafafa;">
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
                            <v-layout row wrap >
                                <v-flex xs2>
                                    <v-text-field v-model="filter.DocumentName"
                                                @change="GetDocumentList"
                                                Name="DocumentName"
                                                label="ISMS文件名稱"></v-text-field>
                                </v-flex>
                                <v-flex xs2>
                                    <v-text-field v-model="filter.DocumentNumber"
                                                @change="GetDocumentList"
                                                Name="DocumentNumber"
                                                label="文件編號"></v-text-field>
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
                                  <!-- <v-text-field dense
                                                v-model="Edit.DocumentYearMonth"
                                                label="資料年月"></v-text-field> -->
                                  <v-dialog
                                          ref="filterDialogStart"
                                          v-model="openDocumentYearMonthStartPicker"
                                          :return-value.sync="filter.DocumentYearMonthStart"
                                          persistent
                                          width="290px"
                                        >
                                          <template v-slot:activator="{ on }">
                                            <v-text-field
                                              v-model="filterDocumentYearMonthStart"
                                              label="資料年月 (起)"
                                              prepend-icon="event"
                                              readonly
                                              v-on="on"
                                            ></v-text-field>
                                          </template>
                                          <v-date-picker
                                            v-model="filter.DocumentYearMonthStart"
                                            type="month"
                                            :locale="Language"
                                            scrollable
                                            @change="GetDocumentList"
                                            :year-format="yearFormat"
                                            :title-date-format="yearFormat"
                                            :header-date-format="yearFormat"
                                          >
                                            <v-spacer></v-spacer>
                                            <v-btn text color="primary" @click="openDocumentYearMonthStartPicker = false">取消</v-btn>
                                            <v-btn text color="primary" @click="$refs.filterDialogStart.save(filter.DocumentYearMonthStart)">確認</v-btn>
                                          </v-date-picker>
                                  </v-dialog>
                                </v-flex>
                                <v-flex xs2>
                                  <!-- <v-text-field dense
                                                v-model="Edit.DocumentYearMonth"
                                                label="資料年月"></v-text-field> -->
                                  <v-dialog
                                          ref="filterDialogEnd"
                                          v-model="openDocumentYearMonthEndPicker"
                                          :return-value.sync="filter.DocumentYearMonthEnd"
                                          persistent
                                          width="290px"
                                        >
                                          <template v-slot:activator="{ on }">
                                            <v-text-field
                                              v-model="filterDocumentYearMonthEnd"
                                              label="資料年月 (迄)"
                                              prepend-icon="event"
                                              readonly
                                              v-on="on"
                                            ></v-text-field>
                                          </template>
                                          <v-date-picker
                                            v-model="filter.DocumentYearMonthEnd"
                                            type="month"
                                            :locale="Language"
                                            scrollable
                                            @change="GetDocumentList"
                                            :year-format="yearFormat"
                                            :title-date-format="yearFormat"
                                            :header-date-format="yearFormat"
                                          >
                                            <v-spacer></v-spacer>
                                            <v-btn text color="primary" @click="openDocumentYearMonthEndPicker = false">取消</v-btn>
                                            <v-btn text color="primary" @click="$refs.filterDialogEnd.save(filter.DocumentYearMonthEnd)">確認</v-btn>
                                          </v-date-picker>
                                  </v-dialog>
                                </v-flex>
                                <!-- <v-flex xs2>
                                    <v-text-field v-model="filter.DocumentStartNumber"
                                                @change="GetDocumentList"
                                                Name="DocumentStartNumber"
                                                label="紀錄編號 (起)"></v-text-field>
                                </v-flex>
                                <v-flex xs2>
                                    <v-text-field v-model="filter.DocumentEndNumber"
                                                @change="GetDocumentList"
                                                Name="DocumentEndNumber"
                                                label="紀錄編號 (迄)"></v-text-field>
                                </v-flex> -->
                                <!-- <v-flex xs2>
                                    <v-menu
                                        v-model="isShowStartDatePicker"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y>
                                        <template v-slot:activator="{ on }">
                                            <v-text-field
                                                v-model="filterStartDate"
                                                :label="'查詢起始日期'"
                                                readonly
                                                v-on="on"></v-text-field>
                                        </template>
                                        <v-date-picker
                                            v-model="filter.StartDate"
                                            locale="Language"
                                            @change="GetDocumentList"
                                            :year-format="yearFormat"
                                            :title-date-format="yearFormat"
                                            :header-date-format="yearFormat"
                                            @input="isShowStartDatePicker = false"></v-date-picker>
                                    </v-menu>
                                </v-flex>
                                <v-flex xs2>
                                    <v-menu
                                        v-model="isShowEndDatePicker"
                                        :close-on-content-click="false"
                                        transition="scale-transition"
                                        offset-y>
                                        <template v-slot:activator="{ on }">
                                            <v-text-field
                                                v-model="filterEndDate"
                                                :label="'查詢結束日期'"
                                                readonly
                                                v-on="on"></v-text-field>
                                        </template>
                                        <v-date-picker
                                            v-model="filter.EndDate"
                                            locale="Language"
                                            @change="GetDocumentList"
                                            :year-format="yearFormat"
                                            :title-date-format="yearFormat"
                                            :header-date-format="yearFormat"
                                            @input="isShowEndDatePicker = false"></v-date-picker>
                                    </v-menu>
                                </v-flex> -->
                                <!-- <v-flex xs4>
                                    <v-text-field v-model="filter.FileName"
                                                @change="GetDocumentList"
                                                Name="FileName"
                                                label="檔案名稱"></v-text-field>
                                </v-flex> -->
                            </v-layout>
                        </v-container>
                      </v-card>
                  </v-flex>
                  <v-spacer></v-spacer>
                  <v-flex flex-grow-1 xs12 sm12 md12 lg12 x12>
                    <v-card>
                      <v-data-table :loading="isLoading"
                                    :headers="headers"
                                    :items="DocumentList"
                                    :items-per-page.sync="pagination.rowsPerPage"
                                    :footer-props="{
                                        'items-per-page-options': [20, 30], // -> Add this example
                                        'items-per-page-text':'每頁筆數'
                                    }"
                                    :sort-by="['DocumentYearMonth','DocumentStartNumber']"
                                    multi-sort
                                    class="elevation-1"
                                    dense>
                        <template v-slot:item.CreateTime="{ item }">
                          {{ formatDate(item.CreateTime) }}
                        </template>
                        <template v-slot:item.DocumentYearMonth="{ item }">
                          {{ ToFormatDocumentYearMonth(item.DocumentYearMonth) }}
                        </template>
                        <template v-slot:item.action="{ item }">
                          <v-btn small tile outlined color="primary" @click="download(item)">
                            <v-icon left style="cursor:pointer;">
                                  archive
                            </v-icon> 下載
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
  components: {

  },
  data () {
    return {
      isLoading: false,
      DocumentList: [],
      pagination: {
        rowsPerPage: -1,
        descending: false
      },
      test: true,
      headers: [
        {
          text: 'ISMS文件名稱',
          align: 'start',
          sortable: false,
          value: 'DocumentName'
        },
        {
          text: '文件編號',
          align: 'start',
          sortable: false,
          value: 'DocumentNumber'
        },
        {
          text: '紀錄編號 (起)',
          align: 'start',
          sortable: true,
          value: 'DocumentStartNumber'
        },
        {
          text: '紀錄編號 (迄)',
          align: 'start',
          sortable: false,
          value: 'DocumentEndNumber'
        },
        {
          text: '文件件數',
          align: 'start',
          sortable: false,
          value: 'DocumentCount'
        },
        {
          text: '文件頁數',
          align: 'start',
          sortable: false,
          value: 'DocumentPageCount'
        },
        {
          text: '資料年月',
          align: 'start',
          sortable: false,
          value: 'DocumentYearMonth'
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
        DocumentType: 5,
        DocumentName: '',
        DocumentNumber: '',
        DocumentYearMonthStart: '',
        DocumentYearMonthEnd: '',
        DocumentStartNumber: '',
        DocumentEndNumber: '',
        IsPublish: 'Y'
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
      DocumentType: 5,
      DocumentName: '',
      DocumentNumber: '',
      currentUploadFile: null,

      editDialog: false,
      Edit: {
        Id: -1,
        EffectiveDate: '',
        PublishDate: '',
        DocumentType: 5,
        DocumentName: '',
        DocumentNumber: '',
        IsReUploadFile: false,
        currentUploadFile: null
      },

      openDocumentYearMonthStartPicker: false,
      openDocumentYearMonthEndPicker: false

    }
  },
  created: function () {

  },
  mounted () {
    this.GetDocumentList()
  },
  directives: {

  },
  methods: {
    download: function (item) {
      axios.post('/api/Document/DownloadDocument', item, {
        headers: {
          'content-type': 'application/json'
        },
        responseType: 'blob'
      }).then(
        response => {
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
        }
      )
    },
    previewFiles: function (files) {
      this.currentUploadFile = files[0]
    },
    previewEditFiles: function (files) {
      this.Edit.currentUploadFile = files[0]
    },
    edit: function (item) {
      this.Edit.Id = item.Id
      this.Edit.EffectiveDate = item.EffectiveDate === '0001-01-01T00:00:00' ? '' : item.EffectiveDate.split('T')[0]
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

      axios.post('/api/Document/UploadFile', formData, {
        headers: {
          'content-type': 'multipart/form-data'
        }
      }).then(
        response => {
          self.GetDocumentList()
        }
      ).catch(function () {

      })
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

      axios.post('/api/Document/SetFile', formData, {
        headers: {
          'content-type': 'multipart/form-data'
        }
      }).then(
        response => {
          self.GetDocumentList()
        }
      ).catch(function () {

      })
        .finally(function () {
          self.loadingDialog = false
          self.editDialog = false
        })
    },
    publish: function (item) {
      let self = this
      axios.post('/api/Document/Publish', item)
        .then(response => {
          self.GetDocumentList()
        })
    },
    unPublish: function (item) {
      let self = this
      axios.post('/api/Document/UnPublish', item)
        .then(response => {
          self.GetDocumentList()
        })
    },
    formatDate (dateTime) {
      let date = moment(dateTime).format('YYYY/MM/DD hh:mm')
      let dateArray = date.split('/')
      if (dateArray.length === 3) {
        return `${(dateArray[0] * 1 - 1911)}/${dateArray[1]}/${dateArray[2]}`
      }
      return moment(dateTime).format('YYYY/MM/DD hh:mm')
    },
    ToFormatDocumentYearMonth (documentYearMonth) {
      if (documentYearMonth) {
        let dateStr = documentYearMonth
        let dateArray = dateStr.split('-')
        if (dateArray.length === 2) {
          return `${(dateArray[0] * 1 - 1911)}-${dateArray[1]}`
        }

        return dateStr
      }
    },
    GetDocumentList: function (item) {
      let self = this
      self.isLoading = true
      let yearMonthStart = self.filter.DocumentYearMonthStart.replace('-', '') * 1
      let yearMonthEnd = self.filter.DocumentYearMonthEnd.replace('-', '') * 1

      if (yearMonthStart > yearMonthEnd && yearMonthEnd !== 0) {
        alert('輸入的[ 資料年月 (迄) ]大於[ 資料年月 (迄) ]，請確認!')
        self.filter.DocumentYearMonthEnd = self.filter.DocumentYearMonthStart
      }

      axios.post('/api/Document/GetDocumentList', self.filter)
        .then(response => {
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
          return `民國${(dateArray[0] * 1 - 1911)}年${dateArray[1]}月${dateArray[2]}日`
        } else if (dateArray.length === 2) {
          return `民國${(dateArray[0] * 1 - 1911)}年${dateArray[1]}月`
          // return (dateArray[0] * 1 - 1911) + '-' + dateArray[1]
        } else if (dateArray.length === 1) {
          return `民國${(dateArray[0] * 1 - 1911)}年`
        }
      }
      return yearMonth
    },
    cleanFilter () {
      this.filter.FileName = ''
      this.filter.StartDate = ''
      this.filter.EndDate = ''
      this.filter.DocumentType = 5
      this.filter.DocumentName = ''
      this.filter.DocumentNumber = ''
      this.filter.DocumentYearMonthStart = ''
      this.filter.DocumentYearMonthEnd = ''
      this.filter.DocumentStartNumber = ''
      this.filter.DocumentEndNumber = ''
      this.filter.IsPublish = 'Y'
      this.GetDocumentList()
    }
  },
  computed: {
    Language () {
      return 'zh-TW'
    },
    filterStartDate: {
      get () {
        return this.yearFormat(this.filter.StartDate)
      }
    },
    filterEndDate: {
      get () {
        return this.yearFormat(this.filter.EndDate)
      }
    },
    filterDocumentYearMonthStart: {
      get () {
        return this.yearFormat(this.filter.DocumentYearMonthStart)
      }
    },
    filterDocumentYearMonthEnd: {
      get () {
        return this.yearFormat(this.filter.DocumentYearMonthEnd)
      }
    }
  },
  watch: {
    uploadDialog (isOpen) {
      if (!isOpen) {
        this.EffectiveDate = ''
        this.PublishDate = ''
        this.DocumentName = ''
        this.DocumentNumber = ''
      }
    }
  }
}
</script>
