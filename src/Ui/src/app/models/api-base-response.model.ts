interface IApiBaseResponseModel{
  data: any;
  message: string;
}

export class ApiBaseResponseModel implements  IApiBaseResponseModel{
  data: any;
  message: string;
}

interface IHttpErrorResponseModel{
  url: string;
  message: string;
  status: string;
  statusText: string;
}

export class HttpErrorResponseModel implements  IHttpErrorResponseModel{
  url: string;
  message: string;
  status: string;
  statusText: string;
}
