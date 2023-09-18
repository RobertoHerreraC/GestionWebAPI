using GestionWebAPI.Datos;
using GestionWebAPI.Modelo;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoFormaEntregaController : Controller
    {
        [HttpGet("Lista")]
        public async Task<ActionResult> Obtener()
        {
            var rsp = new Resp<List<FormaEntrega>>();
            var objDatos = new DFormaEntrega();
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

        [HttpPost]
        public async Task<ActionResult> Guardar([FromBody] FormaEntrega modelo)
        {
            var rsp = new Resp<string>();
            var objDatos = new DFormaEntrega();
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar([FromBody] int id)
        {
            var rsp = new Resp<string>();
            var objDatos = new DFormaEntrega();
            try
            {
                await objDatos.Eliminar(id);
                rsp.status = true;
                rsp.value = "Dato se elimino exitosamente";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] FormaEntrega modelo)
        {
            var rsp = new Resp<string>();
            var objDatos = new DFormaEntrega();
            try
            {
                modelo.FormaEntregaID = id;
                await objDatos.Actualizar(modelo);
                rsp.status = true;
                rsp.value = "Dato se actualizo exitosamente";
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
