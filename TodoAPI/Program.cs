using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Dtos;
using TodoAPI.Models;
// Cria o builder da aplicação ASP.NET.
// Ele é responsável por configurar serviços, dependências e opções da aplicação.
var builder = WebApplication.CreateBuilder(args);

// Registra o contexto do Entity Framework no container de injeção de dependência.
// Aqui estamos dizendo que o TodoDb usará SQL Server e a connection string "DefaultConnection" definida no appsettings.json.
builder.Services.AddDbContext<TodoDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona um filtro de exceções voltado ao desenvolvimento.
// Ele ajuda a exibir erros relacionados ao banco de dados de forma mais detalhada, para o desenvolvedor.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//criar views
builder.Services.AddControllersWithViews();

// Constrói a aplicação com base nas configurações definidas acima.
var app = builder.Build();

// Cria um grupo de rotas com o prefixo "/todoitems".
// Isso evita repetir esse trecho em todos os endpoints.
var todoItems = app.MapGroup("/todoitems");

// =========================
// GET /todoitems
// GET /todoitems?isComplete=true

// =========================
// Retorna todos os itens.
// Se o parâmetro opcional "isComplete" for enviado, filtra pelos itens completos ou incompletos.

// =========================
// Método GET com query parameter nomeado, sem usar Lambda, se for usar esse, tem que comentar o que está usando Lambda
// =========================
// GET /todoitems
// GET /todoitems?isComplete=true
// =========================
// Retorna todos os itens.
// Se o parâmetro opcional "isComplete" for enviado, filtra pelos itens completos ou incompletos.

todoItems.MapGet("/", GetTodoItems);

static async Task<IResult> GetTodoItems(bool? isComplete, TodoDb db)
{
    var query = db.Todos.AsQueryable();

    if (isComplete.HasValue)
    {
        query = query.Where(t => t.IsComplete == isComplete.Value);
    }

    var items = await query
        .OrderBy(t => t.Id)
        .Select(t => new TodoItemDto(t))
        .ToListAsync();

    return Results.Ok(items);
}

// =========================
// GET /todoitems/{id}
// =========================
// Retorna um item específico pelo Id.
todoItems.MapGet("/{id:int}", async (int id, TodoDb db) =>
{
    // Procura o item pelo Id informado.
    var todo = await db.Todos.FindAsync(id);

    // Se não encontrou, retorna HTTP 404 Not Found.
    if (todo is null)
        return Results.NotFound();

    // Se encontrou, retorna HTTP 200 OK com o item convertido para DTO.
    return Results.Ok(new TodoItemDto(todo));
});

// =========================
// POST /todoitems
// =========================
// Cria um novo item no banco.
todoItems.MapPost("/", async (TodoItemDto input, TodoDb db) =>
{
    // Validação simples: o Name é obrigatório.
    if (string.IsNullOrWhiteSpace(input.Name))
    {
        // Retorna HTTP 400 Bad Request com detalhes de validação.
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["name"] = new[] { "O nome é obrigatorio." }
        });
    }
    if(string.IsNullOrWhiteSpace(input.Email))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["email"] = new[] { "O email é obrigatorio." }
        });
    }
    if(input.Datetime == DateTime.MinValue)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["datetime"] = new[] { "A data e hora são obrigatórias." }
        });
    }
    if (input.Deadline == DateTime.MinValue)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["deadline"] = new[] { "O prazo é obrigatório." }
        });
    }

    if (input.StudentId <= 0)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["studentId"] = new[] { "O aluno é obrigatório." }
        });
    }

    var studentExists = await db.Students.AnyAsync(s => s.Id == input.StudentId);

    if (!studentExists)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["studentId"] = new[] { "Aluno não encontrado." }
        });
    }

    // Cria a entidade Todo a partir do DTO recebido.
    var todo = new Todo
    {
        Name = input.Name.Trim(),
        IsComplete = input.IsComplete,
        Email = input.Email.Trim(),
        Datetime = input.Datetime
    };

    // Adiciona o novo item ao contexto.
    db.Todos.Add(todo);

    // Persiste as alterações no banco.
    await db.SaveChangesAsync();

    // Retorna HTTP 201 Created com a URL do recurso recém criado e o objeto criado no corpo da resposta.
    return Results.Created($"/todoitems/{todo.Id}", new TodoItemDto(todo));
});

// =========================
// PUT /todoitems/{id}
// =========================
// Atualiza completamente um item existente.
todoItems.MapPut("/{id:int}", async (int id, TodoItemDto input, TodoDb db) =>
{
    // Validação simples: o Name é obrigatório.
    if (string.IsNullOrWhiteSpace(input.Name))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["name"] = new[] { "O nome é obrigatorio." }
        });
    }

    // Busca o item existente pelo Id.
    var todo = await db.Todos.FindAsync(id);

    // Se não existir, retorna 404.
    if (todo is null)
        return Results.NotFound();

    // Atualiza todos os campos relevantes do recurso.
    todo.Name = input.Name.Trim();
    todo.IsComplete = input.IsComplete;
    todo.Datetime = input.Datetime;
    todo.Email = input.Email.Trim();

    // Salva no banco.
    await db.SaveChangesAsync();

    // Retorna HTTP 204 No Content, indicando sucesso sem corpo na resposta.
    return Results.NoContent();
});

// =========================
// PATCH /todoitems/{id}
// =========================
// Atualiza parcialmente um item existente.
todoItems.MapPatch("/{id:int}", async (int id, TodoPatchDto input, TodoDb db) =>
{
    // Busca o item existente.
    var todo = await db.Todos.FindAsync(id);

    // Se não existir, retorna 404.
    if (todo is null)
        return Results.NotFound();

    // Se o campo Name veio na requisição, tenta atualizá-lo.
    if (input.Name is not null)
    {
        // Se veio vazio ou só com espaços, retorna erro de validação.
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["name"] = new[] { "O nome não pode ser vazio." }
            });
        }

        // Atualiza o nome com remoção de espaços extras nas extremidades.
        todo.Name = input.Name.Trim();
    }

    // Se o campo IsComplete veio na requisição, atualiza esse valor.
    if (input.IsComplete.HasValue)
    {
        todo.IsComplete = input.IsComplete.Value;
    }

    if(input.Email is not null)
    {
        if (string.IsNullOrWhiteSpace(input.Email))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["Email"] = new[] {"O Email não pode ser vazio"}
            });
        }
        todo.Email = input.Email.Trim();
    }

    if (input.Datetime.HasValue)
    {
        if (input.Datetime.Value == DateTime.MinValue)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["datetime"] = new[] { "A data e hora não podem ser vazias." }
            });
        }

        todo.Datetime = input.Datetime.Value;
    }

    // Salva as alterações no banco.
    await db.SaveChangesAsync();

    // Retorna HTTP 204 No Content.
    return Results.NoContent();
});

// =========================
// DELETE /todoitems/{id}
// =========================
// Remove um item existente.
todoItems.MapDelete("/{id:int}", async (int id, TodoDb db) =>
{
    // Busca o item pelo Id.
    var todo = await db.Todos.FindAsync(id);

    // Se não existir, retorna 404.
    if (todo is null)
        return Results.NotFound();

    // Remove o item do contexto.
    db.Todos.Remove(todo);

    // Persiste a remoção no banco.
    await db.SaveChangesAsync();

    // Retorna HTTP 204 No Content.
    return Results.NoContent();
});

// Inicia a aplicação e começa a escutar requisições HTTP.
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
