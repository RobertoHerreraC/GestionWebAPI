namespace GestionWebAPI.DTO
{
    public class SolicitudDetalleResponse
    {

        public int SolicitudID { get; set; }
        public string CodigoSolicitud { get; set; }
        public string? RUC { get; set; }
        public string? RazonSocial { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NroDocumento { get; set; }
        public string CorreoEletronico { get; set; }
        public string Telefono { get; set; }
        public string InformacionSolicitada { get; set; }
        public string Direccion { get; set; }
        public string? CodigoSigedd { get; set; }
        public string? CostoTotal { get; set; }
        public string? FechaPresentacion { get; set; }
        public string? FechaVencimiento { get; set; }
        public string? FechaVencimientoProrroga { get; set; }
        public int PlazoMaximo { get; set; }
        public int Prorroga { get; set; }
        public string FechaRegistroSolicitud { get; set; }
        public string DescripcionFormaEntrega { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }

    }
}
