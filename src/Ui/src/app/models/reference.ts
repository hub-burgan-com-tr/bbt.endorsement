export interface IReference {
  process: string;
  state: string;
  processNo: string;
  callback: {
    mode: string,
    url: string
  }
}

export default class Reference implements IReference {
  callback: { mode: string; url: string };
  process: string;
  processNo: string;
  state: string;

}
