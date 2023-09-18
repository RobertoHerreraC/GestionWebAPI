namespace GestionWebAPI.Modelo
{
    public class DocumentoAdjunto
    {
        public int BitacoraID { get; set; }
        public string NombreDocumento { get; set; }
        public string Ruta { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
