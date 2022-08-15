﻿using Application.Endorsements.Commands.NewOrders;

namespace Application.Endorsements.Queries.GetApprovalsDetails
{
    public class GetApprovalDetailsDto
    {
        public List<OrderDocument>Documents { get; set; }
        public string Title { get; internal set; }
        public long CitizenShipNumber { get; set; }
      
        public string FirstAndSurname { get; set; }
    }
    public class DocumentAction
    {
        public string Value { get; set; }
        public string Title { get; set; }
        public string DocumentActionId { get; internal set; }
    }
    public class OrderDocument
    {
        public string DocumentId { get; internal set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public List<DocumentAction> Actions { get; set; }


    }
    public enum DocumentApprovedEnum
    {
        Approved=1,
        Rejected=2,      
    }

}
