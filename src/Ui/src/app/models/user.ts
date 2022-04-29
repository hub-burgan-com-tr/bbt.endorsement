export interface User {
  "clientNumber": number,
  "citizenshipNumber": number,
  "token": string,
  "name": {
    "first": string,
    "last": string
  },
  "isCustomer": boolean,
  "statusCode": number,
  "authory": {
    "isReadyFormCreator": boolean,
    "isNewFormCreator": boolean,
    "isFormReader": boolean,
    "isBranchFormReader": boolean,
    "isBranchApproval": boolean
  }
}
