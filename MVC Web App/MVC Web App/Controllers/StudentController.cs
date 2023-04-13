using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVC_Web_App.Models;

namespace MVC_Web_App.Controllers
{

    public class StudentController : Controller
    {IConfiguration configuration;
        public StudentController(IConfiguration configuration) { 
            this.configuration = configuration;
        }
        public IActionResult Student()
        {
            return View();
        }

        public List<StudentModel> students = new List<StudentModel>();
        public string info = "Hello student info";
        public IActionResult Index()
        {
            
                var connectionString = configuration.GetConnectionString("TestDB");
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connectionString;

                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM dbo.employees";

                SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StudentModel s1 = new();
                        s1.employee_id = reader.GetInt32(0);
                        s1.employee_name = reader.GetString(1);
                    try
                    {

                        s1.department_id = (reader["department_id"] == null) ? (1) : (reader.GetInt32(2));
                    }
                    catch (Exception ex) 
                    { 
                        Console.WriteLine(ex); 
                    }

                    students.Add(s1);
                    } 


            ViewBag.info = info;

            
            ViewBag.students = students;

            return View();

        }
    }
}