using GestionWebAPI.Modelo;
using System.ComponentModel.DataAnnotations;

namespace GestionWebAPI.DTO
{
    public class SolicitudRequest
    {
        public int? SolicitudId { get; set; }
        public PersonaJuridica? PersonaJuridica { get; set; }
        public PersonaNatural? PersonaNatural { get; set; }

        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "El campo Telefono es obligatorio.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo InformacionSolicitada es obligatorio.")]
        public string InformacionSolicitada { get; set; }

        [Required(ErrorMessage = "El campo FormaEntregaID es obligatorio.")]
        public int FormaEntregaID { get; set; }

        [Required(ErrorMessage = "El campo Direccion es obligatorio.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El campo DistritoID es obligatorio.")]
        public int DistritoID { get; set; }
    }
}
