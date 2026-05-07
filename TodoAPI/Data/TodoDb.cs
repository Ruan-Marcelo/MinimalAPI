using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
namespace TodoAPI.Data;
// Classe de contexto do Entity Framework Core.
// Ela faz a ponte entre a aplicação e o banco de dados.
// É por meio dela que consultamos, inserimos, atualizamos e removemos dados.
public class TodoDb : DbContext
{
    // Construtor que recebe as opções de configuração do contexto,
    // como a connection string e o provedor do banco de dados (SQL Server).
    public TodoDb(DbContextOptions<Todo> options) : base(options)
{
}
// Representa a tabela &quot;Todos&quot; no banco de dados.
// O tipo DbSet&lt;Todo&gt; permite executar operações de consulta e persistência sobre a entidade Todo.
 public DbSet<Todo> Todos => Set<Todo>();

}
