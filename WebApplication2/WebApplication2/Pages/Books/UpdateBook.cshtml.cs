using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication2.Pages.Books
{
    public class UpdateBookModel : PageModel
    {
        public Books book = new Books();
        public List<Books> BookList = new List<Books>();

        public void OnGet()
        {

            try
            {
                string book_code = Request.Query["book_code_input"];
                SqlConnection sqlConnection = new SqlConnection("Data Source=1GMYTP2;Initial Catalog=SQLProject;Integrated Security=True;Encrypt=False");
                sqlConnection.Open();
                string query = $"SELECT * FROM LMS_BOOK_DETAILS WHERE BOOK_CODE = {book_code}";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    book.book_code = reader.GetString(0);
                    book.book_title = reader.GetString(1);
                    book.author = reader.GetString(2);
                    book.publication = (string)reader["publication"];
                    book.price = (int)reader["price"];

                    BookList.Add(book);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }
}
