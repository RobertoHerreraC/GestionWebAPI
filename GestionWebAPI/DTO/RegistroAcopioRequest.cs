using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class RegistroAcopioRequest
    {
        [Required(ErrorMessage = "El campo SolicitudID es obligatorio.")]
        public int SolicitudID { get; set; }
        public int DerivadoID { get; set; }
        // public int ResponsableID { get; set; }
        public string? Comentario { get; set; }

        [Required(ErrorMessage = "El campo FechaHoraAccion es obligatorio.")]
        public string FechaHoraAccion { get; set; }

        public IFormFile? Documento { get; set; }
    }
}
