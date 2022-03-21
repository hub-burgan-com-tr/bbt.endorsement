export interface IApprover {
  type: string;
  value: string;
  nameSurname: string;
}

export default class Approver implements IApprover {
  nameSurname: string;
  type: string;
  value: string;
}
