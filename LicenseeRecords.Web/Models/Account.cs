namespace LicenseeRecords.Web.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string LicenseNumber { get; set; } // Add this if missing
        public string AccountStatus { get; set; }
        public List<ProductLicence> ProductLicence { get; set; } = new List<ProductLicence>();
    }
}