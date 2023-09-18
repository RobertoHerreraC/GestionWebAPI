using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlazoController : Controller
    {
        [HttpPost]
        public async Task <ActionResult> Guardar([FromBody] Plazo modelo)
        {
            var rsp = new Resp<string>();
            var objDatos = new DPlazo();
            try
            {
                await objDatos.Insertar(modelo);
                rsp.status = true;
                rsp.value = "Datos guardado exitosamente";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            
            return Ok(rsp);
        }


        [HttpGet("Obtener")]
        public async Task<ActionResult> Obtener()
        {
            var rsp = new Resp<Plazo>();
            var objDatos = new DPlazo();
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
