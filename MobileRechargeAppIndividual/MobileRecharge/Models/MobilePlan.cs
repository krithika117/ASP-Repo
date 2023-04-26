namespace MobileRecharge.Models
{
    public class MobilePlan
    {
        public int Id { get; set; }
        public String ServiceProvider { get; set; }
        public String  PlanName { get; set; }
        public String Mode { get; set; }
        public String DataQuant { get; set; }
        public String ServiceProviderUPI { get; set; }
        public string NoOfMonths { get; set; }
        public int Amount { get; set; }


    }
}
