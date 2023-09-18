namespace GestionWebAPI.DTO
{
    public class EstadoIndividual
    {
        public int DerivadoID { get; set; }
        public string Area { get; set; }
        public int BitacoraPeticionID { get; set; }
        public string DescripcionPeticion { get; set; }
        public string FechaHoraPeticion { get; set; }
        public string RutaPeticion { get; set; }
        public int BitacoraRespuestaPeticionID { get; set; }
        public string DescripcionRespuesta { get; set; }
        public string FechaHoraRespuesta { get; set; }
        public string RutaRespuesta { get; set; }

        public int BitacoraAcopioID { get; set; }
        public string DescripcionAcopio { get; set; }
        public string FechaHoraAcopio { get; set; }
        public string RutaAcopio { get; set; }
    }
}
