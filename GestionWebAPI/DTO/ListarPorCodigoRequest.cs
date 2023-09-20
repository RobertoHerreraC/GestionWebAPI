using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class ListarPorCodigoRequest
    {
        [Required(ErrorMessage = "El campo Codigo es obligatorio.")]
        public string Codigo { get; set; }
    }
}
