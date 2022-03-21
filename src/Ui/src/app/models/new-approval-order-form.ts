import Reference from './reference';
import Approver from "./approver";

interface INewApprovalOrderForm {
  title: string;
  formId: string;
  content: string;
  reference: Reference;
  approver: Approver
}

export default class NewApprovalOrderForm implements INewApprovalOrderForm {
  approver: Approver;
  content: string;
  formId: string;
  reference: Reference;
  title: string;

  constructor(approver, content, formId, reference, title) {
    this.approver = approver;
    this.content = content;
    this.formId = formId;
    this.reference = reference;
    this.title = title;
  }
}
