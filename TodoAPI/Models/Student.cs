using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TodoAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="O nome do aluno é obrigatório.")]
        public String Name { get; set; } = string.Empty;

        [Required(ErrorMessage ="O email do aluno é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public String Email { get; set; } = string.Empty;
        public List<Todo> Todos { get; set; } = new();



    }
}
