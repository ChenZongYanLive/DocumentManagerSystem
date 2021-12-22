
// import cookieHelper from './cookieHelper'
// v-if="$isRoleOk($identity.SETTING)"
// :disabled="!$isRoleOk($identity.SETTING_ORGAIZATION_UPDATE_COMPANY_COMPANYCODE)"
export function isRoleOk (rule) {
  // let strUserRoles = cookieHelper.getCookie(cookieHelper.IDENTITY_ROLES)
  let strUserRoles = window.localStorage.getItem('roles')

  let verificationRule = rule

  if (strUserRoles) {
    let userRoles = JSON.parse(strUserRoles)
    var isOK = CheckRuleIsOk(verificationRule, userRoles)
    return isOK
  }

  return false
}

export const directiveCheckIsEdit = {
  bind (el, binding, vnode) {
    // let strUserRoles = vnode.context.$cookieHelper.getCookie(vnode.context.$cookieHelper.IDENTITY_ROLES)
    let strUserRoles = window.localStorage.getItem('roles')
    let verificationRule = binding.value

    if (strUserRoles) {
      let userRoles = JSON.parse(strUserRoles)
      CheckRuleIsOk(verificationRule, userRoles)
    }
  },
  updated () {

  }
}

function CheckRuleIsOk (verificationRule, userRoles) {
  let userRolesLength = userRoles.length

  let oneOfTheRoleList = verificationRule.OneOfThe
  let oneOfTheRoleListLength = oneOfTheRoleList.length

  if (oneOfTheRoleListLength > 0) {
    let hasOneOfRole = false
    for (let i = 0; i < userRolesLength; i++) {
      let _roles = $.grep(oneOfTheRoleList, function (item) {
        return item === userRoles[i]
      })

      if (_roles.length > 0) {
        hasOneOfRole = true
        break
      }
    }

    if (!hasOneOfRole) {
      return false
    }
  }

  let includeRoleList = verificationRule.Include
  let includeRoleListLength = includeRoleList.length

  if (includeRoleListLength > 0) {
    let hasRole = true
    for (let i = 0; i < includeRoleListLength; i++) {
      let _roles = $.grep(userRoles, function (item) {
        return item === includeRoleList[i]
      })

      if (_roles.length === 0) {
        hasRole = false
        break
      }
    }

    if (!hasRole) {
      return false
    }
  }

  return true
}
