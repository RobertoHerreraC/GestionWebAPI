using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace GestionWebAPI.Datos
{
    public class DProvincia
    {
        Conexion cn = new Conexion();

        public async Task<List<Provincia>> Obtener(int id)
        {
            var objProvincia = new List<Provincia>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ListarProvincias", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DepartamentoID", id);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objProvincia.Add(new Provincia
                                {
                                    ProvinciaID = (int)item["ProvinciaID"],
                                    NombreProvincia = ((string)item["Provincia"]).ToUpper()
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


            return objProvincia;
        }

    }
}
