using System.ComponentModel.DataAnnotations;
using TodoAPI.Models;
namespace TodoAPI.Dtos;
// DTO (Data Transfer Object) usado para enviar e receber dados da API.
// Neste caso, ele representa apenas os campos que desejamos expor ao cliente.
public class TodoItemDto
{
    // Identificador da tarefa.
    public int Id { get; set; }
    // Nome da tarefa.
    public string Name { get; set; } = string.Empty;
    // Indica se a tarefa está concluída.
    public bool IsComplete { get; set; }
    public DateTime Datetime { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    // Construtor vazio.
    // É útil para desserialização automática do JSON recebido pela API.
    public TodoItemDto()
    {
    }
    // Construtor de conveniência para converter uma entidade Todo em DTO.
    // Facilita o retorno dos dados ao cliente sem expor diretamente a entidade do banco.
    public TodoItemDto(Todo todo)
    {
        Id = todo.Id;
        Name = todo.Name;
        IsComplete = todo.IsComplete;
        Datetime = todo.Datetime;
        Email = todo.Email;
    }
}
