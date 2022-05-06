namespace Application.BbtInternals.Queries.GetSearchPersonSummary
{
    public class GetSearchPersonSummaryDto
    {
        public long CitizenshipNumber { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public long ClientNumber { get;  set; }
        public string Token { get; set; }

        public bool IsCustomer { get; set; }
        public AuthoryModel Authory { get; set; }

        public class AuthoryModel
        {
            public bool IsReadyFormCreator { get; set; } // Form ile Emir Oluşturma
            public bool IsNewFormCreator { get; set; } //Yeni Onay Emri Oluşturma
            public bool IsFormReader { get; set; } // Tüm Onay Emirlerini İzleyebilir
            public bool IsBranchFormReader { get; set; } //Farklı Şube Onay İsteme
            public bool IsBranchApproval { get; set; } //Farklı Şube Onay Listeleme
        }
    }
}
