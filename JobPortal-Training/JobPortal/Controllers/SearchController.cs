using System.Text;
using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace JobPortal.Controllers
{
	public class SearchController : Controller
	{
		public List<JobModel> searchJobs = new();
		public IConfiguration configuration;
		public SearchController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
        public IActionResult SearchIndex()
        {
			ViewData["categories"] = Helper.GetCategories();
            List<JobModel> jobs = new List<JobModel>();
            try
            {
                SqlCommand command = Connection.CreateCommand();
                command.CommandText = $"SELECT * FROM jobs";
                Console.WriteLine(command.CommandText);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        JobModel job = new JobModel();
                        job.Id = (int)reader["id"];
                        job.Name = (string)reader["name"];
                        job.Category = (string)reader["category"];
                        job.SubCategory = (string)reader["subcategory"];
                        job.Salary = (float)reader.GetDouble(4);
                        job.DeadLine = (DateTime)reader["deadline"];
                        searchJobs.Add(job);
                        Console.WriteLine(job.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            ViewBag.jobs = searchJobs;
            return View();
        }

        public ActionResult Search(string search, string searchCategory, string searchSubcategory, string salary)
        {
			ViewData["categories"] = Helper.GetCategories();
            try
            {
                List<JobModel> jobs = new List<JobModel>();
                SqlCommand command = Connection.CreateCommand();

                StringBuilder commandText = new("SELECT * FROM jobs");
                StringBuilder whereClause = new();
                if(search != null && search.Length > 0)
                {
					whereClause.Append($"name LIKE '%{search}%' OR companyName LIKE '%{search}%' OR Category LIKE '%{search}%'");
				}
				if (searchCategory != null && searchCategory.Length > 0)
				{
					if (whereClause.Length > 0)
					{
						whereClause.Append(" and ");
					}
					whereClause.Append($"category='{searchCategory}'");
				}
				if (searchSubcategory != null && searchSubcategory.Length > 0)
				{
					if (whereClause.Length > 0)
					{
						whereClause.Append(" and ");
					}
					whereClause.Append($"subcategory='{searchSubcategory}'");
				}
				if (salary != null && salary.Length > 0)
				{
					if (whereClause.Length > 0)
					{
						whereClause.Append(" and ");
					}
					whereClause.Append($"salary>={salary}");
				}
				if (whereClause.Length > 0)
				{
					whereClause.Insert(0, " where ");
				}
                commandText.Append(whereClause);
                command.CommandText = commandText.ToString();
                Console.WriteLine(command.CommandText);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        JobModel job = new JobModel();
                        job.Id = (int)reader["id"];
                        job.Name = (string)reader["name"];
                        job.Category = (string)reader["category"];
                        job.SubCategory = (string)reader["subcategory"];
                        job.Salary = (float)reader.GetDouble(4);
                        job.DeadLine = (DateTime)reader["deadline"];
                        jobs.Add(job);
                        Console.WriteLine(job.Name);
                    }
                }
                ViewBag.jobs = jobs;
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return PartialView("_SearchResults", ViewBag.jobs);
        }
    }
}
