using Application.BbtInternals.Models;

namespace Application.BbtInternals.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryDto
    {
        public GetSearchPersonSummaryDto()
        {
            Authory = new AuthoryModel();
        }

        public string CitizenshipNumber { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public int CustomerNumber { get;  set; }
        public string Token { get; set; }
        public bool IsStaff { get; set; }
        public string TaxNo { get; set; }
        public AuthoryModel Authory { get; set; }
        public string Email { get; set; }
        public GsmPhones GsmPhone { get; set; }
        public string Data { get; set; }
        public string BranchCode { get; set; }
        public string BusinessLine { get; set; }

    }
    public class AuthoryModel
    {
        public bool IsReadyFormCreator { get; set; } // Form ile Emir Oluşturma
        public bool IsNewFormCreator { get; set; } //Yeni Onay Emri Oluşturma
        public bool IsFormReader { get; set; } // Tüm Onay Emirlerini İzleyebilir
        public bool IsBranchFormReader { get; set; } //Farklı Şube Onay İsteme
        public bool IsBranchApproval { get; set; } //Farklı Şube Onay Listeleme
    }
}