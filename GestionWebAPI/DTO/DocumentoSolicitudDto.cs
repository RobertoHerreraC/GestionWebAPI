namespace GestionWebAPI.DTO
{
    public class DocumentoSolicitudDto
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }

        public string AreaNombre { get; set; }
        public string NombreDocumento { get; set; }

        public string Ruta { get; set; }
        public int DocumentoAdjuntoID { get; set; }


    }
}
