using RestabookWebApp.Models.Common;

namespace RestabookWebApp.Models
{
    public class Reservation : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Persons { get; set; }
        public DateTime ResDate { get; set; }
        public string ResTime { get; set; }
        public string? Message { get; set; }
    }

}