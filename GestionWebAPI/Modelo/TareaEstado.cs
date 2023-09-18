namespace GestionWebAPI.Modelo
{
    public class TareaEstado
    {
        public int TareaID { get; set; }
        public string Descripcion { get; set; }
        public Estado Estado { get; set; }
        public int AccionSistema { get; set; }
        public int Orden { get; set; }
        
    }
}
