using System.Text.Json.Serialization;

namespace GestionWebAPI.Modelo
{
    public class FormaEntrega
    {
        public int? FormaEntregaID { get; set; }
        public string? Descripcion { get; set; }
        public int? GeneraCosto { get; set; }
    }
}
