using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MiMatos.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}
