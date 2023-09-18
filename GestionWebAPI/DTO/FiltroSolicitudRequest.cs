namespace GestionWebAPI.DTO
{
    public class FiltroSolicitudRequest
    {
        public string? CodigoSolicitud { get; set; }
        public int? CatalogoEstadoID { get; set; }
        public string? NroDocumento { get; set; }
        public string? FechaInicioPresentacion { get; set; }
        public string? FechaFinPresentacion { get; set; }

    }
}
