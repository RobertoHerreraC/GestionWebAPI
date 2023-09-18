namespace GestionWebAPI.Modelo
{
    public class Solicitud
    {
        public int? SolicitudId { get; set; }
        public PersonaJuridica? PersonaJuridica { get; set; }
        public PersonaNatural? PersonaNatural { get; set; }
        public string Correo { get; set; } = null!;
        public string Telefono { get; set; }
        public string InformacionSolicitada { get; set; }
        public int FormaEntregaID { get; set; }
        public string Direccion { get; set; }
        public int DistritoID { get; set; }

    }
}
