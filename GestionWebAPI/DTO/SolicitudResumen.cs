namespace GestionWebAPI.DTO
{
    public class SolicitudResumen
    {
        public int SolicitudID { get; set; }
        public string CodigoSolicitud { get; set; }
        public string Administrado { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string FechaPresentacion { get; set; }
        public string FormaEntrega { get; set; }
        public string FechaUltmaAtencion { get; set; }
        public string Tarea { get; set; }
        public int TareaID { get; set; } = 0;
        public string CatalogoEstado { get; set; }
        public int GeneraCosto { get; set; }
    }
}
