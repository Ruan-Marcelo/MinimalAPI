using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class Todo
    {
        // Chave primária da tabela.
        // O Entity Framework Core reconhecerá esta propriedade como o identificador do registro.
        public int Id { get; set; }
        // Nome ou descrição curta da tarefa.
        // É inicializada com string vazia para evitar valores nulos no uso da aplicação.

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        public string Name { get; set; } = string.Empty;
        // Indica se a tarefa foi concluída.
        // false = pendente
        // true = concluída

        [Required(ErrorMessage = "A Data da tarefa é obrigatória.")]
        public DateTime Datetime{ get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public string Email{ get; set; } = string.Empty;

        public bool IsComplete { get; set; }

        [Required(ErrorMessage = "O prazo da tarefa é obrigatório.")]
        public DateTime Deadline { get; set; }

        public int StudentId { get; set; }

        public Students? Student { get; set; }
    }
}
