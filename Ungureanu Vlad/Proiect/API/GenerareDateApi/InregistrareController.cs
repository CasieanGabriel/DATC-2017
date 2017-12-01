using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GenerareDateApi.Models
{
    [Route("api/[controller]")]
    public class InregistrareController : Controller
    {
        private readonly InregistrareContext _context;

        public InregistrareController(InregistrareContext context)
        {
            _context = context;

            if (_context.InregistrariItems.Count() == 0)
            {
       //         _context.InregistrariItems.Add(new TodoItem { Name = "Item1" });
         //       _context.SaveChanges();
            }
        }
        [HttpGet]
        public IEnumerable<Inregistrare> GetAll()
        {
            return _context.InregistrariItems.ToList();
        }

        [HttpGet("{id}", Name = "GetInregistrare")]
        public IActionResult GetById(long id)
        {
            var item = _context.InregistrariItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Inregistrare item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.InregistrariItems.Add(item);
            _context.SaveChanges();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "silviu.database.windows.net";
            builder.UserID = "silviumilu";
            builder.Password = "!Silviu1";
            builder.InitialCatalog = "proiect";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();

                var queryString = $"insert into TabelaInregistrari (id,idsenzor,temperatura,umiditate,presiune,data) values ({item.Id}, {item.idsenzor}, {item.temperatura}, {item.umiditate},{item.presiune},'{item.data}');";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.ExecuteNonQuery();
                }
            }



            return CreatedAtRoute("GetInregistrare", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Inregistrare item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.InregistrariItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

     //       todo.idsenzor = item.idsenzor;
       //     todo.temperatura = item.temperatura;
    //        todo.umiditate = item.umiditate;
      //      todo.presiune = item.presiune;
       //     todo.data = item.data;

            _context.InregistrariItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var ing = _context.InregistrariItems.FirstOrDefault(t => t.Id == id);
            if (ing == null)
            {
                return NotFound();
            }

            _context.InregistrariItems.Remove(ing);
            _context.SaveChanges();
            return new NoContentResult();
        }

    }
}
