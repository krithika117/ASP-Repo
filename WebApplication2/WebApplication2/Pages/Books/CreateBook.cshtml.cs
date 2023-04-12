using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication2.Pages.Books
{
    public class CreateBookModel : PageModel
    {
        public List<Books> BookList = new List<Books>();
        public string errorMessage = "";

        Books b = new Books();
        public void OnGet() { }
        public void OnPost()
        {

            try
            {
                SqlConnection sqlConnection = new SqlConnection("Data Source=1GMYTP2;Initial Catalog=SQLProject;Integrated Security=True;Encrypt=False");
                sqlConnection.Open();

                b.book_code = Request.Form["book_code"];
                b.book_title = Request.Form["book_title"];
                b.category = Request.Form["category"];
                b.author = Request.Form["author"];
                b.publication = Request.Form["publication"];
                b.publish_date = Convert.ToDateTime(Request.Form["publish_date"]);
                b.book_edition = Convert.ToInt32(Request.Form["book_edition"]);
                b.price =Convert.ToInt32(Request.Form["price"]);
                b.date_arrival = Convert.ToDateTime(Request.Form["date_arrival"]);
                b.rack_num = Request.Form["rack_num"];
                b.supplier_id = Request.Form["supplier_id"];

                string query = $"INSERT INTO LMS_BOOK_DETAILS VALUES ('{b.book_code}', '{b.book_title}','{b.category}', '{b.author}', '{b.publication}','{b.publish_date}',{b.book_edition}, {b.price},'{b.rack_num}', '{b.date_arrival}', '{b.supplier_id}')";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                Console.WriteLine(query);

                command.ExecuteNonQuery();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorMessage += ex.Message;
            }

        }
    }

}
