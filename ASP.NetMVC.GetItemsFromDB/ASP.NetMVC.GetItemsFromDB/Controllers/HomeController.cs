using ASP.NetMVC.GetItemsFromDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

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

            string queryComments = $"SELECT v.Video, c.Comment " +
                $"FROM [Youtube].[dbo].[Comments] c " +
                $"inner join [Youtube].[dbo].[videos] v " +
                $"on v.Id = c.Id WHERE Comment LIKE '%{search}%'";

            using SqlConnection con = new("Data Source=.;Initial Catalog=Youtube; Integrated Security=True;TrustServerCertificate=true");

            using SqlCommand cmd = new SqlCommand(queryComments, con);
            con.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comments.Add(new Comment() { Text = reader.GetString(0) + @"\r\n" + reader.GetString(1) });              
            }
            con.Close();

            ViewBag.Comments = comments.Count;

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
