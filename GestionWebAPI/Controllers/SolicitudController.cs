using GestionWebAPI.Datos;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using GestionWebAPI.Recursos;
using GestionWebAPI.Servicio;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Mvc;
using static GestionWebAPI.Recursos.Constantes;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] Solicitud modelo)
        {
            var objDatos = new DSolicitud();

            DResponsable dRepObj = new DResponsable();

            DPlazo dPlaObj = new DPlazo();
            DBitacora dBotacoraObj = new DBitacora();
            DPlantillaCorreo dPlantilla = new DPlantillaCorreo();
            CorreoServicio correoServicio = new CorreoServicio();

            //String codigoSolicitud = String.Empty;
            int solicitudId = 0;
            var objPlazo = new Plazo();

            var listaFrai = new List<ResponsableDatosTipo>();
            var objFrai = new ResponsableDatosTipo();

            var listaMpv = new List<ResponsableDatosTipo>();
            var objMpv = new ResponsableDatosTipo();

            var listaRespCla = new List<ResponsableDatosTipo>();
            var objResCla = new ResponsableDatosTipo();

            var identificador = new SolicitudIdentificador();

            try
            {
                #region Plazo
                
                objPlazo = await dPlaObj.Obtener();

                #endregion Plazo

                #region FRAI
                //obtener FRAI
                listaFrai = await dRepObj.ObtenerPorTipo(Recursos.Constantes.Responsable.FRAI);

                if (listaFrai.Count > 0)
                {
                    objFrai = listaFrai[0];
                }
                else
                {
                    throw new Exception("No hay asignacion para el responsable de la informacion");
                }

                #endregion FRAI

                #region MPV
                listaMpv = await dRepObj.ObtenerPorTipo(Recursos.Constantes.Responsable.MPV);

                if (listaMpv.Count > 0)
                {
                    objMpv = listaMpv[0];
                }
                else
                {
                    throw new Exception("No hay asignacion para la mesa de partes");
                }

                #endregion MPV

                #region Clasificacion
                listaRespCla = await dRepObj.ObtenerPorTipo(Recursos.Constantes.Responsable.RCL);

                if (listaRespCla.Count > 0)
                {
                    objResCla = listaRespCla[0];
                }
                else
                {
                    throw new Exception("No hay asignacion para la mesa de partes");
                }

                #endregion Clasificacion

                #region Crear Solicitud
                identificador = await objDatos.Insertar(modelo, objPlazo, objFrai.ResponsableID,
                    objMpv.ResponsableID, objResCla.ResponsableID );

                #endregion Crear Solicitud
                
                int primeraBitacoraId = await dBotacoraObj.CrearPrimeraBitacora(identificador.SolicitudID);

                //generar doc

                //guardar doc en carpeta

                //guardar doc en bd


                #region Plantilla
                List<PlantillaCorreoResponse> plantillas = await dPlantilla.ObtenerPorCriterio(Constantes.CorreoCriterio.REGISTRO);
                if (plantillas.Count() > 0)
                {
                    string administrado = string.Empty;
                    if (modelo.PersonaNatural != null)
                    {
                        administrado = modelo.PersonaNatural.Nombres;
                    }
                    else if (modelo.PersonaJuridica != null)
                    {
                        administrado = modelo.PersonaJuridica.RazonSocial;
                    }

                    if (plantillas[0].Entidad.Equals(Constantes.CorreoEntidad.ADMINISTRADO))
                    {
                       
                        var correoAdminstrado = new CorreoRequest();

                        
                        correoAdminstrado.Asunto = plantillas[0].Asunto.ToString();
                        correoAdminstrado.Contenido = plantillas[0].CuerpoCorreo.
                            Replace("{Asunto}", plantillas[0].Asunto).Replace("{Administrado}", administrado).
                            Replace("{NumeroSolicitud}", identificador.CodigoSolicitud).
                            Replace("{FechaSolicitud}", DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                        //falta la fecha

                        correoAdminstrado.Destinatario = modelo.Correo;
                        correoServicio.EnviarCorreo(correoAdminstrado);

                    }

                    if (plantillas[1] != null && plantillas[1].Entidad.Equals(Constantes.CorreoEntidad.FRAI))
                    {
                        var correoFuncionario = new CorreoRequest();

                        correoFuncionario.Asunto = $"{identificador.CodigoSolicitud} - {plantillas[1].Asunto.ToString()}";
                        correoFuncionario.Contenido = plantillas[1].CuerpoCorreo.
                            Replace("{Asunto}", plantillas[1].Asunto).Replace("{ResponsableAccesoInformacion}", objFrai.Nombres).
                            Replace("{NombreSolicitante}", administrado).
                            Replace("{NumeroSolicitud}", identificador.CodigoSolicitud).
                            Replace("{FechaSolicitud}", DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                        //falta la fecha

                        correoFuncionario.Destinatario = objFrai.Correo;
                        
                        correoServicio.EnviarCorreo(correoFuncionario);

                    }


                }
                
                #endregion Plantilla

                var rsp = new Resp<object>
                {
                    status = true,
                    value = new { codigoSolicitud = identificador.CodigoSolicitud },
                };

                return Ok(rsp);
            }
            catch (Exception ex)
            {
                var rsp = new Resp<object>
                {
                    status = false,
                    msg = ex.Message
                };

                return BadRequest(rsp);
            }


        }

        [HttpPost]
        [Route("Listar")]
        public async Task<IActionResult> ListarConFiltro([FromBody] FiltroSolicitudRequest modelo, [FromQuery] PaginadoRequest paginado)
        {
            var rsp = new Resp<FiltroSolicitudResponse>();
            var objDatos = new DSolicitud();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerPorFiltro(modelo, paginado);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }


            return Ok(rsp);
        }

        [HttpPost]
        [Route("Detalle")]
        public async Task<IActionResult> DetalleSolicitudPorId([FromQuery] int id)
        {
            var rsp = new Resp<SolicitudDetalleResponse>();
            var objDatos = new DSolicitud();
            try
            {
                rsp.status = true;
                rsp.value = await objDatos.ObtenerDetalleSolcitudPorId(id);
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
