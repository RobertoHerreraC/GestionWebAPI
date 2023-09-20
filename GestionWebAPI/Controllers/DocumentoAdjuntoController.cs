using GestionWebAPI.Datos;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using GestionWebAPI.Servicio;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoAdjuntoController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Guardar([FromForm] DocumentoAdjunto modelo)
        {
            var rsp = new Resp<string>();
            var objDatos = new DDocumentoAdjunto();
            var subir = new SubirDocumento();
            try
            {

                string res = subir.GuardarDocumento(modelo.Ruta,modelo.Archivo);

                modelo.Ruta = res;
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

        
    }
}
