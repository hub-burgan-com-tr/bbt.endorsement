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
  fileType: string;
  formId: string;
  reference: Reference;
  title: string;
  source: string;
  dependencyOrderId: string;

  constructor(approver, content, fileType, formId, reference, title, insuranceType, source, dependencyOrderId) {
    this.approver = approver;
    this.content = content;
    this.fileType = fileType;
    this.formId = formId;
    this.reference = reference;
    this.title = title;
    this.insuranceType = insuranceType;
    this.source = source;
    this.dependencyOrderId = dependencyOrderId;
  }
}
