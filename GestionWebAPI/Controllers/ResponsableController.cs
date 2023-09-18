using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Recursos;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsableController : ControllerBase
    {
        [HttpGet]
        [Route("porArea/{id:int}")]
        public async Task<IActionResult> ObtenerPorArea(int id)
        {
            var rsp = new Resp<List<ResponsableArea>>();
            var objDatos = new DResponsable();

            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerPorAreaID(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }//eliminar

        
    }
}
