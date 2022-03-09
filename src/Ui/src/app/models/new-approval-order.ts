interface ApproverInterface {
  type: string;
  value: string;
  nameSurname: string;
}

class Approver implements ApproverInterface {
  nameSurname: string;
  type: string;
  value: string;
}

interface DocumentInterface {
  documentType: number;
  options: [];
  files: File[];
  title: string;
  content: string;
  formId: string;
  identityNo: number;
  nameSurname: string;
}

class Document implements DocumentInterface {
  content: string;
  documentType: number;
  files: File[];
  formId: string;
  identityNo: number;
  nameSurname: string;
  options: any;
  title: string;
}

export interface NewApprovalOrderInterface {
  title: string;
  process: string;
  step: string;
  processNo: string;
  validity: string;
  reminderFrequency: string;
  reminderCount: number;
  documents: Document[];
  approver: Approver;
}

export class NewApprovalOrder implements NewApprovalOrderInterface {
  approver: Approver;
  documents: Document[];
  process: string;
  processNo: string;
  reminderCount: number;
  reminderFrequency: string;
  step: string;
  title: string;
  validity: string;

  constructor() {
    this.approver = new Approver();
    this.documents = new Array<Document>();
  }
}
