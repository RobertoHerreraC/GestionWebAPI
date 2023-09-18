using GestionWebAPI.Datos;
using GestionWebAPI.DTO;
using GestionWebAPI.Recursos;
using GestionWebAPI.Servicio;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DerivadoController : ControllerBase
    {
        [HttpPost]
        [Route("Guardar")]
        public async Task<ActionResult> RegistrarDerivacion([FromForm] DerivadoSolicitudRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDerivado = new DDerivado();
            var objDatosSoli = new DSolicitud();
            //var objResp = new DResponsable();
            var dPlantilla = new DPlantillaCorreo();
            var objRespon = new DResponsable();
            var correoServicio = new CorreoServicio();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
  
            SubirDocumento sb = new SubirDocumento();
            try
            {

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);
                var responsable = await objRespon.ObtenerPorReponsableID(modelo.ResponsableID);
                List<PlantillaCorreoResponse> plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.PETICION_INFORMACION);
                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(sdr.CodigoSolicitud, Constantes.Documento.DERIVACION_CONSULTA,modelo.ResponsableID.ToString());
                    RutaCompleta = sb.ObtenerSubRuta(subRuta, modelo.Documento);
                    
                }

                //Guardar documento
                if (!RutaCompleta.Equals(""))
                {
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }

                await objDerivado.RegistrarDerivado(modelo, Constantes.Documento.DERIVACION_CONSULTA, 
                    Constantes.Respuestas.DERIVAR_CONSULTA, RutaCompleta);

                foreach (var item in plantillas)
                {
                    if (item.Entidad.Equals(Constantes.CorreoEntidad.DERIVADOS))
                    {
                        var correoDerivado = new CorreoRequest();


                        correoDerivado.Asunto = item.Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                        correoDerivado.Contenido =item.CuerpoCorreo.
                            Replace("{Area}", responsable.NombreArea).Replace("{Responsable}", $"{responsable.Nombres} {responsable.ApellidoPaterno}").
                            Replace("{CodigoSolicitud}", sdr.CodigoSolicitud).
                            Replace("{FechaRegistro}", sdr.FechaPresentacion).
                            Replace("{Comentarios}", modelo.Comentario);
                        //falta la fecha

                        correoDerivado.Destinatario = responsable.Correo;
                        correoDerivado.Adjuntos = new List<string>();
                        correoDerivado.Adjuntos.Add(RutaCompleta);
                        correoServicio.EnviarCorreo(correoDerivado);
                    }
                }


                


                rsp.status = true;
                rsp.value = "Registro Exitoso";

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }

        [HttpPost]
        [Route("GuardarRespuestaIndividual")]
        public async Task<ActionResult> RegistrarRespuestaIndividual([FromForm] RespuestaIndividualAreaRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDerivado = new DDerivado();
            var objDatosSoli = new DSolicitud();
            //var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;

            SubirDocumento sb = new SubirDocumento();
            try
            {

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(sdr.CodigoSolicitud, Constantes.Documento.DERIVACION_RESPUESTA, modelo.DerivadoID.ToString());
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);

                }


                if (modelo.Respuesta == 1)
                {
                    modelo.Respuesta = 2;
                }
                else if (modelo.Respuesta == 0)
                {
                    modelo.Respuesta = 1;
                }

                


                await objDerivado.RegistrarRespuesta(modelo, Constantes.Documento.DERIVACION_RESPUESTA,
                    Constantes.Respuestas.DERIVAR_RESPUESTA, RutaCompleta);

                //Guardar documento
                if (!RutaCompleta.Equals(""))
                {
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }


                rsp.status = true;
                rsp.value = "Registro Exitoso";

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }



        [HttpPost]
        [Route("GuardarAcopioIndividual")]
        public async Task<ActionResult> RegistrarAcopioIndividual([FromForm] RegistroAcopioRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDerivado = new DDerivado();
            var objDatosSoli = new DSolicitud();
            //var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;

            SubirDocumento sb = new SubirDocumento();
            try
            {

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(sdr.CodigoSolicitud, Constantes.Documento.ACOPIO, modelo.DerivadoID.ToString());
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);

                }




                await objDerivado.RegistrarAcopio(modelo, Constantes.Documento.ACOPIO,
                    Constantes.Respuestas.ACOPIO_INDIVIDUAL, RutaCompleta, Constantes.Respuestas.ACOPIO_FINAL);

                //Guardar documento
                if (!RutaCompleta.Equals(""))
                {
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }


                rsp.status = true;
                rsp.value = "Registro Exitoso";

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }


        //[HttpGet]
        //[Route("Descargar")]
        //public HttpResponseMessage DownloadDocument()
        //{
        //    // Ruta al archivo PDF que deseas descargar
        //    string filePath = "ruta/del/archivo/documento.pdf";

        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El archivo no se encuentra.");
        //    }

        //    // Crea una respuesta HTTP para el archivo
        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open);

        //    // Asigna el contenido de la respuesta al flujo de archivo
        //    response.Content = new StreamContent(fileStream);

        //    // Configura el tipo de contenido del archivo (en este caso, PDF)
        //    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

        //    // Establece el encabezado Content-Disposition para indicar el nombre del archivo
        //    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = "documento.pdf"
        //    };

        //    return response;
        //}



        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Eliminar([FromBody] int id)
        [HttpDelete]
        [Route("EliminarDerivacion")]
        public async Task<ActionResult> Eliminar(ListarPorIDRequest  modelo)
        {
            var rsp = new Resp<string>();
            var objDatos = new DDerivado();
            try
            {
                await objDatos.Eliminar(modelo);
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

        [HttpPost]
        [Route("ListaPorSolicitud")]
        public async Task<ActionResult> Obtener(ListarPorIDRequest modelo)
        {
            var rsp = new Resp<List<DerivadosPorSolicitudResponse>>();
            var objDatos = new DDerivado();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerPorSolicitudID(modelo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }

        [HttpPost]
        [Route("ListaPorDerivacionIndividual")]
        public async Task<ActionResult> ObtenerEstadoIndividual(ListarPorIDRequest modelo)
        {
            var rsp = new Resp<RegistroAreaIndividualResponse>();
            var lisRep = new RegistroAreaIndividualResponse();
            lisRep.EstadosArea = new List<EstadoAreaIndividual>();
            var objRep = new EstadoIndividual();
            var objDatos = new DDerivado();
            try
            {
                rsp.status = true;
                objRep = await objDatos.ObtenerEstadosPorDerivado(modelo);


                lisRep.DerivadoID = objRep.DerivadoID;
                lisRep.AreaDescripcion = objRep.Area;
                if (objRep.DescripcionPeticion != null)
                {
                    var estado = new EstadoAreaIndividual
                    {
                        BitacoraEstado = objRep.BitacoraPeticionID,
                        DescripcionEstado = objRep.DescripcionPeticion.ToString(),
                        FechaHora = objRep.FechaHoraPeticion.ToString(),
                        //Ruta = objRep.RutaPeticion.ToString(),

                    };
                    lisRep.EstadosArea.Add(estado);
                }

                if (objRep.DescripcionRespuesta != null)
                {
                    var estado = new EstadoAreaIndividual
                    {
                        BitacoraEstado = objRep.BitacoraRespuestaPeticionID,
                        DescripcionEstado = objRep.DescripcionRespuesta.ToString(),
                        FechaHora = objRep.FechaHoraRespuesta.ToString(),
                        //Ruta = objRep.RutaRespuesta.ToString(),

                    };
                    lisRep.EstadosArea.Add(estado);
                }

                if (objRep.DescripcionAcopio != null)
                {
                    var estado = new EstadoAreaIndividual
                    {
                        BitacoraEstado = objRep.BitacoraAcopioID,
                        DescripcionEstado = objRep.DescripcionAcopio.ToString(),
                        FechaHora = objRep.FechaHoraAcopio.ToString(),
                        //Ruta = objRep.RutaAcopio.ToString(),

                    };
                    lisRep.EstadosArea.Add(estado);
                }


                rsp.value = lisRep;
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
