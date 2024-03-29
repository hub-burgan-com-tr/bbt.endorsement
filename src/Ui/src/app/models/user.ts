export interface User {
  "customerNumber": number,
  "citizenshipNumber": number,
  "token": string,
  "name": {
    "first": string,
    "last": string
  },
  "isStaff": boolean,
  "statusCode": number,
  "authory": {
    "isReadyFormCreator": boolean,
    "isNewFormCreator": boolean,
    "isFormReader": boolean,
    "isBranchFormReader": boolean,
    "isBranchApproval": boolean,
    "isUIVisible": boolean
  },
  "email": string,
  "gsmPhone": string,
  "data": any,
  "branchCode": string,
  "businessLine": string
}
