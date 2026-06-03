using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TodoAPI.Models
{
    public class Students
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="O nome do aluno é obrigatório.")]
        public String Name { get; set; }

        [Required(ErrorMessage ="O email do aluno é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        public String Email { get; set; }
        public List<Todo> Todos { get; set; } = new();



    }
}
