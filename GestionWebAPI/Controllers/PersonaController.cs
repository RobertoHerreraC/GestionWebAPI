using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Servicio;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        [HttpGet("reniec")]
        public async Task<IActionResult> reniec(string dni)
        {
            var rsp = new Resp<PersonaNaturalConsulta>();
            var objDatos = new ConsultaPersona();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.validarDni(dni);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }

        [HttpGet("sunat")]
        public async Task<IActionResult> sunat(string ruc)
        {
            var rsp = new Resp<PersonaJuridicaConsulta>();
            var objDatos = new ConsultaPersona();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.validarRuc(ruc);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }
    }
}
