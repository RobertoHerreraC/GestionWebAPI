namespace GestionWebAPI.Configuracion
{
    public class ConfiguracionCorreo
    {
        private string userName = string.Empty;
        private string host = string.Empty;
        private string port = string.Empty;
        private string pass = string.Empty;

        public ConfiguracionCorreo()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            userName = builder.GetSection("Email:UserName").Value;
            host = builder.GetSection("Email:Host").Value;
            port = builder.GetSection("Email:Port").Value;
            pass = builder.GetSection("Email:PassWord").Value;
        }

        public string UserName()
        {
            return userName;
        }


        public string Host()
        {
            return host;
        }

        public string Port()
        {
            return port;
        }

        public string Pass()
        {
            return pass;
        }
    }
}
