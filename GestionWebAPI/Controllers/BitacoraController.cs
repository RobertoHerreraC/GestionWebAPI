using GestionWebAPI.Datos;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using GestionWebAPI.Servicio;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionWebAPI.Recursos;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        [HttpPost]
        [Route("ListaPorSolicitud")]
        public async Task<ActionResult> Obtener(ListarPorIDRequest modelo)
        {
            var rsp = new Resp<List<EstadosPorSolicitudResponse>>();
            var objDatos = new DBitacora();
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
        [Route("ListaPorCodigo")]
        public async Task<ActionResult> ObtenerPorCodigo(ListarPorCodigoRequest modelo)
        {
            var rsp = new Resp<List<EstadosPorSolicitudResponse>>();
            var objDatos = new DBitacora();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerPorCodigo(modelo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }

        [HttpPost]
        [Route("GuardarRevision")]
        public async Task<ActionResult> RegistrarRevisionSolicitud( [FromForm] RespuestaSolicitudRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
            SubirDocumento sb = new SubirDocumento();
            var objMpv = new ResponsableDatosTipo();
            DPlantillaCorreo dPlantilla = new DPlantillaCorreo();
            CorreoServicio correoServicio = new CorreoServicio();
            List<PlantillaCorreoResponse> plantillas = new List<PlantillaCorreoResponse>();
            List<ResponsableDatosTipo> sdtList = new List<ResponsableDatosTipo>();
            try
            {

                sdtList = await objResp.ObtenerPorTipo(Constantes.Responsable.MPV);

                if (sdtList.Count>0)
                {
                    objMpv = sdtList[0];
                }
                else
                {
                    throw new Exception("No hay asignacion para la mesa de partes");
                }

                SolicitudDetalleResponse sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);
                string Principal = sdr.CodigoSolicitud;
                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(Principal, Constantes.Documento.VALIDACION);
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }
                

                 await objDatosBit.RegistrarRespuestaSolicitud(modelo,Constantes.Documento.VALIDACION,modelo.Respuesta,Constantes.Respuestas.VALIDACION_POSITIVA ,
                     Constantes.Respuestas.VALIDACION_NEGATIVA, RutaCompleta);

                if (modelo.Respuesta == 1)
                {
                    plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.VALIDACION_APROBADA);
                }else if (modelo.Respuesta == 0)
                {
                    plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.VALIDACION_RECHAZADA);
                }

                
                if (plantillas.Count() > 0)
                {
                    var correoMPV = new CorreoRequest();
                    string administrado = string.Empty;
                    string administradoResumen = string.Empty;
                    if ( sdr.NroDocumento?.Length>0)
                    {
                        administrado = $"<li>Nombres : {sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}</li>" +
                            $"<li>Tipo Documento : {sdr.TipoDocumento}</li>" +
                            $"<li>Nro Documento : {sdr.NroDocumento}</li>";
                        administradoResumen = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                    }
                    else if (sdr.RUC != null)
                    {
                        administrado = $"<li>Razón Social : {sdr.RazonSocial}</li>" +
                            $"<li>RUC : {sdr.RUC}</li>";
                        administradoResumen = sdr.RazonSocial;
                    }

                    if (plantillas[0] != null && plantillas[0].Entidad.Equals(Constantes.CorreoEntidad.MPV) && modelo.Respuesta == 1)
                    {


                        correoMPV.Asunto = plantillas[0].Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                        correoMPV.Contenido = plantillas[0].CuerpoCorreo.
                            Replace("{Asunto}", correoMPV.Asunto).Replace("{NumeroSolicitud}", sdr.CodigoSolicitud).
                            Replace("{FechaSolicitud}", DateTime.Now.ToString("dd-MM-yyyy HH:mm")).
                            Replace("{NombreSolicitante}", administrado);

                    }

                    if (plantillas[0] != null && plantillas[0].Entidad.Equals(Constantes.CorreoEntidad.MPV) && modelo.Respuesta == 0)
                    {
                        //var correoMPV = new CorreoRequest();
                        var razon = string.Empty;
                        if (modelo.Comentario != null)
                        {
                            razon = $"<p>Razón: </p><p>{modelo.Comentario}</p>";
                        }


                        correoMPV.Asunto = plantillas[0].Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                        correoMPV.Contenido = plantillas[0].CuerpoCorreo.
                            Replace("{Asunto}", correoMPV.Asunto).Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);

                        correoMPV.Contenido = correoMPV.Contenido.Replace("{RazonesRechazo}", razon);

                        
                    }
                    correoMPV.Destinatario = objMpv.Correo;
                    correoMPV.Adjuntos = new List<string>();
                    correoMPV.Adjuntos.Add(RutaCompleta);
                    correoServicio.EnviarCorreo(correoMPV);



                    if (plantillas.Count>1)
                    {
                        if (plantillas[1] != null && plantillas[1].Entidad.Equals(Constantes.CorreoEntidad.ADMINISTRADO) && modelo.Respuesta == 0)
                        {
                            var correoAdm = new CorreoRequest();
                            var razon = string.Empty;
                            if (modelo.Comentario != null)
                            {
                                razon = $"<p>Razón: </p><p>{modelo.Comentario}</p>";
                            }


                            correoAdm.Asunto = plantillas[1].Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                            correoAdm.Contenido = plantillas[1].CuerpoCorreo.
                                Replace("{Asunto}", correoAdm.Asunto).Replace("{Administrado}", administradoResumen).
                                Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);

                            correoAdm.Contenido = correoAdm.Contenido.Replace("{RazonesRechazo}", razon);

                            correoAdm.Destinatario = sdr.CorreoEletronico;
                            correoAdm.Adjuntos = new List<string>();
                            correoAdm.Adjuntos.Add(RutaCompleta);
                            correoServicio.EnviarCorreo(correoAdm);
                        }
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
        [Route("GuardarSigedd")]
        public async Task<ActionResult> RegistrarCodigoSigedd([FromForm] RegistroSigeddRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            DPlantillaCorreo dPlantilla = new DPlantillaCorreo();
            var correoServicio = new CorreoServicio();
            //SubirDocumento sb = new SubirDocumento();
            try
            {
                var sdtList = await objResp.ObtenerPorTipo(Constantes.Responsable.MPV);

                if (sdtList.Count == 0)
                    throw new Exception("No hay asignación para la mesa de partes");


                var sclList = await objResp.ObtenerPorTipo(Constantes.Responsable.RCL);

                if (sclList.Count == 0)
                    throw new Exception("No hay asignación para el responsable de la clasificacion");

                var objrcl = sclList[0];

                var objMpv = sdtList[0];
                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                var subRuta = string.Empty;
                var RutaCompleta = string.Empty;

                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(sdr.CodigoSolicitud, Constantes.Documento.SIGEDD);
                    RutaCompleta = new SubirDocumento().GuardarDocumento(subRuta, modelo.Documento);
                }


                await objDatosBit.RegistrarCodigoSigedd(modelo, Constantes.Documento.SIGEDD,
                    Constantes.Respuestas.REGISTRO_SIGEDD, RutaCompleta);

                var plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.REGISTRO_SIGEDD);

                var administrado = string.Empty;

                if (sdr.NroDocumento?.Length > 0)
                {
                    administrado = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                }
                else if (sdr.RUC != null)
                {
                    administrado = sdr.RazonSocial;
                }

                foreach ( var c in plantillas )
                {
                    var correo = new CorreoRequest();
                    if (c.Entidad.Equals(Constantes.CorreoEntidad.MPV))
                    {
                        correo.Asunto = c.Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                        correo.Contenido = c.CuerpoCorreo.
                            Replace("{Asunto}", correo.Asunto).Replace("{CodigoSigedd}", modelo.CodigoSigedd).
                            Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);

                        correo.Adjuntos = new List<string>();
                        correo.Adjuntos.Add(RutaCompleta);
                        correo.Destinatario = objMpv.Correo;
                    }

                    if (c.Entidad.Equals(Constantes.CorreoEntidad.ADMINISTRADO))
                    {
                        correo.Asunto = c.Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                        correo.Contenido = c.CuerpoCorreo.
                            Replace("{Asunto}", correo.Asunto).Replace("{CodigoSigedd}", modelo.CodigoSigedd).
                            Replace("{NumeroSolicitud}", sdr.CodigoSolicitud).Replace("{Administrado}", administrado);

                        correo.Destinatario = sdr.CorreoEletronico;
                    }

                    
                    
                    correoServicio.EnviarCorreo(correo);
                }


                //Notificar al clasificacion
                var plantillasClasificacion = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.DERIVAR_CLASIFICACION);
                var modeloBitacora = new Bitacora
                {
                    SolicitudID = modelo.SolicitudID,
                    TareaID = Constantes.Respuestas.CLASIFICACION_DERIVACION,
                    FechaHoraAccion = modelo.FechaHoraAccion

                };
                if (modelo.Comentario != null)
                {
                    modeloBitacora.Comentario = modelo.Comentario;
                }

                await objDatosBit.CrearBitacora(modeloBitacora);

                foreach (var item in plantillasClasificacion)
                {
                    if (item.Entidad.Equals(Constantes.CorreoEntidad.RCL))
                    {
                        var correo = new CorreoRequest();
                        correo.Asunto = item.Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                        correo.Contenido = item.CuerpoCorreo.
                            Replace("{Asunto}", correo.Asunto).Replace("{CodigoSigedd}", modelo.CodigoSigedd).
                            Replace("{NumeroRegistro}", sdr.CodigoSolicitud);

                        correo.Adjuntos = new List<string>();
                        correo.Adjuntos.Add(RutaCompleta);
                        correo.Destinatario = objrcl.Correo;
                        correoServicio.EnviarCorreo(correo);
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
        [Route("GuardarClasificacion")]
        public async Task<ActionResult> RegistrarRespuestaClasificacion([FromForm] RespuestaSolicitudRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
            var dPlantilla = new DPlantillaCorreo();
            var correoServicio = new CorreoServicio();
            SubirDocumento sb = new SubirDocumento();
            try
            {
                var sclList = await objResp.ObtenerPorTipo(Constantes.Responsable.RCL);

                if (sclList.Count == 0)
                    throw new Exception("No hay asignación para el responsable de la clasificacion");

                var objrcl = sclList[0];

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                string Principal = await objDatosSoli.ObtenerCodigoPorId(modelo.SolicitudID);
                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(Principal, Constantes.Documento.CLASIFICACION);
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }

                var administrado = string.Empty;

                if (sdr.NroDocumento?.Length > 0)
                {
                    administrado = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                }
                else if (sdr.RUC != null)
                {
                    administrado = sdr.RazonSocial;
                }

                await objDatosBit.RegistrarRespuestaSolicitud(modelo, Constantes.Documento.CLASIFICACION, modelo.Respuesta, Constantes.Respuestas.CLASIFICACION_POSITIVA,
                    Constantes.Respuestas.CLASIFICACION_NEGATIVA, RutaCompleta);

                var plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.CLASIFICACION_NO_PUB);
                if (modelo.Respuesta == 0)
                {
                    var razon = string.Empty;
                    if (modelo.Comentario != null)
                    {
                        razon = $"<p>Razón: </p><p>{modelo.Comentario}</p>";
                    }
                    foreach (var item  in plantillas)
                    {
                        if (item.Entidad.Equals(Constantes.CorreoEntidad.ADMINISTRADO))
                        {
                            var correo = new CorreoRequest();
                            correo.Asunto = item.Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                            correo.Contenido = item.CuerpoCorreo.
                                Replace("{Asunto}", correo.Asunto).Replace("{Administrado}", administrado).
                                Replace("{RazonesRechazo}", razon);

                            correo.Adjuntos = new List<string>();
                            correo.Adjuntos.Add(RutaCompleta);
                            correo.Destinatario = objrcl.Correo;
                            correoServicio.EnviarCorreo(correo);
                        }
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
        [Route("GuardarPago")]
        public async Task<ActionResult> RegistrarCostoSolicitud([FromForm] RespuestaPagoRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
            var dPlantilla = new DPlantillaCorreo();
            var correoServicio = new CorreoServicio();
            SubirDocumento sb = new SubirDocumento();
            try
            {
                var sclList = await objResp.ObtenerPorTipo(Constantes.Responsable.FRAI);

                if (sclList.Count == 0)
                    throw new Exception("No hay asignación para el Funcionario responsable de la Información");

                var objrcl = sclList[0];

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                string Principal = await objDatosSoli.ObtenerCodigoPorId(modelo.SolicitudID);
                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(Principal, Constantes.Documento.COSTO_SOLICITUD);
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }

                var administrado = string.Empty;

                if (sdr.NroDocumento?.Length > 0)
                {
                    administrado = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                }
                else if (sdr.RUC != null)
                {
                    administrado = sdr.RazonSocial;
                }

                await objDatosBit.RegistrarCostoSolicitud(modelo, Constantes.Documento.COSTO_SOLICITUD,
                    Constantes.Respuestas.REGISTRO_COSTO,
                     RutaCompleta);

                var plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.COSTO_SOLICITUD);
                
                var razon = string.Empty;
                if (modelo.Comentario != null)
                {
                    razon = $"<p>{modelo.Comentario}</p>";
                }
                foreach (var item in plantillas)
                {
                    if (item.Entidad.Equals(Constantes.CorreoEntidad.ADMINISTRADO))
                    {
                        var correo = new CorreoRequest();
                        correo.Asunto = item.Asunto.Replace("{CodigoSolicitud}", sdr.CodigoSolicitud);
                        correo.Contenido = item.CuerpoCorreo.
                            Replace("{asunto}", correo.Asunto).Replace("{Responsable}", administrado).
                            Replace("{CodigoSolicitud}", sdr.CodigoSolicitud).
                            Replace("{costo}", modelo.Costo.ToString()).
                            Replace("{emailRemitir}", objrcl.Correo).
                            Replace("{Comentarios}", razon); 

                        correo.Adjuntos = new List<string>();
                        correo.Adjuntos.Add(RutaCompleta);
                        correo.Destinatario = sdr.CorreoEletronico;
                        correoServicio.EnviarCorreo(correo);
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
        [Route("GuardarVoucher")]
        public async Task<ActionResult> RegistrarVoucherSolicitud([FromForm] RegistroBitacoraRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
            var dPlantilla = new DPlantillaCorreo();
            var correoServicio = new CorreoServicio();
            SubirDocumento sb = new SubirDocumento();
            try
            {
                //var sclList = await objResp.ObtenerPorTipo(Constantes.Responsable.RCL);

                //if (sclList.Count == 0)
                //    throw new Exception("No hay asignación para el responsable de la clasificacion");

                //var objrcl = sclList[0];

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                string Principal = await objDatosSoli.ObtenerCodigoPorId(modelo.SolicitudID);
                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(Principal, Constantes.Documento.REGISTRO_VOUCHER);
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }

                var administrado = string.Empty;

                if (sdr.NroDocumento?.Length > 0)
                {
                    administrado = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                }
                else if (sdr.RUC != null)
                {
                    administrado = sdr.RazonSocial;
                }

                await objDatosBit.RegistrarVoucherSolicitud(modelo, Constantes.Documento.REGISTRO_VOUCHER,
                    Constantes.Respuestas.REGISTRO_VOUCHER,
                     RutaCompleta);

                //var plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.CLASIFICACION_NO_PUB);

                var razon = string.Empty;
                if (modelo.Comentario != null)
                {
                    razon = $"<p>Razón: </p><p>{modelo.Comentario}</p>";
                }
                //foreach (var item in plantillas)
                //{
                //    if (item.Entidad.Equals(Constantes.CorreoEntidad.ADMINISTRADO))
                //    {
                //        var correo = new CorreoRequest();
                //        correo.Asunto = item.Asunto.Replace("{NumeroSolicitud}", sdr.CodigoSolicitud);
                //        correo.Contenido = item.CuerpoCorreo.
                //            Replace("{Asunto}", correo.Asunto).Replace("{Administrado}", administrado).
                //            Replace("{RazonesRechazo}", razon);

                //        correo.Adjuntos = new List<string>();
                //        correo.Adjuntos.Add(RutaCompleta);
                //        correo.Destinatario = objrcl.Correo;
                //        correoServicio.EnviarCorreo(correo);
                //    }
                //}






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
        [Route("GuardarEntrega")]
        public async Task<ActionResult> RegistrarEntregaSolicitud([FromForm] EntregaRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
            var dPlantilla = new DPlantillaCorreo();
            var correoServicio = new CorreoServicio();
            SubirDocumento sb = new SubirDocumento();
            try
            {

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                string Principal = await objDatosSoli.ObtenerCodigoPorId(modelo.SolicitudID);


                var administrado = string.Empty;

                if (sdr.NroDocumento?.Length > 0)
                {
                    administrado = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                }
                else if (sdr.RUC != null)
                {
                    administrado = sdr.RazonSocial;
                }

                await objDatosBit.RegistrarEntregaSolicitud(modelo,
                    Constantes.Respuestas.ENTREGA_INFORMACION);
                var razon = string.Empty;
                if (modelo.Comentario != null)
                {
                    razon = $"<p>Razón: </p><p>{modelo.Comentario}</p>";
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
        [Route("GuardarConfirmacionEntrega")]
        public async Task<ActionResult> RegistrarConfirmacion([FromForm] RegistroBitacoraRequest modelo)
        {
            var rsp = new Resp<string>();
            var objDatosBit = new DBitacora();
            var objDatosSoli = new DSolicitud();
            var objResp = new DResponsable();
            string subRuta = string.Empty;
            string RutaCompleta = string.Empty;
            var dPlantilla = new DPlantillaCorreo();
            var correoServicio = new CorreoServicio();
            SubirDocumento sb = new SubirDocumento();
            try
            {

                var sdr = await objDatosSoli.ObtenerDetalleSolcitudPorId(modelo.SolicitudID);

                string Principal = await objDatosSoli.ObtenerCodigoPorId(modelo.SolicitudID);
                if (modelo.Documento != null)
                {
                    subRuta = Path.Combine(Principal, Constantes.Documento.CONFIRMACION_ENTREGA);
                    RutaCompleta = sb.GuardarDocumento(subRuta, modelo.Documento);
                }

                var administrado = string.Empty;

                if (sdr.NroDocumento?.Length > 0)
                {
                    administrado = $"{sdr.Nombres} {sdr.ApellidoPaterno} {sdr.ApellidoMaterno}";
                }
                else if (sdr.RUC != null)
                {
                    administrado = sdr.RazonSocial;
                }

                await objDatosBit.RegistrarRecepcionEntrega(modelo, Constantes.Documento.CONFIRMACION_ENTREGA,
                    Constantes.Respuestas.RECEPCION_ENTREGA,
                     RutaCompleta);

                var razon = string.Empty;
                if (modelo.Comentario != null)
                {
                    razon = $"<p>Razón: </p><p>{modelo.Comentario}</p>";
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
    }
}
