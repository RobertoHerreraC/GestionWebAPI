namespace GestionWebAPI.Modelo
{
    public class ResponsableDatosTipo
    {
        public int ResponsableID { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public int? AreaID { get; set; }
        public string? NombreArea { get; set; }
        public string? Correo { get; set; }
    }
}
