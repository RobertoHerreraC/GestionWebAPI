using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciaController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult> Obtener(int id)
        {
            var rsp = new Resp<List<Provincia>>();
            var objDatos = new DProvincia();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.Obtener(id);
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
