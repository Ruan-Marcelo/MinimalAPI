using TodoAPI.Models;

namespace TodoAPI.ViewModels;

public class HomeIndexViewModel
{
    // Lista de tarefas mostrada na tabela.
    public List<Todo> Todos { get; set; } = [];

    // Tarefa que esta sendo editada.
    public Todo? EditingTodo { get; set; }
}
