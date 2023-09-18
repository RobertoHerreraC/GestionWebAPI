using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class PaginadoRequest
    {
        [Required(ErrorMessage = "El campo NumeroPagina es obligatorio.")]
        public int NumeroPagina { get; set; }
        [Required(ErrorMessage = "El campo TamanoPagina es obligatorio.")]
        public int TamanoPagina { get; set; }
    }
}
