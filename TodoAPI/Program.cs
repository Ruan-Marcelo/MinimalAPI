using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura a conexao com o banco de dados.
builder.Services.AddDbContext<TodoDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Adiciona suporte para controllers e views.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Permite usar arquivos estaticos, como CSS e JavaScript.
app.UseStaticFiles();

// Define a pagina inicial da aplicacao.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia a aplicacao.
app.Run();
