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

        //[HttpGet("descargar-archivo/{id}")]
        //public async Task<ActionResult> Descargar(int id)
        //{
        //    var objDatos = new DDocumentoAdjunto();
        //    var subir = new SubirDocumento();
        //    try
        //    {
        //        //obtener ruta de bd
        //        var resRuta = objDatos.ObtenerDocumentoPorId(id);

        //        if (resRuta == null)
        //        {
        //            return NotFound();
        //        }

        //        //Construir ruta completa archivo
        //        //var rutaArchivo = subir.Va;


        //       // if (!System.IO.File.Exists(rutaArchivo))
        //        //{
        //          //  return NotFound(); // El archivo no fue encontrado en el servidor
        //        //}

        //        //var contenidoArchivo = System.IO.File.ReadAllBytes(rutaArchivo);
        //        //var contentType = "application/octet-stream";
        //        return File(contenidoArchivo, contentType);

        //    }
        //    catch (Exception ex)
        //    {
        //        rsp.status = false;
        //        rsp.msg = ex.Message;
        //    }


        //    return Ok(rsp);
        //}
    }
}
