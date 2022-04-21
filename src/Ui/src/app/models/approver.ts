export interface IApprover {
  citizenshipNumber: number;
  first: string;
  last: string;
  clientNumber : string;
}

export default class Approver implements IApprover {
  citizenshipNumber: number;
  first: string;
  last: string;
  clientNumber : string;
}
