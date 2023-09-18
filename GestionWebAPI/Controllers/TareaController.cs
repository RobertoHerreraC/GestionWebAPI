using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        [HttpGet("primeraTarea")]
        public async Task<ActionResult> ObtenerPrimerTarea()
        {
            var rsp = new Resp<TareaEstado>();
            var objDatos = new DTarea();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerAlta();
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
