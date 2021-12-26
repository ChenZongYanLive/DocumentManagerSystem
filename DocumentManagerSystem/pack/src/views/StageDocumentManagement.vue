<template>
  <v-container
    id="dashboard-view"
    fluid
    tag="section"
  >
    <v-row>
      <v-col
        cols="12"
        md="12"
      >
        <material-card
          color="orange"
          full-header
        >
          <template #heading>
            <div class="pa-8 white--text">
              <div class="text-h4 font-weight-light">
                ISMS文件
              </div>
              <div class="text-caption">

              </div>
            </div>
          </template>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="items"
            />
          </v-card-text>
        </material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  data () {
    return {
      headers: [
        {
          sortable: false,
          text: '文件名稱',
          value: 'DocumentName'
        },
        {
          sortable: false,
          text: '文件編號',
          value: 'Name'
        },
        {
          sortable: false,
          text: '版號',
          value: 'Version',
          align: 'right'
        },
        {
          sortable: false,
          text: '生效日期',
          value: 'EffDate',
          align: 'right'
        },
        {
          sortable: false,
          text: '權責人員',
          value: 'OwnerUser',
          align: 'right'
        },
        {
          sortable: false,
          text: '檔案名稱',
          value: 'FileName',
          align: 'right'
        },
        {
          sortable: false,
          text: '附檔名',
          value: 'FileType',
          align: 'right'
        },
        {
          sortable: false,
          text: '文件階層',
          value: 'Stage',
          align: 'right'
        },
        {
          sortable: false,
          text: '狀態',
          value: 'Status',
          align: 'right'
        },
        {
          sortable: false,
          text: '動作',
          value: 'Action',
          align: 'right'
        }
      ],
      items: []
    }
  },
  methods: {
    name () {

    }
  }
}
</script>
