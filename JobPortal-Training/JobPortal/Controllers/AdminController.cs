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
			return View();
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
			return Redirect("Admin");
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
				TempData["messageType"] = "alert-danged";
			}
			return Redirect("Admin");
		}
		public IActionResult AddJob2()
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
			return Redirect("AddJob");
		}
		public IActionResult Admin()
		{
			SqlCommand command = Connection.CreateCommand();
			command.CommandText = "select * from jobs";
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
			ViewBag.jobs = jobs;
			if (Request.Query["editid"].ToString() != null)
			{
				ViewBag.editID = Convert.ToInt32(Request.Query["editid"]);
			}
			else
			{
				ViewBag.editID = null;
			}
			return View();
		}
	}
}
