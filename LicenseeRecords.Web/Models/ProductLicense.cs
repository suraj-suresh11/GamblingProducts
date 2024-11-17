namespace LicenseeRecords.Web.Models
{
    public class ProductLicence
    {
        public int LicenceId { get; set; }
        public string LicenceStatus { get; set; }
        public DateTime LicenceFromDate { get; set; }
        public DateTime? LicenceToDate { get; set; }
        public Product Product { get; set; }
    }
}