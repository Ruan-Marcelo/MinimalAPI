namespace TodoAPI.Models
{
    public class Todo
    {
        // Chave primária da tabela.
        // O Entity Framework Core reconhecerá esta propriedade como o identificador do registro.
        public int Id { get; set; }
        // Nome ou descrição curta da tarefa.
        // É inicializada com string vazia para evitar valores nulos no uso da aplicação.
        public string Name { get; set; } = string.Empty;
        // Indica se a tarefa foi concluída.
        // false = pendente
        // true = concluída
        public bool IsComplete { get; set; }
    }
}
