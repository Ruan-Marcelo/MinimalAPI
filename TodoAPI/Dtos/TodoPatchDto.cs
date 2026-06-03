namespace TodoAPI.Dtos;
// DTO específico para operações PATCH.

// Como PATCH representa atualização parcial, as propriedades são anuláveis.
// Assim, a API consegue distinguir entre:
// - campo não enviado na requisição
// - campo enviado com valor definido
public class TodoPatchDto
{
    // Nome da tarefa.
    // Se vier nulo, significa que o cliente não quer alterar esse campo.
    public string? Name { get; set; }
    // Estado de conclusão da tarefa.
    // O tipo bool? permite diferenciar:
    // - null -&gt; não alterar o campo
    // - true -&gt; marcar como concluída
    // - false -&gt; marcar como pendente
    public bool? IsComplete { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Email { get; set; }

    public DateTime? Deadline { get; set; }
    public int? StudentId { get; set; }
}
