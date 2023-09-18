namespace GestionWebAPI.Modelo
{
    public class Bitacora
    {
        public int? BitacoraID { get; set; }
        public int SolicitudID { get; set; }
        public int TareaID { get; set; }
        public string FechaHoraAccion { get; set; }
        public string Comentario { get; set; }

    }
}
