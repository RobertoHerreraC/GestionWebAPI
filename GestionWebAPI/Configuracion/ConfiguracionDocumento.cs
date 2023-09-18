namespace GestionWebAPI.Configuracion
{
    public class ConfiguracionDocumento
    {
        private string ruta = string.Empty;
        public ConfiguracionDocumento()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            ruta = builder.GetSection("Configuracion:RutaServidor").Value;
        }

        public string Ruta()
        {
            return ruta;
        }
    }
}
