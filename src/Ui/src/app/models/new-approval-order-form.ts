import Reference from './reference';
import Approver from "./approver";

interface INewApprovalOrderForm {
  title: string;
  formId: string;
  content: string;
  reference: Reference;
  approver: Approver;
  insuranceType: string;
}

export default class NewApprovalOrderForm implements INewApprovalOrderForm {
  insuranceType: string;
  approver: Approver;
  content: string;
  formId: string;
  reference: Reference;
  title: string;

  constructor(approver, content, formId, reference, title, insuranceType) {
    this.approver = approver;
    this.content = content;
    this.formId = formId;
    this.reference = reference;
    this.title = title;
    this.insuranceType = insuranceType;
  }
}
