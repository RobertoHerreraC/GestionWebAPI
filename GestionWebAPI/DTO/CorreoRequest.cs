using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class CorreoRequest
    {
        [Required(ErrorMessage = "El campo Destinatario es obligatorio.")]
        public string Destinatario { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Asunto es obligatorio.")]
        public string Asunto { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Contenido es obligatorio.")]
        public string Contenido { get; set; } = string.Empty;

        public List<string>? Adjuntos { get; set; }
    }
}
