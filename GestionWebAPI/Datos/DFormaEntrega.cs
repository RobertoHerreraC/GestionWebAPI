using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DFormaEntrega
    {
        Conexion cn = new Conexion();

        public async Task<List<FormaEntrega>> Obtener()
        {
            var objFormaEntrega = new List<FormaEntrega>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerFormaEntrega", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objFormaEntrega.Add(new FormaEntrega {
                                    FormaEntregaID = (int)item["FormaEntregaID"],
                                    Descripcion = ((string)item["Descripcion"]).ToUpper(),
                                    GeneraCosto = (Boolean)item["GeneraCosto"] == true ? 1 : 0
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


            return objFormaEntrega;
        }


        public async Task Insertar(FormaEntrega modelo)
        {
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("InsertarFormaEntrega", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Descripcion", modelo.Descripcion);
                        cmd.Parameters.AddWithValue("@GeneraCosto", modelo.GeneraCosto == 1 ? true : false);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
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


        public async Task Eliminar(int id)
        {
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("EliminarFormaEntrega", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FormaEntregaID",id);
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

        public async Task Actualizar(FormaEntrega modelo)
        {
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ActualizarFormaEntrega", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FormaEntregaID", modelo.FormaEntregaID);
                        cmd.Parameters.AddWithValue("@Descripcion", modelo.Descripcion);
                        cmd.Parameters.AddWithValue("@GeneraCosto", modelo.GeneraCosto);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al actualizar datos : " + ex.Message, ex);
            }


        }
    }
}
