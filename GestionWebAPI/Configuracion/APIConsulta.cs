using GestionWebAPI.Datos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace GestionWebAPI.Configuracion
{
    public class APIConsulta
    {
        private static string? _token;
        private static string _baseurl;
        private static string? _usuario;
        private static string? _clave;

        public APIConsulta()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _usuario = builder.GetSection("ApiSettings:usuario").Value;
            _clave = builder.GetSection("ApiSettings:clave").Value;
            _baseurl = builder.GetSection("ApiSettings:baseUrl").Value!;
            _token = "apis-token-5330.BLa3uNGI6hj1FptQn112sQ2wWV73Y0Y-";
        }

        public string Usuario()
        {
            return _usuario;
        }

        public string Clave()
        {
            return _clave;
        }

        public string BaseUrl()
        {
            return _baseurl;
        }

        public string Token()
        {
            return _token;
        }

        public async Task Autenticar()
        {
            _token = "apis-token-5330.BLa3uNGI6hj1FptQn112sQ2wWV73Y0Y-";
        }

        
    }
}
