interface IGetEndorsementListRequestModel {
  pageNumber: number;
  pageSize: number;
}

export class GetEndorsementListRequestModel implements IGetEndorsementListRequestModel {
  pageNumber: number;
  pageSize: number;
}

interface IGetApprovalDetailRequestModel {
  orderId: any;
}

export class GetApprovalDetailRequestModel implements IGetApprovalDetailRequestModel {
  orderId: any;
}
