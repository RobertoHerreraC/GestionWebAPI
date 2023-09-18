using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class RespuestaSolicitudRequest
    {
        [Required(ErrorMessage = "El campo SolicitudID es obligatorio.")]
        public int SolicitudID { get; set; }
        public string? Comentario { get; set; }

        [Required(ErrorMessage = "El campo FechaHoraAccion es obligatorio.")]
        public string FechaHoraAccion { get; set; }
        public int Respuesta { get; set; } = 0;
        public IFormFile? Documento { get; set; }


    }
}
