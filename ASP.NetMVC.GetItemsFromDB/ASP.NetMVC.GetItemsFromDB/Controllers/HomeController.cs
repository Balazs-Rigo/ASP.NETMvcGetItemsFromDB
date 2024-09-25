using ASP.NetMVC.GetItemsFromDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace ASP.NetMVC.GetItemsFromDB.Controllers
{
    public class HomeController : Controller
    {        
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
                search = "varicose";

            var comment = new Comment();  
            var comments = new List<Comment>();

            string queryComments = $"SELECT [Comment] FROM [Youtube].[dbo].[Comments] WHERE Comment LIKE '%{search}%'";

            using SqlConnection con = new("Data Source=.;Initial Catalog=Youtube; Integrated Security=True;Trust Server Certificate=True");

            using SqlCommand cmd = new SqlCommand(queryComments, con);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comments.Add(new Comment() { Text = reader.GetString(0) });              
            }
            con.Close();          

            return View(comments);
        }  

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
