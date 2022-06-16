using Dms.Integration.Infrastructure.Attributes;

namespace Dms.Integration.Infrastructure.Enums;

public enum DocumentDefinitionType
{
    /// <summary>
    /// Bilinmiyor
    /// </summary>
    [Name("Bilinmiyor")]
    None = 0,

    /// <summary>
    /// Pazarlama izni
    /// </summary>
    [Name("İzinli Pazarlama Talimatı")]
    MarkettingPermissionContract = 10,

    /// <summary>
    /// Kisisel veri isleme (KVK)
    /// </summary>
    [Name("KVK")]
    PersonalInfoUsageContract = 20,

    //Text değişmemeli => Bankacılık Hizmetleri Sözleşmesi 
    /// <summary>
    /// Bankacilik hizmet sozlesmesi
    /// </summary>
    [Name("Bankacılık Hizmetleri Sözleşmesi")]
    BHSContract = 30,

    /// <summary>
    /// Kimlik, nufus cuzdani
    /// </summary>
    [Name("Nüfus Cüzdanı")]
    IdentificationCard = 40,

    /// <summary>
    /// Diğer Elementer Kasko Poliçesi => 1572
    /// </summary>
    [Name("Diğer Elementer Kasko Poliçesi")]
    OtherInsurancePolicy = 1200,

    /// <summary>
    /// Esya-Konut-DASK-Isyeri => 1565
    /// </summary>
    [Name("Esya Konut DASK Isyeri Poliçesi")]
    DaskInsurancePolicy = 1210,

    /// <summary>
    ///  Kasko-Trafik => 1568
    /// </summary>
    [Name("Araç Kasko Trafik Poliçesi")]
    VehicleInsurancePolicy = 1220,
}