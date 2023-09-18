namespace GestionWebAPI.Modelo
{
    public class PersonaNatural
    {
        public string Nombres { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string NroDocumento { get; set; } = null!;
        public string TipoDocumento { get; set; } = null!;
    }
}
