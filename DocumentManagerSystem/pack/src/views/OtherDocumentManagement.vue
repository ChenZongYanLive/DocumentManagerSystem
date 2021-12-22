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
              <v-btn class="ml-1" color="primary" dark @click.stop="uploadDialog = true">上傳文件</v-btn>
              <v-btn class="ml-2" color="primary" dark @click.stop="openExportReportPopup" v-if="$isRoleOk($identity.ExportReport)">匯出報表</v-btn>
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
                          label="外來文件名稱"
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
                            :year-format="yearFormat"
                            :title-date-format="yearFormat"
                            :header-date-format="yearFormat"
                            v-model="filter.StartEffectiveDate"
                            locale="Language"
                            @change="GetDocumentList"
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
                            :year-format="yearFormat"
                            :title-date-format="yearFormat"
                            :header-date-format="yearFormat"
                            v-model="filter.EndEffectiveDate"
                            locale="Language"
                            @change="GetDocumentList"
                            @input="isShowEndDatePicker = false"
                          ></v-date-picker>
                        </v-menu>
                      </v-flex>
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
                      v-slot:item.DocumentVersion="{ item }">
                      {{ `${item.DocumentVersion !== '' ? 'v'+ item.DocumentVersion : ''}` }}
                    </template>
                    <template
                      v-slot:item.DocumentType="{ item }"
                    >{{ GetDocumentTypeString(item.DocumentType) }}</template>
                    <template
                      v-slot:item.EffectiveDate="{ item }"
                    >{{ formatDate(item.EffectiveDate) }}</template>
                    <template v-slot:item.CreateTime="{ item }">{{ formatDate(item.CreateTime) }}</template>
                    <template v-slot:item.IsPublish="{ item }">{{ getStatus(item) }}</template>
                    <template v-slot:item.action="{ item }">
                      <v-btn
                        small
                        v-if="!item.IsPublish && !item.IsInvalid"
                        tile
                        outlined
                        color="teal"
                        @click="publish(item)"
                      >
                        <v-icon left>send</v-icon>發佈
                      </v-btn>
                      <v-btn
                        small
                        v-if="item.IsPublish"
                        tile
                        outlined
                        color="teal"
                        @click="unPublish(item)"
                      >
                        <v-icon left>send</v-icon>取消發佈
                      </v-btn>

                      <v-btn small tile outlined color="success" @click="edit(item)" v-if="!item.IsInvalid">
                        <v-icon left>create</v-icon>修改
                      </v-btn>

                      <v-btn
                        small
                        tile
                        outlined
                        color="purple"
                        @click="changeVersion(item)"
                        v-if="!item.IsInvalid"
                      >
                        <v-icon left>update</v-icon>更版
                      </v-btn>
                      <v-btn small tile outlined color="deep" @click="versionHistory(item)">
                        <v-icon left>history</v-icon>更版歷程
                      </v-btn>

                      <v-btn small tile outlined color="primary" @click="download(item)">
                        <v-icon left style="cursor:pointer;">archive</v-icon>下載
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

                      <v-btn
                        small
                        tile
                        outlined
                        color="orange"
                        v-if="$isRoleOk($identity.InvalidDocument) && !item.IsInvalid"
                        @click="OpenInvalidPopup(item)"
                      >
                        <v-icon left>highlight_off</v-icon>作廢
                      </v-btn>
                      <v-btn
                        small
                        tile
                        outlined
                        color="orange"
                        v-if="$isRoleOk($identity.reInvalidDocument) && item.IsInvalid"
                        @click="CancelVoidDocument(item)"
                      >
                        <v-icon left>check_circle</v-icon>取消作廢
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
    <v-dialog v-model="uploadDialog" hide-overlay persistent width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">上傳文件</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="uploadForm" style="width:100%">
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
                <ValidationProvider name="DocumentType" rules="required" v-slot="{ errors }">
                  <v-select
                    :items="DocumentTypes"
                    item-text="Name"
                    item-value="Id"
                    v-model="DocumentType"
                    :label="$t('文件階層')"
                    persistent-hint
                    :error-messages="errors[0]"
                    @change="ChangeDocumentTypes"
                  ></v-select>
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
              <template v-if="!IsSpecialDocument(DocumentNumber)">
                <v-flex xs12>
                  <ValidationProvider name="DocumentVersion" rules="required" v-slot="{ errors }">
                    <v-text-field
                      dense
                      v-model="DocumentVersion"
                      :error-messages="errors[0]"
                      label="版號"
                      prefix="v"
                      hint="請輸入#.#.#格式 Ex. 1.0.0"
                    ></v-text-field>
                  </ValidationProvider>
                </v-flex>
                <v-flex xs8>
                  <v-menu
                    v-model="isShowEffectiveDatePicker"
                    :close-on-content-click="false"
                    transition="scale-transition"
                    offset-y
                  >
                    <template v-slot:activator="{ on }">
                      <ValidationProvider name="EffectiveDate" rules="required" v-slot="{ errors }">
                        <v-text-field
                          v-model="formatEffectiveDate"
                          :label="'上版日期'"
                          readonly
                          :error-messages="errors[0]"
                          v-on="on"
                        ></v-text-field>
                      </ValidationProvider>
                    </template>
                    <v-date-picker
                      :year-format="yearFormat"
                      :title-date-format="yearFormat"
                      :header-date-format="yearFormat"
                      v-model="EffectiveDate"
                      locale="Language"
                      @input="isShowEffectiveDatePicker = false"
                    ></v-date-picker>
                  </v-menu>
                </v-flex>
              </template>
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
              <v-flex xs12>
                <v-file-input
                  show-size
                  counter
                  multiple
                  prepend-icon
                  label="選擇檔案"
                  type="file"
                  name="files"
                  v-model="previewFilesData"
                  @change="previewFiles"
                ></v-file-input>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="uploadDialog = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click="upload">上傳</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="editDialog" hide-overlay persistent width="450">
      <v-card>
        <v-card-title class="blue darken-2 white--text">修改文件</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="uploadEditForm" style="width:100%">
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
                <ValidationProvider name="DocumentType" rules="required" v-slot="{ errors }">
                  <v-select
                    :error-messages="errors[0]"
                    :items="DocumentTypes"
                    item-text="Name"
                    item-value="Id"
                    v-model="Edit.DocumentType"
                    :label="$t('文件階層')"
                    persistent-hint
                    @change="ChangeEditDocumentTypes"
                  ></v-select>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentType" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentNumber"
                    label="文件編號"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <template v-if="!IsSpecialDocument(Edit.DocumentNumber)">
                <v-flex xs12>
                  <ValidationProvider name="DocumentVersion" rules="required" v-slot="{ errors }">
                    <v-text-field
                      dense
                      :error-messages="errors[0]"
                      v-model="Edit.DocumentVersion"
                      label="版號"
                      prefix="v"
                      hint="請輸入#.#.#格式 Ex. 1.0.0"
                    ></v-text-field>
                  </ValidationProvider>
                </v-flex>
                <v-flex xs8>
                  <v-menu
                    v-model="isShowEffectiveDatePicker_Edit"
                    :close-on-content-click="false"
                    transition="scale-transition"
                    offset-y
                  >
                    <template v-slot:activator="{ on }">
                      <ValidationProvider
                        name="DocumentVersion"
                        rules="required"
                        v-slot="{ errors }"
                      >
                        <v-text-field
                          :error-messages="errors[0]"
                          v-model="EditEffectiveDate"
                          :label="'上版日期'"
                          readonly
                          v-on="on"
                        ></v-text-field>
                      </ValidationProvider>
                    </template>
                    <v-date-picker
                      :year-format="yearFormat"
                      :title-date-format="yearFormat"
                      :header-date-format="yearFormat"
                      v-model="Edit.EffectiveDate"
                      locale="Language"
                      @input="isShowEffectiveDatePicker_Edit = false"
                    ></v-date-picker>
                  </v-menu>
                  <v-flex xs12>
                    <ValidationProvider name="DocumentVersion" rules="required" v-slot="{ errors }">
                      <v-text-field
                        dense
                        :error-messages="errors[0]"
                        v-model="Edit.DocumentOwner"
                        label="權責人員"
                      ></v-text-field>
                    </ValidationProvider>
                  </v-flex>
                </v-flex>
              </template>
              <v-checkbox v-model="Edit.IsReUploadFile" class="mx-2" label="重新上傳檔案"></v-checkbox>
              <v-flex xs12 v-if="Edit.IsReUploadFile">
                <v-file-input
                  show-size
                  counter
                  multiple
                  prepend-icon
                  label="選擇檔案"
                  type="file"
                  name="files"
                  v-model="previewEditFilesData"
                  @change="previewEditFiles"
                ></v-file-input>
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
    <v-dialog v-model="loadingDialog" hide-overlay persistent width="300">
      <v-card color="primary" dark>
        <v-card-text>
          上傳文件中
          <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
        </v-card-text>
      </v-card>
    </v-dialog>
    <v-dialog v-model="ExportFileDialog" hide-overlay persistent width="300">
      <v-card color="primary" dark>
        <v-card-text>
          處理檔案中
          <v-progress-linear indeterminate color="white" class="mb-0"></v-progress-linear>
        </v-card-text>
      </v-card>
    </v-dialog>
    <v-dialog v-model="IsOpenAlertDialog" width="500">
      <v-card>
        <v-alert text prominent type="error" class="mb-0">{{AlertDialogMessage}}</v-alert>
      </v-card>
    </v-dialog>
    <v-dialog v-model="IsShowReportPopup"  hide-overlay persistent width="300">
      <v-card>
        <v-card-title class="blue darken-2 white--text">請選擇匯出報表的時間</v-card-title>
        <v-card-actions>
          <v-container grid-list-sm>
            <v-layout row wrap>
              <v-flex xs12>
                <v-menu
                  v-model="isShowReportEffectiveDatePicker"
                  :close-on-content-click="false"
                  transition="scale-transition"
                  offset-y
                >
                  <template v-slot:activator="{ on }">
                    <ValidationProvider name="ExportDate" rules="required" v-slot="{ errors }">
                      <v-text-field
                        v-model="formatReportEffectiveDate"
                        :label="'匯出日期'"
                        readonly
                        :error-messages="errors[0]"
                        v-on="on"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    :year-format="yearFormat"
                    :title-date-format="yearFormat"
                    :header-date-format="yearFormat"
                    v-model="ReportEffectiveDate"
                    locale="Language"
                    @input="isShowReportEffectiveDatePicker = false"
                  ></v-date-picker>
                </v-menu>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-actions>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="IsShowReportPopup = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click="exportExcel">匯出</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import axios from 'axios'
import moment from 'moment'
export default {
  components: {},
  data () {
    return {
      IsOpenAlertDialog: false,
      AlertDialogMessage: false,
      isLoading: false,
      versionHistoryIsLoading: false,
      DocumentList: [],
      DocumentVersionList: [],
      pagination: {
        rowsPerPage: -1,
        descending: false
      },
      versionHistoryHeaders: [
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
          text: '上版日期',
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
        {
          text: '檔案名稱',
          align: 'start',
          sortable: false,
          value: 'FileName'
        },
        {
          text: '附檔名',
          align: 'start',
          sortable: true,
          value: 'FileType'
        },
        {
          text: '文件階層',
          align: 'start',
          sortable: true,
          value: 'DocumentType'
        },
        {
          text: '異動日期',
          align: 'start',
          sortable: true,
          value: 'UpdateTime'
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
        {
          text: '檔案名稱',
          align: 'start',
          sortable: false,
          value: 'FileName'
        },
        {
          text: '附檔名',
          align: 'start',
          sortable: true,
          value: 'FileType'
        },
        {
          text: '文件階層',
          align: 'start',
          sortable: true,
          value: 'DocumentType'
        },
        // {
        //   text: '上傳時間',
        //   align: 'start',
        //   sortable: true,
        //   value: 'CreateTime'
        // },
        {
          text: '狀態',
          align: 'start',
          sortable: true,
          value: 'IsPublish'
        },
        {
          text: '動作',
          align: 'start',
          sortable: false,
          value: 'action'
        }
      ],
      loadingDialog: false,
      ExportFileDialog: false,
      uploadDialog: false,
      versionUpdateDialog: false,
      documentVersionListDialog: false,

      filter: {
        FileName: '',
        StartDate: '',
        EndDate: '',
        StartEffectiveDate: '',
        EndEffectiveDate: '',
        DocumentType: -1,
        DocumentName: '',
        DocumentNumber: '',
        DocumentVersion: '',
        DocumentOwner: ''
      },
      isShowStartDatePicker: false,
      isShowEndDatePicker: false,
      isShowPublishDatePicker: false,

      isShowEffectiveDatePicker: false,
      isShowEffectiveDatePicker_Edit: false,
      isShowEffectiveDatePicker_Version: false,

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
      DocumentType: null,
      DocumentName: '',
      DocumentNumber: '',
      DocumentVersion: '',
      DocumentOwner: '',
      currentUploadFile: null,

      editDialog: false,
      Edit: {
        Id: -1,
        EffectiveDate: '',
        PublishDate: '',
        DocumentType: null,
        DocumentName: '',
        DocumentNumber: '',
        DocumentVersion: '',
        DocumentOwner: '',
        IsReUploadFile: false,
        currentUploadFile: null
      },

      VersionEdit: {
        Id: -1,
        EffectiveDate: '',
        PublishDate: '',
        DocumentType: null,
        DocumentName: '',
        DocumentNumber: '',
        DocumentVersion: '',
        DocumentOwner: '',
        currentUploadFile: null
      },

      previewFilesData: null,
      previewEditFilesData: null,
      previewVersionEditFilesData: null,

      InvalidDate: '',
      InvalidDoc: null,
      IsShowInvalidPopup: false,
      isShowInvalidDatePicker: false,

      IsShowReportPopup: false,
      ReportEffectiveDate: '',
      isShowReportEffectiveDatePicker: false
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
    downloadDocumentVersion: function (item) {
      axios
        .post('/api/Document/DownloadDocumentVersion', item, {
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
    previewVersionEditFiles: function (files) {
      this.VersionEdit.currentUploadFile = files[0]
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
      this.Edit.DocumentVersion = item.DocumentVersion
      this.Edit.DocumentOwner = item.DocumentOwner
      this.Edit.IsReUploadFile = false

      this.editDialog = true
    },
    upload: async function () {
      let self = this

      const isValid = await self.$refs.uploadForm.validate()

      if (!isValid) return

      let uploadFile = this.currentUploadFile

      if (uploadFile == null) {
        self.AlertDialogMessage = '請選擇檔案'
        self.IsOpenAlertDialog = true
        setTimeout(() => {
          self.IsOpenAlertDialog = false
        }, 1000)
        return
      }

      let effectiveDate = this.EffectiveDate
      let publishDate = this.PublishDate
      let documentType = this.DocumentType
      let documentName = this.DocumentName
      let documentNumber = this.DocumentNumber
      let DocumentVersion = this.DocumentVersion
      let DocumentOwner = this.DocumentOwner

      let checkIsSameDocumentNumberResponse = await axios.post('/api/Document/CheckIsSameStageDocumentNumber', { DocumentNumber: documentNumber })
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

      self.loadingDialog = true

      let formData = new FormData()

      formData.append('file', uploadFile)
      formData.append('EffectiveDate', effectiveDate)
      formData.append('PublishDate', publishDate)
      formData.append('DocumentName', documentName)
      formData.append('DocumentNumber', documentNumber)
      formData.append('DocumentVersion', DocumentVersion)
      formData.append('DocumentOwner', DocumentOwner)
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
    setDocument: async function () {
      let self = this

      const isValid = await self.$refs.uploadEditForm.validate()

      if (!isValid) return

      let uploadFile = this.Edit.currentUploadFile

      if (uploadFile == null && self.Edit.IsReUploadFile) {
        self.AlertDialogMessage = '請選擇檔案'
        self.IsOpenAlertDialog = true
        setTimeout(() => {
          self.IsOpenAlertDialog = false
        }, 1000)
        return
      }

      let effectiveDate = this.Edit.EffectiveDate
      let publishDate = this.Edit.PublishDate
      let documentType = this.Edit.DocumentType
      let documentName = this.Edit.DocumentName
      let documentNumber = this.Edit.DocumentNumber
      let DocumentVersion = this.Edit.DocumentVersion
      let DocumentOwner = this.Edit.DocumentOwner
      let id = this.Edit.Id

      let checkIsSameDocumentNumberResponse = await axios.post(`/api/Document/CheckIsSameStageDocumentNumber/${id}`, { DocumentNumber: documentNumber })
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

      self.loadingDialog = true

      let formData = new FormData()

      if (this.Edit.IsReUploadFile) {
        formData.append('file', uploadFile)
      }

      formData.append('EffectiveDate', effectiveDate)
      formData.append('PublishDate', publishDate)
      formData.append('DocumentName', documentName)
      formData.append('DocumentNumber', documentNumber)
      formData.append('DocumentVersion', DocumentVersion)
      formData.append('DocumentOwner', DocumentOwner)
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
    deleteDocument: function (item) {
      if (confirm('確定要刪除此文件嗎 ? ')) {
        let self = this
        axios.post('/api/Document/Delete', item).then((response) => {
          self.GetDocumentList()
        })
      }
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
      let strDate = ''
      if (dateArray.length === 3) {
        strDate = `${dateArray[0] * 1 - 1911}/${dateArray[1]}/${dateArray[2]}`
      } else {
        strDate = moment(dateTime).format('YYYY/MM/DD')
      }

      return strDate === '-1910/01/01' ? '' : strDate
    },
    GetDocumentList: function (item) {
      let self = this
      self.isLoading = true
      axios
        .post('/api/Document/GetDocumentList', self.filter)
        .then((response) => {
          self.DocumentList = response.data
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
    exportExcel: function () {
      let self = this
      self.ExportFileDialog = true
      self.filter.EffectiveDate = self.ReportEffectiveDate
      axios
        .post('/api/Document/ExportStageDocumentReport', self.filter, {
          headers: {
            'content-type': 'application/json'
          },
          responseType: 'blob'
        })
        .then((response) => {
          var content = response.data
          var blob = new Blob([content])
          if ('download' in document.createElement('a')) {
            var elink = document.createElement('a')
            elink.download = '桃園市政府地方稅務局 最新文件一覽表.pdf'
            elink.style.display = 'none'
            elink.href = URL.createObjectURL(blob)
            document.body.appendChild(elink)
            elink.click()
            URL.revokeObjectURL(elink.href)
            document.body.removeChild(elink)
          } else {
            navigator.msSaveBlob(
              blob,
              '桃園市政府地方稅務局 最新文件一覽表.pdf'
            )
          }

          self.filter.EffectiveDate = ''
          /*
          const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8' })
          const aEle = document.createElement('a') // 創建a標籤
          const href = window.URL.createObjectURL(blob) // 創建下載的鏈接
          aEle.href = href
          aEle.download = '桃園市政府地方稅務局 最新文件一覽表.xlsx' // 下載後文件名
          document.body.appendChild(aEle)
          aEle.click() // 點擊下載
          document.body.removeChild(aEle) // 下載完成移除元素
          window.URL.revokeObjectURL(href) // 釋放掉blob對象
*/
        }).finally(() => {
          self.IsShowReportPopup = false
        })
        .finally(() => {
          self.ExportFileDialog = false
        })
    },
    IsSpecialDocument (documentNumber) {
      return (
        documentNumber.indexOf('PXX') > -1 ||
        documentNumber.indexOf('RXX') > -1 ||
        documentNumber.indexOf('TXX') > -1
      )
    },
    ChangeDocumentTypes (typeId) {
      let prefixDocumentNumber = this.GetPrefixDocumentNumber(typeId)
      this.DocumentNumber = prefixDocumentNumber
    },
    ChangeEditDocumentTypes (typeId) {
      let prefixDocumentNumber = this.GetPrefixDocumentNumber(typeId)
      this.Edit.DocumentNumber = prefixDocumentNumber
    },
    GetPrefixDocumentNumber (typeId) {
      let text = ''

      switch (typeId) {
        case 1:
          text = 'ISMS-L1-'
          break
        case 2:
          text = 'ISMS-L2-'
          break
        case 3:
          text = 'ISMS-L3-'
          break
        case 4:
          text = ''
          break
      }

      return text
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
      this.filter.DocumentVersion = ''
      this.filter.DocumentOwner = ''
      this.GetDocumentList()
    },
    getStatus: function (item) {
      if (item.IsInvalid) return '作廢'
      if (item.IsPublish) return '已發佈'
      return '未發佈'
    },
    openExportReportPopup: function () {
      this.IsShowReportPopup = true
      this.ReportEffectiveDate = ''
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
    },
    formatEffectiveDate: {
      get () {
        return this.yearFormat(this.EffectiveDate)
      }
    },
    formatInvalidDate: {
      get () {
        return this.yearFormat(this.InvalidDate)
      }
    },
    EditEffectiveDate: {
      get () {
        return this.yearFormat(this.Edit.EffectiveDate)
      }
    },
    VersionEditEffectiveDate: {
      get () {
        return this.yearFormat(this.VersionEdit.EffectiveDate)
      }
    },
    formatReportEffectiveDate: {
      get () {
        return this.yearFormat(this.ReportEffectiveDate)
      }
    }
  },
  watch: {
    uploadDialog (isOpen) {
      if (!isOpen) {
        if (this.$refs.uploadForm) this.$refs.uploadForm.reset()

        this.EffectiveDate = ''
        this.PublishDate = ''
        this.DocumentType = null
        this.DocumentName = ''
        this.DocumentNumber = ''
        this.DocumentVersion = ''
        this.DocumentOwner = ''
        this.previewFilesData = null
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
