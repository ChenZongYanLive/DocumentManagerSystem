const AND = 'AND'

const OR = 'OR'

const God = 'God'

const Admin = 'Admin'

const Manager = 'Manager'

const Member = 'Member'

const IT = 'IT'

const IT_EDIT = 'IT-Edit'

export default class identity {
  static get AND () {
    return AND
  }
  static get OR () {
    return OR
  }
  static get God () {
    return God
  }
  static get Admin () {
    return Admin
  }
  static get Manager () {
    return Manager
  }
  static get Member () {
    return Member
  }
  static get IT () {
    return IT
  }
  static get IT_EDIT () {
    return IT_EDIT
  }
  static get StageDocumentSearch () {
    return {
      OneOfThe: [God, Admin, Manager, Member, IT, IT_EDIT],
      Include: []
    }
  }
  static get DocumentRecordSearch () {
    return {
      OneOfThe: [God, Admin, Manager, IT, IT_EDIT],
      Include: []
    }
  }
  static get StageDocumentManagement () {
    return {
      OneOfThe: [God, Admin, Manager],
      Include: []
    }
  }
  static get DocumentRecordManagement () {
    return {
      OneOfThe: [God, Admin, Manager, IT_EDIT],
      Include: []
    }
  }
  static get AccountManager () {
    return {
      OneOfThe: [God, Admin, Manager],
      Include: []
    }
  }
  static get DeleteDocument () {
    return {
      OneOfThe: [God, Admin],
      Include: []
    }
  }
  static get InvalidDocument () {
    return {
      OneOfThe: [God, Admin, Manager],
      Include: []
    }
  }
  static get reInvalidDocument () {
    return {
      OneOfThe: [God, Admin],
      Include: []
    }
  }
  static get DocumentLibrary () {
    return {
      OneOfThe: [God, Admin],
      Include: []
    }
  }
  static get ExportReport () {
    return {
      OneOfThe: [God, Admin, Manager],
      Include: []
    }
  }
}
