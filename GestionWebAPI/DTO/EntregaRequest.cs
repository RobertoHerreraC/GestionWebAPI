using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class EntregaRequest
    {
        [Required(ErrorMessage = "El campo SolicitudID es obligatorio.")]
        public int SolicitudID { get; set; }
        public string? Comentario { get; set; }

        [Required(ErrorMessage = "El campo FechaHoraAccion es obligatorio.")]
        public string FechaHoraAccion { get; set; }
    }
}
