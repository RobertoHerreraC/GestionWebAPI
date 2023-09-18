using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class DerivadoSolicitudRequest
    {
        [Required(ErrorMessage = "El campo SolicitudID es obligatorio.")]
        public int SolicitudID { get; set; }
        [Required(ErrorMessage = "El campo ResponsableID es obligatorio.")]
        public int ResponsableID { get; set; }
        [Required(ErrorMessage = "El campo Comentario es obligatorio.")]
        public string Comentario { get; set; }

        public IFormFile? Documento { get; set; }
    }
}
