using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace GestionWebAPI.Datos
{
    public class DSolicitud
    {
        Conexion cn = new Conexion();

        public async Task<SolicitudIdentificador> Insertar(Solicitud modelo, Plazo plazo, int fraiID, int mpvId, int responsableId)
        {
            try
            {
                var esquema = new SolicitudIdentificador();
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("CrearSolicitud", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ruc", modelo.PersonaJuridica?.Ruc ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@RazonSocial", modelo.PersonaJuridica?.RazonSocial ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Nombres", modelo.PersonaNatural?.Nombres?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", modelo.PersonaNatural?.ApellidoPaterno ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", modelo.PersonaNatural?.ApellidoMaterno?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NroDocumento", modelo.PersonaNatural?.NroDocumento?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TipoDocumento", modelo.PersonaNatural?.TipoDocumento?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FraiID", fraiID);
                        cmd.Parameters.AddWithValue("@MpvID", mpvId);
                        cmd.Parameters.AddWithValue("@ResponsableClasificacionID", responsableId);
                        cmd.Parameters.AddWithValue("@Correo", modelo.Correo);
                        cmd.Parameters.AddWithValue("@Telefono", modelo.Telefono);
                        cmd.Parameters.AddWithValue("@InformacionSolicitada", modelo.InformacionSolicitada);
                        cmd.Parameters.AddWithValue("@FormaEntregaID", modelo.FormaEntregaID);
                        cmd.Parameters.AddWithValue("@Direccion", modelo.Direccion);
                        cmd.Parameters.AddWithValue("@DistritoID", modelo.DistritoID);
                        cmd.Parameters.AddWithValue("@PlazoMaximo",plazo.DiasPlazoMaximo);
                        cmd.Parameters.AddWithValue("@Prorroga", plazo.DiasProrroga);

                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                esquema.SolicitudID = (int)item["SolicitudId"];
                                esquema.CodigoSolicitud = (string)item["CodigoSolicitud"];
                            }
                        }
                    }
                }



                return esquema;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al guardar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al guardar datos : " + ex.Message, ex);
            }


        }

        public async Task<FiltroSolicitudResponse> ObtenerPorFiltro(FiltroSolicitudRequest modelo, PaginadoRequest pag)
        {
            var solicitudes = new FiltroSolicitudResponse();
            solicitudes.Solicitudes = new List<SolicitudResumen>();

            
            if ((modelo.FechaInicioPresentacion != null && modelo.FechaFinPresentacion == null) ||
                (modelo.FechaInicioPresentacion == null && modelo.FechaFinPresentacion != null))
            {
                throw new Exception("Debe enviar la Fecha de Inicio y Fin de presentación");
            }

            if ((modelo.FechaInicioPresentacion != null && modelo.FechaFinPresentacion != null))
            {
                if (!DateTime.TryParseExact(modelo.FechaInicioPresentacion, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime  fechaInicio))
                {
                    throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy.");
                }

                if (!DateTime.TryParseExact(modelo.FechaFinPresentacion, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaFin))
                {
                    throw new Exception("El formato de fecha y hora es incorrecto: dd-MM-yyyy.");
                }

            }


            try
            {
                
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ListarSolicitudesFiltro", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoSolicitud", modelo?.CodigoSolicitud ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CatalogoEstadoID", modelo?.CatalogoEstadoID ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NroDocumento", modelo?.NroDocumento ?? (Object)DBNull.Value);

                        if (modelo.FechaFinPresentacion != null && modelo.FechaInicioPresentacion != null)
                        {
                            DateTime.TryParse(modelo.FechaInicioPresentacion, out DateTime fechaInicio);
                            DateTime.TryParse(modelo.FechaFinPresentacion, out DateTime fechaFin);
                            
                            cmd.Parameters.AddWithValue("@FechaInicioPresentacion", fechaInicio);
                            cmd.Parameters.AddWithValue("@FechaFinPresentacion", fechaFin);
                        }
                        
                        cmd.Parameters.AddWithValue("@NumeroPagina", pag.NumeroPagina);
                        cmd.Parameters.AddWithValue("@TamañoPagina", pag.TamanoPagina);

                        
                        SqlParameter totalRegistrosParam = new SqlParameter("@TotalRegistros", SqlDbType.Int);
                        totalRegistrosParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(totalRegistrosParam);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        solicitudes.RegistroTotales = (int)totalRegistrosParam.Value;
                        Console.WriteLine("total registros: " + solicitudes.RegistroTotales.ToString());
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                solicitudes.Solicitudes.Add(new SolicitudResumen
                                {
                                    SolicitudID = (int)reader["SolicitudID"],
                                    CodigoSolicitud = reader["CodigoSolicitud"].ToString(),
                                    Administrado = reader["Administrado"].ToString(),
                                    TipoDocumento = reader["TipoDocumento"].ToString(),
                                    NroDocumento = reader["NroDocumento"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Telefono = reader["Telefono"].ToString(),
                                    FechaPresentacion = reader["FechaPresentacion"].ToString(),
                                    FormaEntrega = reader["FormaEntrega"].ToString(),
                                    FechaUltmaAtencion = reader["FechaUltmaAtencion"].ToString(),
                                    Tarea = reader["Tarea"].ToString(),
                                    TareaID = (int)reader["TareaID"],
                                    CatalogoEstado = reader["CatalogoEstado"].ToString(),
                                    GeneraCosto = (Boolean)reader["GeneraCosto"] == true? 1 : 0
                                }); 
                            }
                            
                        }
                    }
                }
                
            }
            catch (SqlException ex)
            {
                // Manejar la excepción SQL aquí
                throw new Exception("Error al obtener solicitudes: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones aquí
                throw new Exception("Error general al obtener solicitudes: ", ex);
            }

            return solicitudes;
        }


        public async Task<string> ObtenerCodigoPorId(int id)
        {
            string codigo = string.Empty;
            try
            {

                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerCodigoSolicitud", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", id);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                codigo = reader["CodigoSolicitud"].ToString();
                            }

                        }
                    }
                }

            }
            catch (SqlException ex)
            {

                throw new Exception("Error al obtener solicitudes: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {

                throw new Exception("Error general al obtener solicitudes: ", ex);
            }

            return codigo;
        }

        public async Task<SolicitudDetalleResponse> ObtenerDetalleSolcitudPorId(int id)
        {
            var solicitud = new SolicitudDetalleResponse();
            try
            {

                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    
                    using (var cmd = new SqlCommand("ObtenerDetalleSolicitud", sql)) 
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", id);

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                solicitud.SolicitudID = (int)reader["SolicitudID"];
                                solicitud.CodigoSolicitud = reader["CodigoSolicitud"].ToString();
                                solicitud.RUC = reader["RUC"]?.ToString() ;
                                solicitud.RazonSocial = reader["RazonSocial"]?.ToString();
                                solicitud.Nombres = reader["Nombres"]?.ToString();
                                solicitud.ApellidoPaterno = reader["ApellidoPaterno"]?.ToString();
                                solicitud.ApellidoMaterno = reader["ApellidoMaterno"]?.ToString();
                                solicitud.TipoDocumento = reader["TipoDocumento"]?.ToString();
                                solicitud.NroDocumento = reader["NroDocumento"]?.ToString();
                                solicitud.CorreoEletronico = reader["Correo"].ToString();
                                solicitud.Telefono = reader["Telefono"].ToString();
                                solicitud.InformacionSolicitada = reader["InformacionSolicitada"].ToString();
                                solicitud.Direccion = reader["Direccion"].ToString();
                                solicitud.CodigoSigedd = reader["CodigoSigedd"]?.ToString();
                                solicitud.CostoTotal = reader["CostoTotal"]?.ToString();
                                solicitud.FechaPresentacion = reader["FechaPresentacion"]?.ToString();
                                solicitud.FechaVencimiento = reader["FechaVencimiento"]?.ToString();
                                solicitud.FechaVencimientoProrroga = reader["FechaVencimientoProrroga"]?.ToString();
                                solicitud.PlazoMaximo =(Int16) reader["PlazoMaximo"];
                                solicitud.Prorroga = (Int16)reader["Prorroga"];
                                solicitud.FechaRegistroSolicitud = reader["FechaRegistroSolicitud"].ToString();
                                solicitud.DescripcionFormaEntrega = reader["DescripcionFormaEntrega"].ToString();
                                solicitud.Departamento = reader["Distrito"].ToString();
                                solicitud.Provincia = reader["Provincia"].ToString(); 
                                solicitud.Distrito = reader["Departamento"].ToString();

                            }

                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                // Manejar la excepción SQL aquí
                throw new Exception("Error al obtener solicitudes: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones aquí
                throw new Exception("Error general al obtener solicitudes: ", ex);
            }

            return solicitud;
        }
    }
}
