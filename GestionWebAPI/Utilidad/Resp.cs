namespace GestionWebAPI.Utilidad
{
    public class Resp<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string msg { get; set; }
    }
}
