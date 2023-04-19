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
            List<JobModel> jobs = new List<JobModel>();
            try
            {
                SqlConnection conn = new SqlConnection(configuration.GetConnectionString("jobDB"));
                conn.Open();
                SqlCommand command = conn.CreateCommand();
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

        public ActionResult Search(string search)
        {
            try
            {
                List<JobModel> jobs = new List<JobModel>();
                SqlConnection conn = new SqlConnection(configuration.GetConnectionString("jobDB"));
                conn.Open();
                SqlCommand command = conn.CreateCommand();

                if (string.IsNullOrEmpty(search))
                {
                    command.CommandText = $"SELECT * FROM jobs";
                }
                else
                {
                    command.CommandText = $"SELECT * FROM jobs WHERE name LIKE '%{search}%' OR companyName LIKE '%{search}%' OR Category LIKE '%{search}%'";
                }

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
