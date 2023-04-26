using System.ComponentModel.DataAnnotations.Schema;

namespace MobileRecharge.Models
{
    public class Recharge
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public MobilePlan Plan { get; set; }
    }
}
