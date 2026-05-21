using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;

namespace TodoAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly TodoDb _db;

        public HomeController(TodoDb db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var todos = await _db.Todos.ToListAsync();

            return View(todos);
        }
    }
}