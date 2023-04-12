using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication2.Pages.Books
{
    public class IndexBookModel : PageModel
    {
        public List<Books> BookList;

        public void OnGet()
        {

            try
            {
                BookList = new List<Books>();

                SqlConnection sqlConnection = new SqlConnection("Data Source=1GMYTP2;Initial Catalog=SQLProject;Integrated Security=True;Encrypt=False");
                sqlConnection.Open();
                string query = "SELECT BOOK_CODE, BOOK_TITLE, AUTHOR, PUBLICATION, PRICE FROM LMS_BOOK_DETAILS";
                SqlCommand command = new SqlCommand(query, sqlConnection);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Books book = new Books();
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
    public class Books
    {
        public string book_code { get; set; }

        public string book_title { get; set; }

        public string category { get; set; }
        public string author { get; set; }
        public string publication { get; set; }
        public DateTime publish_date { get; set; }
        public int book_edition { get; set; }
        public int price { get; set; }
        public string rack_num { get; set; }
        public DateTime date_arrival { get; set; }
        public string supplier_id { get; set; }
    }
}
