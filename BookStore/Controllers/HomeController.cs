using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BookStore.DataSource;
using Npgsql;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        DataSource.DataSource ds;
        public HomeController()
        {
            ds = new DataSource.DataSource();
        }
        public IActionResult Index()
        {
            NpgsqlCommand cmd = ds.source.CreateCommand("Select * from GetAllBooks()");
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            List<Book> books = new List<Book>();
            while(rdr.Read())
            {
                Book b = new Book();
                b.Id = (int)rdr["id"];
                b.Name= (string)rdr["name"];
                b.Publish_Date = (string)rdr["publish_date"];
                b.Author = (string)rdr["author"];
                b.Price = Convert.ToSingle(rdr["price"]);
                books.Add(b);
            }
            TempData["ISADMIN"] = User.IsInRole("Users");
            return View(books);
        }
        [Authorize(Roles ="Users")]
        public ActionResult Create() {
            return View();
        }
        [HttpPost]
        [Authorize(Roles ="Users")]
        public ActionResult Create(Book b) 
        {
            if(ModelState.IsValid)
            {
                NpgsqlCommand cmd = ds.source.CreateCommand($"Call CreateBook('{b.Name}','{b.Author}','{b.Publish_Date}','{b.Price}')");
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            return View(b);
        }
       
        [Authorize(Roles ="Users")]
        public ActionResult Delete(int id) 
        {
            NpgsqlCommand cmd = ds.source.CreateCommand($"Call DeleteBook({id})");
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }
    }
}