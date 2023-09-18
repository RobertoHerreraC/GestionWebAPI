namespace GestionWebAPI.DTO
{
    public class RegistroAreaIndividualResponse
    {
        public int DerivadoID { get; set; }
        public string AreaDescripcion { get; set; }
        public List<EstadoAreaIndividual> EstadosArea { get; set; }
    }
}
