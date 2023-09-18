using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class RegistroBitacoraRequest
    {
        [Required(ErrorMessage = "El campo SolicitudID es obligatorio.")]
        public int SolicitudID { get; set; }
        public string? Comentario { get; set; }

        [Required(ErrorMessage = "El campo FechaHoraAccion es obligatorio.")]
        public string FechaHoraAccion { get; set; }
        [Required(ErrorMessage = "El campo Documento es obligatorio.")]
        public IFormFile? Documento { get; set; }
    }
}
