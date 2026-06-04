using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Models;
using TodoAPI.ViewModels;

namespace TodoAPI.Controllers;

public class HomeController : Controller
{
    private readonly TodoDb _db;

    public HomeController(TodoDb db)
    {
        _db = db;
    }

    // Mostra a lista de tarefas.
    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel
        {
            Todos = await _db.Todos
                .OrderBy(todo => todo.IsComplete)
                .ThenBy(todo => todo.Deadline)
                .ToListAsync()
        };

        return View(model);
    }

    // Cria uma nova tarefa.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Todo todo)
    {
        ModelState.Remove(nameof(Todo.Student));
        ModelState.Remove(nameof(Todo.StudentId));

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Preencha corretamente os campos da tarefa.";
            return RedirectToAction(nameof(Index));
        }

        todo.Name = todo.Name.Trim();
        todo.Email = todo.Email.Trim();
        todo.StudentId = null;

        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Tarefa criada.";
        return RedirectToAction(nameof(Index));
    }
}
