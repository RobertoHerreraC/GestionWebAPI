namespace GestionWebAPI.DTO
{
    public class DerivadosPorSolicitudResponse
    {
        public int DerivadoID { get; set; }
        public string Area { get; set; }
        public int AreaID { get; set; }
        public string EstadoArea { get; set; }
        public int TieneInformacion { get; set; }
        public string Comentario { get; set; }
        public string FechaPeticion { get; set; }
        //public string Ruta { get; set; }
    }
}
