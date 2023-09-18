using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DResponsable
    {
        Conexion cn = new Conexion();

        public async Task<List<ResponsableArea>> ObtenerPorAreaID(int id)//eliminar
        {
            try
            {
                var responsables = new List<ResponsableArea>();
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerResponsablesPorAreaID", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AreaID", id);

                        

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                responsables.Add(new ResponsableArea
                                {
                                    ResponsableID = reader["ResponsableID"].ToString(),
                                    Responsable = new PersonaResponsable
                                    {
                                        Nombres = reader["Nombres"].ToString(),
                                        ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                                        ApellidoMaterno = reader["ApellidoMaterno"].ToString(),
                                    },
                                    Area = new AreaResponsable
                                    {
                                        AreaID = reader["AreaID"].ToString(),
                                        Nombre = reader["NombreArea"].ToString()
                                    },
                                }); ;
                            }
                        }
                    }
                }
                return responsables;
            }
            catch (SqlException ex)
            {
                // Manejar la excepción SQL aquí
                throw new Exception("Error al obtener responsables: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones aquí
                throw new Exception("Error general al obtener responsables: ", ex);
            }
        }


        public async Task<List<ResponsableDatosTipo>> ObtenerPorTipo(string tipo)
        {
            try
            {
                var responsables = new List<ResponsableDatosTipo>();
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerResponsablesTipoPorTipo", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Tipo", tipo);



                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                responsables.Add(new ResponsableDatosTipo
                                {
                                    ResponsableID = (int)reader["ResponsableID"],
                                    Nombres = reader["Nombres"].ToString(),
                                    ApellidoPaterno = reader["ApellidoPaterno"].ToString(),
                                    ApellidoMaterno = reader["ApellidoMaterno"].ToString(),
                                    AreaID = (int)reader["AreaID"],
                                    NombreArea = reader["NombreArea"].ToString(),
                                    Correo = reader["Correo"].ToString()
                                }); 
                            }
                        }
                    }
                }
                return responsables;
            }
            catch (SqlException ex)
            {
                // Manejar la excepción SQL aquí
                throw new Exception("Error al obtener responsables: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones aquí
                throw new Exception("Error general al obtener responsables: ", ex);
            }
        }

        public async Task<ResponsableDatosTipo> ObtenerPorReponsableID(int idResponsable)
        {
            try
            {
                var responsable = new ResponsableDatosTipo();
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerResponsablePorID", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ResponsableID", idResponsable);



                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                responsable.ResponsableID = (int)reader["ResponsableID"];
                                responsable.Nombres = reader["Nombres"].ToString();
                                responsable.ApellidoPaterno = reader["ApellidoPaterno"].ToString();
                                responsable.ApellidoMaterno = reader["ApellidoMaterno"].ToString();
                                responsable.AreaID = (int)reader["AreaID"];
                                responsable.NombreArea = reader["NombreArea"].ToString();
                                responsable.Correo = reader["Correo"].ToString();

                            }
                        }
                    }
                }
                return responsable;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener responsables: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener responsables: ", ex);
            }
        }

    }
}
