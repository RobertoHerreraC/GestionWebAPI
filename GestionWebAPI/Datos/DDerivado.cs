using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using static GestionWebAPI.Recursos.Constantes;

namespace GestionWebAPI.Datos
{
    public class DDerivado
    {
        Conexion cn = new Conexion();
        public async Task<string> RegistrarDerivado(DerivadoSolicitudRequest modelo, string caso,  int tareaID, string ruta)
        {

            var objDerivado = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarDerivadoBitacora", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@ResponsableID", modelo.ResponsableID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta", (Object)DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@TareaID", tareaID);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        
                    }
                }

                objDerivado = "Registro existoso";
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objDerivado;
        }





        public async Task<string> RegistrarRespuesta(RespuestaIndividualAreaRequest modelo, string caso, int tareaID, string ruta)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }

            if (modelo.Respuesta < 0 || modelo.Respuesta > 2)
            {
                throw new Exception("Respuesta no permitida.");
            }

            var objDerivado = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarRespuestaDerivado", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@DerivadoID", modelo.DerivadoID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta", (Object)DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@Respuesta", modelo.Respuesta);
                        cmd.Parameters.AddWithValue("@TareaID", tareaID);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                    }
                }

                objDerivado = "Registro existoso";
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objDerivado;
        }


        public async Task<string> RegistrarAcopio(RegistroAcopioRequest modelo, string caso, int tareaID, string ruta, int tareaFinalAcopio)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }


            var objDerivado = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarAcopio", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@DerivadoID", modelo.DerivadoID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta", (Object)DBNull.Value);
                        }
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@TareaID", tareaID);
                        cmd.Parameters.AddWithValue("@TareaAcopioFinalID", tareaFinalAcopio);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                    }
                }

                objDerivado = "Registro existoso";
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objDerivado;
        }














        public async Task Eliminar(ListarPorIDRequest modelo)
        {
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("EliminarDerivadoBitacora", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DerivadoID", modelo.ID);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar datos : " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al eliminar datos : " + ex.ToString(), ex);
            }


        }

        public async Task<List<DerivadosPorSolicitudResponse>> ObtenerPorSolicitudID(ListarPorIDRequest modelo)
        {
            var objDerivados = new List<DerivadosPorSolicitudResponse>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ListarDerivadoSolicitud", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.ID);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objDerivados.Add(new DerivadosPorSolicitudResponse
                                {
                                    DerivadoID = (int)item["DerivadoID"],
                                    Area = (string)item["Area"],
                                    AreaID = (int)item["AreaID"],
                                    EstadoArea = ((string)item["EstadoArea"]).ToUpper(),
                                    TieneInformacion = (Int16)item["TieneInformacion"],
                                    Comentario = ((string)item["Comentario"]).ToUpper(),
                                    FechaPeticion = (string)item["FechaPeticion"],
                                    //Ruta = (string)item["Ruta"]
                                  
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener datos : " + ex.Message, ex);
            }


            return objDerivados;
        }

        public async Task<EstadoIndividual> ObtenerEstadosPorDerivado(ListarPorIDRequest modelo)
        {
            var objDerivados = new EstadoIndividual();

            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ListarEstadoDerivadoIndividual", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DerivadoID", modelo.ID);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objDerivados.DerivadoID = (int)item["DerivadoID"];
                                objDerivados.Area = ((string)item["Area"]).ToUpper();
                                objDerivados.BitacoraPeticionID = (int)item["BitacoraPeticionID"];
                                objDerivados.DescripcionPeticion = ((string)item["DescripcionPeticion"]).ToUpper();
                                objDerivados.FechaHoraPeticion = (string)item["FechaHoraPeticion"];
                                objDerivados.RutaPeticion = (string)item["RutaPeticion"];
                     
                                if (!item.IsDBNull(item.GetOrdinal("BitacoraRespuestaPeticionID")))
                                {
                                    objDerivados.BitacoraRespuestaPeticionID = (int)item["BitacoraRespuestaPeticionID"];
                                    objDerivados.DescripcionRespuesta = ((string)item["DescripcionRespuesta"]).ToUpper();
                                    objDerivados.FechaHoraRespuesta = (string)item["FechaHoraRespuesta"];
                                    objDerivados.RutaRespuesta = (string)item["RutaRespuesta"];
                                }
                                if (!item.IsDBNull(item.GetOrdinal("BitacoraAcopioID")))
                                {
                                    objDerivados.BitacoraAcopioID = (int)item["BitacoraAcopioID"];
                                    objDerivados.DescripcionAcopio = ((string)item["DescripcionAcopio"]).ToUpper();
                                    objDerivados.FechaHoraAcopio = (string)item["FechaHoraAcopio"];
                                    objDerivados.RutaAcopio = (string)item["RutaAcopio"];
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener datos : " + ex.Message, ex);
            }


            return objDerivados;
        }
    }
}
