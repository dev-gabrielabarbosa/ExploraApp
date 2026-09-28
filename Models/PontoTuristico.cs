using System;
using System.ComponentModel.DataAnnotations;

namespace PontoTuristicoApp.Models
{
    public class PontoTuristico
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(100, ErrorMessage = "A descrição não pode ter mais que 100 caracteres.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A localização é obrigatória.")]
        [Display(Name = "Localização/Endereço")]
        public string Localizacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;

        [Display(Name = "Data de Inclusão")]
        public DateTime DataInclusao { get; set; } = DateTime.Now;
    }
}
