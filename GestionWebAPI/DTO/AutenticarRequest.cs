using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class AutenticarRequest
    {
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El campo Contrasena es obligatorio.")]
        public string Contrasena { get; set; }

    }
}
