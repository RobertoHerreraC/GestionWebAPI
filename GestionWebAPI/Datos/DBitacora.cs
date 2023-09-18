using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using GestionWebAPI.Servicio;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace GestionWebAPI.Datos
{
    public class DBitacora
    {
        Conexion cn = new Conexion();
        public async Task<int> CrearPrimeraBitacora(int idSolicitud)
        {
            int bitacoraId = 0;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("CrearPrimeraBitacora", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", idSolicitud);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                bitacoraId = (int)item["BitacoraID"];
                            }
                        }
                    }
                }
                return bitacoraId;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al guardar datos : " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al guardar datos : " + ex.ToString(), ex);
            }


        }

        public async Task<List<EstadosPorSolicitudResponse>> ObtenerPorSolicitudID(ListarPorIDRequest modelo)
        {
            var objEstados = new List<EstadosPorSolicitudResponse>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ListarEstadoSolicitudID", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.ID);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objEstados.Add(new EstadosPorSolicitudResponse
                                {
                                    BitacoraID = (int)item["BitacoraID"],
                                    Descripcion = ((string)item["Tarea"]).ToUpper(),
                                    Estado = ((string)item["Estado"]).ToUpper(),
                                    FechaHora = ((string)item["BitacoraFechaHora"])
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


            return objEstados;
        }

        public async Task<List<EstadosPorSolicitudResponse>> ObtenerPorCodigo(ListarPorCodigoRequest modelo)
        {
            var objEstados = new List<EstadosPorSolicitudResponse>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ListarEstadoCodigoSolicitud", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoSolicitud", modelo.Codigo);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objEstados.Add(new EstadosPorSolicitudResponse
                                {
                                    BitacoraID = (int)item["BitacoraID"],
                                    Descripcion = ((string)item["Tarea"]).ToUpper(),
                                    Estado = ((string)item["Estado"]).ToUpper(),
                                    FechaHora = ((string)item["BitacoraFechaHora"])
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


            return objEstados;
        }

        public async Task<string> RegistrarRespuestaSolicitud(RespuestaSolicitudRequest modelo, string caso, int respuesta, int tareaPositiva, int tareaNegativa, string ruta)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }

            if (respuesta<0  || respuesta>1)
            {
                throw new Exception("Respuesta no permitida.");
            }


            var objBitacora = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarRevision", sql)) 
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario?? (Object) DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@Respuesta", respuesta);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta",(Object) DBNull.Value);
                        }
                        
                        cmd.Parameters.AddWithValue("@TareaIDPositiva", tareaPositiva);
                        cmd.Parameters.AddWithValue("@TareaIDNegativa", tareaNegativa);

                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objBitacora = item["BitacoraID"].ToString();

                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }

        public async Task<string> RegistrarCodigoSigedd(RegistroSigeddRequest modelo, string caso, int tareaID, string ruta)
        {
            var objBitacora = string.Empty;
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }

            
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarSigedd", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@CodigoSigedd", modelo.CodigoSigedd);
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
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objBitacora = item["BitacoraID"].ToString();

                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }

        public async Task<string> CrearBitacora(Bitacora modelo)
        {
            var objBitacora = string.Empty;
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }


            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarBitacora", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@TareaID", modelo.TareaID);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                     

                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objBitacora = item["BitacoraID"].ToString();

                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }

        public async Task<string> RegistrarCostoSolicitud(RespuestaPagoRequest modelo, string caso,  int tarea,  string ruta)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }



            var objBitacora = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarPago", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@CostoTotal", modelo.Costo);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta", (Object)DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@TareaID", tarea);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }


        public async Task<string> RegistrarVoucherSolicitud(RegistroBitacoraRequest modelo, string caso, int tarea, string ruta)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }



            var objBitacora = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarVoucher", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta", (Object)DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@TareaID", tarea);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }

        public async Task<string> RegistrarEntregaSolicitud(EntregaRequest modelo, int tarea)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }



            var objBitacora = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarEntrega", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        cmd.Parameters.AddWithValue("@TareaID", tarea);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }

        public async Task<string> RegistrarRecepcionEntrega(RegistroBitacoraRequest modelo, string caso, int tarea, string ruta)
        {
            if (!DateTime.TryParseExact(modelo.FechaHoraAccion, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaHoraAccion))
            {
                throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy HH:mm.");
            }



            var objBitacora = string.Empty;
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("RegistrarConfirmacionEntrega", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caso", caso);
                        cmd.Parameters.AddWithValue("@SolicitudID", modelo.SolicitudID);
                        cmd.Parameters.AddWithValue("@Comentario", modelo.Comentario ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaHoraAccion", fechaHoraAccion);
                        if (ruta.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Ruta", ruta);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ruta", (Object)DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@TareaID", tarea);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al registrar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al registrar datos : " + ex.Message, ex);
            }


            return objBitacora;
        }
    }
}
