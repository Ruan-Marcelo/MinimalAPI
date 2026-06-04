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

    // Mostra a lista de tarefas e carrega uma tarefa para editar.
    public async Task<IActionResult> Index(int? editId)
    {
        var model = new HomeIndexViewModel
        {
            Todos = await _db.Todos
                .OrderBy(todo => todo.IsComplete)
                .ThenBy(todo => todo.Deadline)
                .ToListAsync()
        };

        if (editId.HasValue)
        {
            model.EditingTodo = await _db.Todos.FindAsync(editId.Value);

            if (model.EditingTodo is null)
            {
                return NotFound();
            }
        }

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

    // Edita uma tarefa que ja existe.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Todo input)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Todo.Student));
        ModelState.Remove(nameof(Todo.StudentId));

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Preencha corretamente os campos da tarefa.";
            return RedirectToAction(nameof(Index), new { editId = id });
        }

        todo.Name = input.Name.Trim();
        todo.Email = input.Email.Trim();
        todo.Datetime = input.Datetime;
        todo.Deadline = input.Deadline;
        todo.IsComplete = input.IsComplete;

        await _db.SaveChangesAsync();

        TempData["Message"] = "Tarefa atualizada.";
        return RedirectToAction(nameof(Index));
    }

    // Exclui uma tarefa.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Tarefa excluida.";
        return RedirectToAction(nameof(Index));
    }
}
