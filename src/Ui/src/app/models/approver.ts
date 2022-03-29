export interface IApprover {
  citizenshipNumber: string;
  first: string;
  last: string;
}

export default class Approver implements IApprover {
  citizenshipNumber: string;
  first: string;
  last: string;
}
