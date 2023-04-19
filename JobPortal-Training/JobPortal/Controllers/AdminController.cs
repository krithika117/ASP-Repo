using System.Text;
using Azure.Messaging;
using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace JobPortal.Controllers
{
	public class AdminController : Controller
	{
		public List<JobModel> jobs = new();
		public IConfiguration configuration;
		public AdminController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		public IActionResult AddJob()
		{
			ViewData["categories"] = GetCategories();
			return View();
		}

		public IActionResult Jobs()
		{
			StringBuilder whereClause = new();

			string category = Request.Query["filterCategory"];
			string subcategory = Request.Query["filterSubcategory"];
			string salary = Request.Query["filterSalary"];
			if (category != null && category.Length > 0)
			{
				ViewData["filterCategory"] = category;
				whereClause.Append($"category='{category}'");
			}
			if (subcategory != null && subcategory.Length > 0)
			{
				ViewData["filterSubcategory"] = subcategory;
				if (whereClause.Length > 0)
				{
					whereClause.Append(" and ");
				}
				whereClause.Append($"subcategory='{subcategory}'");
			}
			if (salary != null && salary.Length > 0)
			{
				ViewData["filterSalary"] = salary;
				if (whereClause.Length > 0)
				{
					whereClause.Append(" and ");
				}
				whereClause.Append($"salary>={salary}");
			}
			if (whereClause.Length > 0)
			{
				whereClause.Insert(0, "where ");
			}
			GetJobs(whereClause.ToString());
			return View();
		}
		private Dictionary<string, List<string>> GetCategories()
		{
			SqlCommand command = Connection.CreateCommand("select c.name as category, sc.name as subcategory from category c join subcategory sc on c.id = sc.categoryid");
			Dictionary<string, List<string>> categories = new();
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					string category = (string)reader["category"];
					string subCategory = (string)reader["subcategory"];
					if (categories.ContainsKey(category))
					{
						categories[category].Add(subCategory);
					}
					else
					{
						categories.Add(category, new() { subCategory });
					}
				}
			}
			return categories;
		}
		public IActionResult Edit()
		{
			try
			{
				SqlCommand command = Connection.CreateCommand();
				int id = Convert.ToInt32(Request.Form["id"]);
				string name = Request.Form["name"];
				string category = Request.Form["category"];
				string subCategory = Request.Form["subCategory"];
				string CompanyName = Request.Form["companyName"];
				float salary = float.Parse(Request.Form["salary"]);
				string deadline = Request.Form["deadline"];
				command.CommandText = $"update jobs set name='{name}', category='{category}', subcategory='{subCategory}', salary={salary}, deadline='{deadline}', companyName= '{CompanyName}' where id={id}";
				command.ExecuteNonQuery();
				TempData["message"] = "Job updated successfully";
				TempData["messageType"] = "alert-success";
			}
			catch (Exception ex)
			{
				TempData["message"] = ex.Message;
				TempData["messageType"] = "alert-danger";
			}
			return Redirect("Jobs");
		}
		public IActionResult Delete()
		{
			try
			{
				int id = Convert.ToInt32(Request.Query["id"]);
				SqlCommand command = Connection.CreateCommand();
				command.CommandText = $"delete from jobs where id={id}";
				command.ExecuteNonQuery();
				TempData["message"] = "Job deleted successfully";
				TempData["messageType"] = "alert-success";
			}
			catch
			{
				TempData["message"] = "Unable to delete job";
				TempData["messageType"] = "alert-danger";
			}
			return Redirect("Jobs");
		}
		[HttpPost]
		public IActionResult AddJob(IFormCollection form)
		{
			try
			{
				string name = Request.Form["name"];
				string category = Request.Form["category"];
				string subCategory = Request.Form["subCategory"];
				float salary = float.Parse(Request.Form["salary"]);
				string deadline = Request.Form["deadline"];
				string CompanyName = Request.Form["CompanyName"];
				SqlCommand command = Connection.CreateCommand();
				command.CommandText = $"insert into jobs values('{name}','{category}','{subCategory}',{salary},'{deadline}','{CompanyName}')";
				command.ExecuteNonQuery();
				TempData["message"] = "Job added successfully";
				TempData["messageType"] = "alert-success";
			}
			catch (Exception e)
			{
				TempData["message"] = e.Message;
				TempData["messageType"] = "alert-danger";
			}
			return View();
		}
		private void GetJobs(string where = "")
		{
			SqlCommand command = Connection.CreateCommand();
			command.CommandText = "select * from jobs " + where;
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					JobModel job = new();
					job.Id = reader.GetInt32(0);
					job.Name = reader.GetString(1);
					job.Category = reader.GetString(2);
					job.SubCategory = reader.GetString(3);
					job.Salary = (float)reader.GetDouble(4);
					job.DeadLine = reader.GetDateTime(5);
					job.CompanyName = reader.GetString(6);
					jobs.Add(job);
				}
			}
			ViewData["jobs"] = jobs;
			if (Request.Query["editid"].ToString() != null)
			{
				ViewData["editid"] = Convert.ToInt32(Request.Query["editid"]);
				ViewData["categories"] = GetCategories();
			}
			else
			{
				ViewData["editid"] = null;
			}
		}
	}
}
