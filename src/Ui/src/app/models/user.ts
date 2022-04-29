export interface User {
  "clientNumber": number,
  "citizenshipNumber": number,
  "token": string,
  "name": {
    "first": string,
    "last": string
  },
  "isSube": boolean,
  "authory": {
    "isReadyFormCreator": boolean,
    "isNewFormCreator": boolean,
    "isFormReader": boolean,
    "isBranchFormReader": boolean,
    "isBranchApproval": boolean
  }
}
