namespace LicenseeRecords.WebAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public List<int> LicensedProducts { get; set; } = new List<int>();
    }
}