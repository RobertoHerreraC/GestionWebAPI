using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Obtener()
        {
            var rsp = new Resp<List<Departamento>>();
            var objDatos = new DDepartamento();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.Obtener();
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
