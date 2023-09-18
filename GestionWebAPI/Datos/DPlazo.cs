using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DPlazo
    {
        Conexion cn = new Conexion();

        public async Task <Plazo> Obtener()
        {
            var objPlazo = new Plazo();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerPlazoActivo", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objPlazo.DiasProrroga = (Int16)item["DiasProrroga"];
                                objPlazo.DiasPlazoMaximo = (Int16)item["DiasPlazoMaximo"];
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener datos : " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener datos : " + ex.ToString(), ex);
            }


            return objPlazo;
        }

        public async Task Insertar(Plazo modelo)
        {
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("InsertarPlazoConValidacion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DiasProrroga", modelo.DiasProrroga);
                        cmd.Parameters.AddWithValue("@DiasPlazoMaximo", modelo.DiasPlazoMaximo);
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
    }
}
