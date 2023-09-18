using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GestionWebAPI.Servicio
{
    public class ConsultaPersona
    {
        APIConsulta objConsulta = new APIConsulta();

        public async Task<PersonaNaturalConsulta> validarDni(string numDocumento)
        {
            PersonaNaturalConsulta objpersona = new PersonaNaturalConsulta();

            //await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(objConsulta.BaseUrl());
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objConsulta.Token());
            var response = await cliente.GetAsync($"reniec/dni?numero={numDocumento}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<PersonaNaturalConsulta>(json_respuesta);
                objpersona.Nombres = resultado!.Nombres;
                objpersona.ApellidoPaterno = resultado.ApellidoPaterno;
                objpersona.ApellidoMaterno = resultado.ApellidoMaterno;
                objpersona.NumeroDocumento = resultado.NumeroDocumento;
            }
            return objpersona;
        }

        public async Task<PersonaJuridicaConsulta> validarRuc(string numRuc)
        {
            PersonaJuridicaConsulta persona = new PersonaJuridicaConsulta();

            //await Autenticar();

            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri(objConsulta.BaseUrl());
            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", objConsulta.Token());
            var response = await cliente.GetAsync($"sunat/ruc?numero={numRuc}");

            if (response.IsSuccessStatusCode)
            {
                var json_respuesta = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<PersonaJuridicaConsulta>(json_respuesta);
                persona.razonSocial = resultado!.razonSocial;
                persona.NumeroDocumento = resultado!.NumeroDocumento;
            }
            return persona;
        }
    }
}
