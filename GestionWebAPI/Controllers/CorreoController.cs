using GestionWebAPI.Datos;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using GestionWebAPI.Servicio;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreoController : ControllerBase
    {
        [HttpPost]
        public IActionResult EnviarEmail([FromBody] CorreoRequest request)
        {
            var rsp = new Resp<string>();
            var objCorreo = new CorreoServicio();

            
            try
            {
                rsp.status = true;
                rsp.value = objCorreo.EnviarCorreo(request);
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
