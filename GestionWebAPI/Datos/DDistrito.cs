using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DDistrito
    {
        Conexion cn = new Conexion();

        public async Task<List<Distrito>> Obtener(int id)
        {
            var objDistrito = new List<Distrito>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ListarDistritos", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProvinciaID", id);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objDistrito.Add(new Distrito
                                {
                                    DistritoID = (int)item["DistritoID"],
                                    NombreDistrito= ((string)item["Distrito"]).ToUpper()
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


            return objDistrito;
        }
    }
}
