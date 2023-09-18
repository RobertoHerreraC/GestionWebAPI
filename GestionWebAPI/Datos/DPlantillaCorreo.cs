using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DPlantillaCorreo
    {
        Conexion cn = new Conexion();
     
        public async Task<List<PlantillaCorreoResponse>> ObtenerPorCriterio(string criterio)
        {
            var objPlantilla = new List<PlantillaCorreoResponse>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {

                    using (var cmd = new SqlCommand("ObtenerCorreosPorCriterio", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Criterio", criterio);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objPlantilla.Add(new PlantillaCorreoResponse
                                {
                                    CorreoID = (int)item["CorreosID"],
                                    Criterio = ((string)item["Criterio"]),
                                    Entidad = ((string)item["Entidad"]),
                                    Asunto = ((string)item["Asunto"]),
                                    CuerpoCorreo = (string)item["CuerpoCorreo"]
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


            return objPlantilla;
        }

    }
}
