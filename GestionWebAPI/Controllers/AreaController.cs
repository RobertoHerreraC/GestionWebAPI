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
    public class AreaController : ControllerBase
    {
        [HttpGet]
        [Route("AreasDerivado")]
        public async Task<IActionResult> ObtenerPorArea()
        {
            var rsp = new Resp<List<ResponsableDatosTipo>>();
            var objDatos = new DResponsable();

            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerPorTipo(Constantes.Responsable.DERIVADO);
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
