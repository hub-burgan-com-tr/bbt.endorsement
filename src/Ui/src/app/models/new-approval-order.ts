import Approver from "./approver";
import Reference from "./reference";

interface IDocument {
  type: number;
  actions: [];
  file: string;
  fileName: string;
  title: string;
  content: string;
  formId: string;
  identityNo: number;
  nameSurname: string;
}

class Document implements IDocument {
  content: string;
  type: number;
  file: string;
  fileName: string;
  formId: string;
  identityNo: number;
  nameSurname: string;
  actions: any;
  title: string;
}

interface IConfig {
  maxRetryCount: number;
  retryFrequence: string;
  expireInMinutes: number;
  notifyMessageSMS: string;
  notifyMessagePush: string;
  renotifyMessageSMS: string;
  renotifyMessagePush: string;
}

class Config implements IConfig {
  expireInMinutes: number;
  maxRetryCount: number;
  notifyMessagePush: string;
  notifyMessageSMS: string;
  renotifyMessagePush: string;
  renotifyMessageSMS: string;
  retryFrequence: string;
}


interface INewApprovalOrder {
  title: string;
  reference: Reference;
  config: Config;
  documents: Document[];
  approver: Approver;
}

export class NewApprovalOrder implements INewApprovalOrder {
  approver: Approver;
  config: Config;
  documents: Document[];
  reference: Reference;
  title: string;

  constructor() {
    this.approver = new Approver();
    this.documents = new Array<Document>();
  }
}
