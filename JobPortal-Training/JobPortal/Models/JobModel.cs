namespace JobPortal.Models
{
	public class JobModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public string SubCategory { get; set; }
		public float Salary { get; set; }
		public DateTime DeadLine { get; set; }
	}
}
