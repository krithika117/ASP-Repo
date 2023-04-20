using Microsoft.Data.SqlClient;

namespace JobPortal
{
    public class Helper
    {
        public static Dictionary<string, List<string>> GetCategories()
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
    }
}
