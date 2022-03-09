import {Injectable} from '@angular/core';
import {NewApprovalOrder} from "../models/new-approval-order";

@Injectable({
  providedIn: 'root'
})
export class NewOrderService {

  constructor() {
  }

  setModel(model: NewApprovalOrder) {
    localStorage.setItem('newApprovalOrder', JSON.stringify(model));
  }

  getModel() {
    const model = localStorage.getItem('newApprovalOrder');
    if (model)
      return JSON.parse(model);
    return undefined;
  }
}
