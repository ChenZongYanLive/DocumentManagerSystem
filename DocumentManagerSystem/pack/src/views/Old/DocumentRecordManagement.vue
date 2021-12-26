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
              <v-btn class="ml-1" color="primary" dark @click.stop="uploadDialog = true">上傳紀錄</v-btn>
              <v-btn class="ml-2" color="primary" dark @click.stop="MonthReportDialog = true" v-if="$isRoleOk($identity.ExportReport)">匯出月報表</v-btn>
              <v-btn class="ml-2" color="primary" dark @click.stop="YearReportDialog = true" v-if="$isRoleOk($identity.ExportReport)">匯出年報表</v-btn>
              <v-btn class="ml-2" color="primary" dark @click.stop="MonthL2003_003ReportDialog = true" v-if="$isRoleOk($identity.ExportReport)">匯出紀錄彙整單</v-btn>
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
                          label="ISMS文件名稱"
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
                          v-model="filter.DocumentOwner"
                          @change="GetDocumentList"
                          name="DocumentOwner"
                          label="權責人員"
                        ></v-text-field>
                      </v-flex>
                      <v-flex xs2>
                        <!-- <v-text-field dense
                                                v-model="Edit.DocumentYearMonth"
                        label="資料年月"></v-text-field>-->
                        <v-dialog
                          ref="dialogStart"
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
                            <v-btn
                              text
                              color="primary"
                              @click="openDocumentYearMonthStartPicker = false"
                            >取消</v-btn>
                            <v-btn
                              text
                              color="primary"
                              @click="$refs.dialogStart.save(filter.DocumentYearMonthStart)"
                            >確認</v-btn>
                          </v-date-picker>
                        </v-dialog>
                      </v-flex>
                      <v-flex xs2>
                        <!-- <v-text-field dense
                                                v-model="Edit.DocumentYearMonth"
                        label="資料年月"></v-text-field>-->
                        <v-dialog
                          ref="dialogEnd"
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
                            <v-btn
                              text
                              color="primary"
                              @click="openDocumentYearMonthEndPicker = false"
                            >取消</v-btn>
                            <v-btn
                              text
                              color="primary"
                              @click="$refs.dialogEnd.save(filter.DocumentYearMonthEnd)"
                            >確認</v-btn>
                          </v-date-picker>
                        </v-dialog>
                      </v-flex>
                      <!-- <v-flex xs2>
                        <v-text-field
                          v-model="filter.DocumentStartNumber"
                          @change="GetDocumentList"
                          name="DocumentStartNumber"
                          label="紀錄編號 (起)"
                        ></v-text-field>
                      </v-flex>
                      <v-flex xs2>
                        <v-text-field
                          v-model="filter.DocumentEndNumber"
                          @change="GetDocumentList"
                          name="DocumentEndNumber"
                          label="紀錄編號 (迄)"
                        ></v-text-field>
                      </v-flex> -->
                      <!-- <v-flex xs2>
                        <v-menu
                          v-model="isShowStartDatePicker"
                          :close-on-content-click="false"
                          transition="scale-transition"
                          offset-y
                        >
                          <template v-slot:activator="{ on }">
                            <v-text-field
                              v-model="filterStartDate"
                              :label="'查詢起始日期'"
                              readonly
                              v-on="on"
                            ></v-text-field>
                          </template>
                          <v-date-picker
                            v-model="filter.StartDate"
                            locale="Language"
                            @change="GetDocumentList"
                            @input="isShowStartDatePicker = false"
                            :year-format="yearFormat"
                            :title-date-format="yearFormat"
                            :header-date-format="yearFormat"
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
                              v-model="filterEndDate"
                              :label="'查詢結束日期'"
                              readonly
                              v-on="on"
                            ></v-text-field>
                          </template>
                          <v-date-picker
                            v-model="filter.EndDate"
                            locale="Language"
                            @change="GetDocumentList"
                            @input="isShowEndDatePicker = false"
                            :year-format="yearFormat"
                            :title-date-format="yearFormat"
                            :header-date-format="yearFormat"
                          ></v-date-picker>
                        </v-menu>
                      </v-flex> -->
                      <!-- <v-flex xs3>
                        <v-text-field
                          v-model="filter.FileName"
                          @change="GetDocumentList"
                          name="FileName"
                          label="檔案名稱"
                        ></v-text-field>
                      </v-flex> -->
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
                    :sort-by="['DocumentYearMonth','DocumentStartNumber']"
                    multi-sort
                    class="elevation-1"
                    dense
                  >
                    <template v-slot:item.CreateTime="{ item }">{{ formatDate(item.CreateTime) }}</template>
                    <template
                      v-slot:item.DocumentYearMonth="{ item }"
                    >{{ ToFormatDocumentYearMonth(item.DocumentYearMonth) }}</template>
                    <template v-slot:item.IsPublish="{ item }">{{ item.IsPublish ? '已發佈' : '未發佈' }}</template>
                    <template v-slot:item.action="{ item }">
                      <v-btn small tile outlined color="primary" @click="download(item)">
                        <v-icon left style="cursor:pointer;">archive</v-icon>下載
                      </v-btn>
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
                      <v-btn
                        small
                        v-if="!item.IsPublish"
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
    <v-dialog v-model="uploadDialog" hide-overlay persistent width="500">
      <v-card>
        <v-card-title class="blue darken-2 white--text">上傳紀錄</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="uploadForm" style="width:100%">
              <v-flex xs12>
                <v-combobox
                  v-model="selectDocumentLib"
                  :items="DocumentLibList"
                  label="請選擇文件編號"
                  item-text="DocumentNumber"
                  item-value="Id"
                  outlined
                  dense
                  @change="SelectDocumentLibEvent">
                    <template v-slot:item="data">
                      {{ `${data.item.DocumentNumber} ${data.item.IsInvalid ? '(已作廢)' : ''}` }}
                    </template>
                    <template v-slot:selection="data">
                      {{ `${data.item.DocumentNumber} ${data.item.IsInvalid ? '(已作廢)' : ''}` }}
                    </template>
                </v-combobox>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentName" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="DocumentName"
                    label="ISMS文件名稱"
                    readonly
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentNumber" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="DocumentNumber"
                    readonly
                    label="文件編號"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <!-- <v-text-field dense
                                        v-model="DocumentYearMonth"
                label="資料年月"></v-text-field>-->
                <v-dialog
                  ref="addDialog"
                  v-model="openAddDocumentYearMonthPicker"
                  :return-value.sync="DocumentYearMonth"
                  persistent
                  width="290px"
                >
                  <template v-slot:activator="{ on }">
                    <ValidationProvider
                      name="DocumentYearMonth"
                      rules="required"
                      v-slot="{ errors }"
                    >
                      <v-text-field
                        :error-messages="errors[0]"
                        v-model="FormatDocumentYearMonth"
                        label="資料年月"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    v-model="DocumentYearMonth"
                    type="month"
                    :locale="Language"
                    scrollable
                    :year-format="yearFormat"
                    :title-date-format="yearFormat"
                    :header-date-format="yearFormat"
                  >
                    <v-spacer></v-spacer>
                    <v-btn text color="primary" @click="openAddDocumentYearMonthPicker = false">取消</v-btn>
                    <v-btn text color="primary" @click="$refs.addDialog.save(DocumentYearMonth)">確認</v-btn>
                  </v-date-picker>
                </v-dialog>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentCount" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="DocumentCount"
                    label="文件件數"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentPageCount" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="DocumentPageCount"
                    label="文件頁數"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <!-- <template v-if="IsSpecialDocument(DocumentNumber)">
                <v-flex xs12>
                  <v-text-field
                    dense
                    v-model="DocumentStartNumber"
                    outlined
                    readonly
                    disabled
                    label="紀錄編號 (起)"
                  ></v-text-field>
                </v-flex>
                <v-flex xs12>
                  <v-text-field
                    dense
                    v-model="DocumentEndNumber"
                    outlined
                    readonly
                    disabled
                    label="紀錄編號 (迄)"
                  ></v-text-field>
                </v-flex>
              </template> -->
              <template>
                <v-flex xs12>
                  <v-text-field
                    dense
                    readonly
                    outlined
                    :value="GetDocumentStartNumber (DocumentNumber, DocumentYearMonth, DocumentCount)"
                    label="紀錄編號 (起)"
                  ></v-text-field>
                </v-flex>
                <v-flex xs12>
                  <v-text-field
                    dense
                    readonly
                    outlined
                    :value="GetDocumentEndNumber (DocumentNumber, DocumentYearMonth, DocumentCount)"
                    label="紀錄編號 (迄)"
                  ></v-text-field>
                </v-flex>
              </template>
              <v-flex xs12>
                <ValidationProvider name="DocumentOwner" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="DocumentOwner"
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
    <v-dialog v-model="editDialog" hide-overlay persistent width="500">
      <v-card>
        <v-card-title class="blue darken-2 white--text">修改文件</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="uploadEditForm" style="width:100%">
              <v-flex xs12>
                <v-combobox
                  v-model="Edit.selectDocumentLib"
                  :items="DocumentLibList"
                  label="請選擇ISMS文件編號"
                  item-text="DocumentNumber"
                  item-value="Id"
                  outlined
                  dense
                  @change="SelectEditDocumentLibEvent"
                ></v-combobox>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentName" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentName"
                    label="ISMS文件名稱"
                    readonly
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
                    readonly
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <!-- <v-text-field dense
                                        v-model="Edit.DocumentYearMonth"
                label="資料年月"></v-text-field>-->
                <v-dialog
                  ref="editDialog"
                  v-model="openEditDocumentYearMonthPicker"
                  :return-value.sync="Edit.DocumentYearMonth"
                  persistent
                  width="290px"
                >
                  <template v-slot:activator="{ on }">
                    <ValidationProvider
                      name="DocumentYearMonth"
                      rules="required"
                      v-slot="{ errors }"
                    >
                      <v-text-field
                        :error-messages="errors[0]"
                        v-model="EditDocumentYearMonth"
                        label="資料年月"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    :year-format="yearFormat"
                    :title-date-format="yearFormat"
                    :header-date-format="yearFormat"
                    v-model="Edit.DocumentYearMonth"
                    type="month"
                    :locale="Language"
                    scrollable
                  >
                    <v-spacer></v-spacer>
                    <v-btn text color="primary" @click="openEditDocumentYearMonthPicker = false">取消</v-btn>
                    <v-btn
                      text
                      color="primary"
                      @click="$refs.editDialog.save(Edit.DocumentYearMonth)"
                    >確認</v-btn>
                  </v-date-picker>
                </v-dialog>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentCount" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentCount"
                    label="文件件數"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <v-flex xs12>
                <ValidationProvider name="DocumentPageCount" rules="required" v-slot="{ errors }">
                  <v-text-field
                    dense
                    :error-messages="errors[0]"
                    v-model="Edit.DocumentPageCount"
                    label="文件頁數"
                  ></v-text-field>
                </ValidationProvider>
              </v-flex>
              <!-- <template v-if="IsSpecialDocument(Edit.DocumentNumber)">
                <v-flex xs12>
                  <v-text-field
                    dense
                    v-model="Edit.DocumentStartNumber"
                    outlined
                    readonly
                    disabled
                    label="紀錄編號 (起)"
                  ></v-text-field>
                </v-flex>
                <v-flex xs12>
                  <v-text-field
                    dense
                    v-model="Edit.DocumentEndNumber"
                    outlined
                    readonly
                    disabled
                    label="紀錄編號 (迄)"
                  ></v-text-field>
                </v-flex>
              </template> -->
              <template>
                <v-flex xs12>
                  <v-text-field
                    dense
                    readonly
                    outlined
                    :value="GetDocumentStartNumber (Edit.DocumentNumber, Edit.DocumentYearMonth, Edit.DocumentCount)"
                    label="紀錄編號 (起)"
                  ></v-text-field>
                </v-flex>
                <v-flex xs12>
                  <v-text-field
                    dense
                    readonly
                    outlined
                    :value="GetDocumentEndNumber (Edit.DocumentNumber, Edit.DocumentYearMonth, Edit.DocumentCount)"
                    label="紀錄編號 (迄)"
                  ></v-text-field>
                </v-flex>
              </template>
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
          上傳紀錄中
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
    <v-dialog v-model="MonthReportDialog" hide-overlay persistent width="300">
      <v-card>
        <v-card-title class="blue darken-2 white--text">匯出月報</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="monthReportForm" style="width:100%">
              <v-flex xs12>
                <!-- <v-text-field dense
                                        v-model="DocumentYearMonth"
                label="資料年月"></v-text-field>-->
                <v-dialog
                  ref="monthReportYearMonthPicker"
                  v-model="openMonthReportYearMonthPicker"
                  :return-value.sync="MonthReportYearMonth"
                  persistent
                  width="290px"
                >
                  <template v-slot:activator="{ on }">
                    <ValidationProvider
                      name="DocumentYearMonth"
                      rules="required"
                      v-slot="{ errors }"
                    >
                      <v-text-field
                        :error-messages="errors[0]"
                        v-model="FormatMonthReportYearMonth"
                        label="資料年月"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    v-model="MonthReportYearMonth"
                    type="month"
                    :locale="Language"
                    scrollable
                    :year-format="yearFormat"
                    :title-date-format="yearFormat"
                    :header-date-format="yearFormat"
                  >
                    <v-spacer></v-spacer>
                    <v-btn text color="primary" @click="openMonthReportYearMonthPicker = false">取消</v-btn>
                    <v-btn text color="primary" @click="$refs.monthReportYearMonthPicker.save(MonthReportYearMonth)">確認</v-btn>
                  </v-date-picker>
                </v-dialog>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="MonthReportDialog = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click.stop="exportMonthReport">匯出</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="MonthL2003_003ReportDialog" hide-overlay persistent width="300">
      <v-card>
        <v-card-title class="blue darken-2 white--text">匯出資訊安全管理紀錄彙整單</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="monthL2003_003ReportForm" style="width:100%">
              <v-flex xs12>
                <!-- <v-text-field dense
                                        v-model="DocumentYearMonth"
                label="資料年月"></v-text-field>-->
                <v-dialog
                  ref="monthL2003_003ReportYearMonthPicker"
                  v-model="openMonthL2003_003ReportYearMonthPicker"
                  :return-value.sync="MonthL2003_003ReportYearMonth"
                  persistent
                  width="290px"
                >
                  <template v-slot:activator="{ on }">
                    <ValidationProvider
                      name="DocumentYearMonth"
                      rules="required"
                      v-slot="{ errors }"
                    >
                      <v-text-field
                        :error-messages="errors[0]"
                        v-model="FormatMonthL2003_003ReportYearMonth"
                        label="資料年月"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    v-model="MonthL2003_003ReportYearMonth"
                    type="month"
                    :locale="Language"
                    scrollable
                    :year-format="yearFormat"
                    :title-date-format="yearFormat"
                    :header-date-format="yearFormat"
                  >
                    <v-spacer></v-spacer>
                    <v-btn text color="primary" @click="openMonthL2003_003ReportYearMonthPicker = false">取消</v-btn>
                    <v-btn text color="primary" @click="$refs.monthL2003_003ReportYearMonthPicker.save(MonthL2003_003ReportYearMonth)">確認</v-btn>
                  </v-date-picker>
                </v-dialog>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="MonthL2003_003ReportDialog = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click.stop="exportMonthL2003_003Report">匯出</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    <v-dialog v-model="YearReportDialog" hide-overlay persistent width="300">
      <v-card>
        <v-card-title class="blue darken-2 white--text">匯出年報</v-card-title>
        <v-container grid-list-sm>
          <v-layout row wrap>
            <ValidationObserver ref="yearReportForm" style="width:100%">
              <v-flex xs12>
                <v-dialog
                  ref="yearReportYearPicker"
                  v-model="openYearReportYearPicker"
                  :return-value.sync="MonthReportYearMonth"
                  persistent
                  width="290px"
                >
                  <template v-slot:activator="{ on }">
                    <ValidationProvider
                      name="Year"
                      rules="required"
                      v-slot="{ errors }"
                    >
                      <v-text-field
                        :error-messages="errors[0]"
                        v-model="FormatDocumentYear"
                        label="資料年"
                        prepend-icon="event"
                        readonly
                        v-on="on"
                      ></v-text-field>
                    </ValidationProvider>
                  </template>
                  <v-date-picker
                    ref="yearPicker"
                    v-model="YearReport"

                    reactive
                    :locale="Language"
                    scrollable
                    :year-format="yearFormat"
                    :title-date-format="yearFormat"
                    :header-date-format="yearFormat"
                    @input="saveYearPicker"
                  >
                    <!-- <v-spacer></v-spacer>
                    <v-btn text color="primary" @click="openYearReportYearPicker = false">取消</v-btn>
                    <v-btn text color="primary" @click="saveYearPicker(YearReport)">確認</v-btn> -->
                  </v-date-picker>
                </v-dialog>
              </v-flex>
            </ValidationObserver>
          </v-layout>
        </v-container>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn type="submit" color="red" dark @click="YearReportDialog = false">取消</v-btn>
          <v-btn type="submit" color="primary" dark @click.stop="exportYearReport">匯出</v-btn>
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
      openDocumentYearMonthStartPicker: false,
      openDocumentYearMonthEndPicker: false,
      openAddDocumentYearMonthPicker: false,
      openEditDocumentYearMonthPicker: false,
      openMonthReportYearMonthPicker: false,
      openMonthL2003_003ReportYearMonthPicker: false,
      openYearReportYearPicker: false,
      DocumentList: [],
      pagination: {
        rowsPerPage: -1,
        descending: false
      },
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
        {
          text: '檔案名稱',
          align: 'start',
          sortable: false,
          value: 'FileName'
        },
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

      filter: {
        FileName: '',
        StartDate: '',
        EndDate: '',
        DocumentType: 5,
        DocumentName: '',
        DocumentNumber: '',
        DocumentStartNumber: '',
        DocumentEndNumber: '',
        DocumentYearMonthStart: '',
        DocumentYearMonthEnd: '',
        DocumentCount: '',
        DocumentPageCount: '',
        DocumentOwner: ''
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
      DocumentStartNumber: '',
      DocumentEndNumber: '',
      DocumentYearMonth: '',
      DocumentCount: '',
      DocumentPageCount: '',
      DocumentOwner: '',
      currentUploadFile: null,
      selectDocumentLib: null,

      editDialog: false,
      Edit: {
        Id: -1,
        EffectiveDate: '',
        PublishDate: '',
        DocumentType: 5,
        DocumentName: '',
        DocumentNumber: '',
        DocumentStartNumber: '',
        DocumentEndNumber: '',
        DocumentYearMonth: '',
        DocumentCount: '',
        DocumentPageCount: '',
        DocumentOwner: '',
        IsReUploadFile: false,
        currentUploadFile: null,
        selectDocumentLib: null
      },
      DocumentLibList: [],

      MonthReportDialog: false,
      MonthReportYearMonth: '',
      YearReportDialog: false,
      YearReport: '2020',
      MonthL2003_003ReportDialog: false,
      MonthL2003_003ReportYearMonth: ''
    }
  },
  created: function () {},
  mounted () {
    this.GetDocumentList()
    this.GetDocumentLibList()
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
      this.Edit.DocumentStartNumber = item.DocumentStartNumber
      this.Edit.DocumentEndNumber = item.DocumentEndNumber
      this.Edit.DocumentYearMonth = item.DocumentYearMonth
      this.Edit.DocumentCount = item.DocumentCount
      this.Edit.DocumentPageCount = item.DocumentPageCount
      this.Edit.DocumentOwner = item.DocumentOwner
      this.Edit.IsReUploadFile = false
      this.Edit.selectDocumentLib = null

      this.editDialog = true
    },
    upload: async function () {
      let self = this

      let uploadFile = this.currentUploadFile

      const isValid = await self.$refs.uploadForm.validate()

      if (!isValid) return

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
      let DocumentNumber = this.DocumentNumber
      let DocumentYearMonth = this.DocumentYearMonth
      let DocumentCount = this.DocumentCount
      let DocumentPageCount = this.DocumentPageCount
      let DocumentOwner = this.DocumentOwner

      var isSpecial = this.IsSpecialDocument(this.DocumentNumber)

      let DocumentStartNumber = !isSpecial
        ? DocumentNumber + '-' + DocumentYearMonth.replace('-', '') + '-' + '001'
        : this.DocumentStartNumber
      let DocumentEndNumber = !isSpecial
        ? DocumentNumber +
        '-' +
        DocumentYearMonth.replace('-', '') +
        '-' +
        this.padLeft(DocumentCount, 3)
        : this.DocumentEndNumber

      self.loadingDialog = true

      let formData = new FormData()

      formData.append('file', uploadFile)
      formData.append('EffectiveDate', effectiveDate)
      formData.append('PublishDate', publishDate)
      formData.append('DocumentName', documentName)
      formData.append('DocumentNumber', DocumentNumber)
      formData.append('DocumentStartNumber', DocumentStartNumber)
      formData.append('DocumentEndNumber', DocumentEndNumber)
      formData.append('DocumentYearMonth', DocumentYearMonth)
      formData.append('DocumentCount', DocumentCount)
      formData.append('DocumentPageCount', DocumentPageCount)
      formData.append('DocumentOwner', DocumentOwner)
      formData.append('DocumentType', documentType)

      let checkIsSameRecordDocumentByYearMonth = await axios.post('/api/Document/CheckIsSameRecordDocumentByYearMonth',
        {
          DocumentNumber: DocumentNumber,
          DocumentYearMonth: DocumentYearMonth
        }
      )

      let isSameDocumentNumber = false

      isSameDocumentNumber = checkIsSameRecordDocumentByYearMonth.data

      if (isSameDocumentNumber) {
        self.AlertDialogMessage = `${DocumentYearMonth}已經有此文件編號(${DocumentNumber})的檔案了，請確認!!`
        self.IsOpenAlertDialog = true
        setTimeout(() => {
          self.loadingDialog = false
          self.uploadDialog = false
          self.IsOpenAlertDialog = false
        }, 1000)
        return
      }

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
      let uploadFile = this.Edit.currentUploadFile

      const isValid = await self.$refs.uploadEditForm.validate()

      if (!isValid) return

      if (uploadFile == null && this.Edit.IsReUploadFile) {
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
      let DocumentNumber = this.Edit.DocumentNumber

      let DocumentYearMonth = this.Edit.DocumentYearMonth
      let DocumentCount = this.Edit.DocumentCount
      let DocumentPageCount = this.Edit.DocumentPageCount
      let DocumentOwner = this.Edit.DocumentOwner

      var isSpecial = this.IsSpecialDocument(this.Edit.DocumentNumber)

      let DocumentStartNumber = !isSpecial
        ? DocumentNumber + '-' + DocumentYearMonth.replace('-', '') + '-' + '001'
        : this.Edit.DocumentStartNumber
      let DocumentEndNumber = !isSpecial
        ? DocumentNumber +
        '-' +
        DocumentYearMonth.replace('-', '') +
        '-' +
        this.padLeft(DocumentCount, 3)
        : this.Edit.DocumentEndNumber

      let id = this.Edit.Id

      self.loadingDialog = true

      let formData = new FormData()

      if (this.Edit.IsReUploadFile) {
        formData.append('file', uploadFile)
      }

      formData.append('EffectiveDate', effectiveDate)
      formData.append('PublishDate', publishDate)
      formData.append('DocumentName', documentName)
      formData.append('DocumentNumber', DocumentNumber)
      formData.append('DocumentStartNumber', DocumentStartNumber)
      formData.append('DocumentEndNumber', DocumentEndNumber)
      formData.append('DocumentYearMonth', DocumentYearMonth)
      formData.append('DocumentCount', DocumentCount)
      formData.append('DocumentPageCount', DocumentPageCount)
      formData.append('DocumentOwner', DocumentOwner)
      formData.append('DocumentType', documentType)
      formData.append('Id', id)

      let checkIsSameRecordDocumentByYearMonth = await axios.post(`/api/Document/CheckIsSameRecordDocumentByYearMonth/${id}`,
        {
          DocumentNumber: DocumentNumber,
          DocumentYearMonth: DocumentYearMonth
        }
      )

      let isSameDocumentNumber = false

      isSameDocumentNumber = checkIsSameRecordDocumentByYearMonth.data

      if (isSameDocumentNumber) {
        self.AlertDialogMessage = `${DocumentYearMonth}已經有此文件編號(${DocumentNumber})的檔案了，請確認!!`
        self.IsOpenAlertDialog = true
        setTimeout(() => {
          self.loadingDialog = false
          self.uploadDialog = false
          self.IsOpenAlertDialog = false
        }, 1000)
        return
      }

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
    deleteDocument: function (item) {
      if (confirm('確定要刪除此紀錄嗎 ? ')) {
        let self = this
        axios.post('/api/Document/Delete', item).then((response) => {
          self.GetDocumentList()
        })
      }
    },
    formatDate (dateTime) {
      let dateStr = moment(dateTime).format('YYYY/MM/DD hh:mm')
      let dateArray = dateStr.split('/')
      if (dateArray.length === 3) {
        return `${dateArray[0] * 1 - 1911}/${dateArray[1]}/${dateArray[2]}`
      }
      return dateStr
    },
    ToFormatDocumentYearMonth (documentYearMonth) {
      if (documentYearMonth) {
        let dateStr = documentYearMonth
        let dateArray = dateStr.split('-')
        if (dateArray.length === 2) {
          return `${dateArray[0] * 1 - 1911}-${dateArray[1]}`
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
    padLeft (str, len) {
      str = '' + str
      if (str.length >= len) {
        return str
      } else {
        return this.padLeft('0' + str, len)
      }
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
    exportMonthReport: function () {
      let self = this
      self.ExportFileDialog = true
      axios
        .post('/api/Document/ExportRecordDocumentMonthReport', { DocumentYearMonth: self.MonthReportYearMonth, DocumentType: 5 }, {
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
            elink.download = '桃園市政府地方稅務局 紀錄統計表.xlsx'
            elink.style.display = 'none'
            elink.href = URL.createObjectURL(blob)
            document.body.appendChild(elink)
            elink.click()
            URL.revokeObjectURL(elink.href)
            document.body.removeChild(elink)
          } else {
            navigator.msSaveBlob(blob, '桃園市政府地方稅務局 紀錄統計表.xlsx')
          }
          self.MonthReportDialog = false
        })
        .finally(() => {
          self.ExportFileDialog = false
        })
    },
    exportMonthL2003_003Report: function () {
      let self = this
      self.ExportFileDialog = true
      axios
        .post('/api/Document/ExportRecordDocumentMonthPDFReport', { DocumentYearMonth: self.MonthL2003_003ReportYearMonth, DocumentType: 5 }, {
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
            elink.download = '桃園市政府地方稅務局 資訊安全管理紀錄彙整單.pdf'
            elink.style.display = 'none'
            elink.href = URL.createObjectURL(blob)
            document.body.appendChild(elink)
            elink.click()
            URL.revokeObjectURL(elink.href)
            document.body.removeChild(elink)
          } else {
            navigator.msSaveBlob(blob, '桃園市政府地方稅務局 資訊安全管理紀錄彙整單.pdf')
          }
          self.MonthL2003_003ReportDialog = false
        })
        .finally(() => {
          self.ExportFileDialog = false
        })
    },
    exportYearReport: function () {
      let self = this
      self.ExportFileDialog = true
      axios
        .post('/api/Document/ExportRecordDocumentYearReport', { Year: self.YearReport * 1, DocumentType: 5 }, {
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
            elink.download = '桃園市政府地方稅務局 紀錄統計表.xlsx'
            elink.style.display = 'none'
            elink.href = URL.createObjectURL(blob)
            document.body.appendChild(elink)
            elink.click()
            URL.revokeObjectURL(elink.href)
            document.body.removeChild(elink)
          } else {
            navigator.msSaveBlob(blob, '桃園市政府地方稅務局 紀錄統計表.xlsx')
          }
          self.YearReportDialog = false
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
    GetDocumentLibList: function () {
      let self = this
      axios.get('/api/Document/GetDocumentIndex').then((response) => {
        self.DocumentLibList = response.data
      })
    },
    SelectDocumentLibEvent: function (item) {
      // console.log(item)
      let isPickUp = true
      if (item.IsInvalid) {
        isPickUp = confirm('此文件編號已作廢，確定要選取次編號嗎 ?')
      }

      if (isPickUp) {
        let isSpecialDocument = this.IsSpecialDocument(item.DocumentNumber)
        if (isSpecialDocument) {
          this.DocumentStartNumber = item.DocumentNumber// .replace('XX', '').replace('xx', '')
          this.DocumentEndNumber = item.DocumentNumber// .replace('XX', '').replace('xx', '')
        }
        this.DocumentName = item.DocumentName
        this.DocumentNumber = item.DocumentNumber
        this.DocumentOwner = item.DocumentOwner
      } else {
        this.$nextTick(function () {
          this.selectDocumentLib = null
        })
      }
    },
    SelectEditDocumentLibEvent: function (item) {
      let isPickUp = true
      if (item.IsInvalid) {
        isPickUp = confirm('此文件編號已作廢，確定要選取次編號嗎 ?')
      }

      if (isPickUp) {
        let isSpecialDocument = this.IsSpecialDocument(item.DocumentNumber)
        if (isSpecialDocument) {
          this.Edit.DocumentStartNumber = item.DocumentNumber// .replace('XX', '').replace('xx', '')
          this.Edit.DocumentEndNumber = item.DocumentNumber// .replace('XX', '').replace('xx', '')
        }
        this.Edit.DocumentName = item.DocumentName
        this.Edit.DocumentNumber = item.DocumentNumber
        this.Edit.DocumentOwner = item.DocumentOwner
      } else {
        this.$nextTick(function () {
          this.selectDocumentLib = null
        })
      }
    },
    cleanFilter () {
      this.filter.FileName = ''
      this.filter.StartDate = ''
      this.filter.EndDate = ''
      this.filter.DocumentType = 5
      this.filter.DocumentName = ''
      this.filter.DocumentNumber = ''
      this.filter.DocumentStartNumber = ''
      this.filter.DocumentEndNumber = ''
      this.filter.DocumentYearMonthStart = ''
      this.filter.DocumentYearMonthEnd = ''
      this.filter.DocumentCount = ''
      this.filter.DocumentPageCount = ''
      this.filter.DocumentOwner = ''
      this.GetDocumentList()
    },
    GetDocumentStartNumber (DocumentNumber, DocumentYearMonth, DocumentCount) {
      let isSpecialDocument = this.IsSpecialDocument(DocumentNumber)

      if (isSpecialDocument) {
        let year = DocumentYearMonth.split('-')[0]
        return DocumentNumber.replace('YYYY', year).replace('XX', this.padLeft(1, 2))
      } else {
        return DocumentNumber + '-' + DocumentYearMonth.replace('-', '') + '-' + '001'
      }
    },
    GetDocumentEndNumber (DocumentNumber, DocumentYearMonth, DocumentCount) {
      let isSpecialDocument = this.IsSpecialDocument(DocumentNumber)

      if (isSpecialDocument) {
        let year = DocumentYearMonth.split('-')[0]
        return DocumentNumber.replace('YYYY', year).replace('XX', this.padLeft(DocumentCount, 2))
      } else {
        return DocumentNumber + '-' + DocumentYearMonth.replace('-', '') + '-' + this.padLeft(DocumentCount, 3)
      }
    },
    saveYearPicker (date) {
      this.$refs.yearReportYearPicker.save(date.split('-')[0])
      this.$refs.yearPicker.activePicker = 'YEAR'
      this.openYearReportYearPicker = false
      this.YearReport = date.split('-')[0]
    }
  },
  computed: {
    Language () {
      return 'zh-TW'
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
    FormatDocumentYearMonth: {
      get () {
        return this.yearFormat(this.DocumentYearMonth)
      }
    },
    FormatDocumentYear: {
      get () {
        return this.yearFormat(this.YearReport)
      }
    },
    EditDocumentYearMonth: {
      get () {
        return this.yearFormat(this.Edit.DocumentYearMonth)
      }
    },
    FormatMonthReportYearMonth: {
      get () {
        return this.yearFormat(this.MonthReportYearMonth)
      }
    },
    FormatMonthL2003_003ReportYearMonth: {
      get () {
        return this.yearFormat(this.MonthL2003_003ReportYearMonth)
      }
    }
  },
  watch: {
    uploadDialog (isOpen) {
      if (!isOpen) {
        if (this.$refs.uploadForm) this.$refs.uploadForm.reset()

        this.EffectiveDate = ''
        this.PublishDate = ''
        this.DocumentName = ''
        this.DocumentNumber = ''
        this.DocumentStartNumber = ''
        this.DocumentEndNumber = ''
        this.DocumentYearMonth = ''
        this.DocumentCount = ''
        this.DocumentPageCount = ''
        this.DocumentOwner = ''
        this.selectDocumentLib = null
      }
    },
    MonthReportDialog (isOpen) {
      if (!isOpen) {
        if (this.$refs.monthReportForm) this.$refs.monthReportForm.reset()

        this.MonthReportYearMonth = ''
      }
    },
    MonthL2003_003ReportDialog (isOpen) {
      if (!isOpen) {
        if (this.$refs.monthL2003_003ReportForm) this.$refs.monthL2003_003ReportForm.reset()

        this.MonthL2003_003ReportYearMonth = ''
      }
    },
    // selectDocumentLib (val) {
    //   if (val) {
    //     if (this.DocumentLibList && this.DocumentLibList.length > 0) {
    //       var docLib = this.DocumentLibList.find(function (item, index, array) {
    //         return item.Id === val.Id
    //       })
    //       if (docLib) {
    //         this.DocumentName = docLib.DocumentName
    //         this.DocumentNumber = docLib.DocumentNumber
    //         this.DocumentOwner = docLib.DocumentOwner
    //       }
    //     }
    //   }
    // },
    'Edit.selectDocumentLib': function (val) {
      if (val) {
        if (this.DocumentLibList && this.DocumentLibList.length > 0) {
          var docLib = this.DocumentLibList.find(function (item, index, array) {
            return item.Id === val.Id
          })
          if (docLib) {
            this.Edit.DocumentName = docLib.DocumentName
            this.Edit.DocumentNumber = docLib.DocumentNumber
            this.Edit.DocumentOwner = docLib.DocumentOwner
          }
        }
      }
    },
    openYearReportYearPicker (val) {
      val && this.$nextTick(() => (this.$refs.yearPicker.activePicker = 'YEAR'))
    }
  }
}
</script>
