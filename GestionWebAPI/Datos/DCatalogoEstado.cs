using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DCatalogoEstado
    {
        Conexion cn = new Conexion();

        public async Task<List<CatalogoEstado>> Obtener()
        {
            var objEstados = new List<CatalogoEstado>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    
                    using (var cmd = new SqlCommand("ListarEstados", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objEstados.Add(new CatalogoEstado
                                {
                                    CatalogoEstadoID = (int)item["EstadoID"],
                                    Descripcion = ((string)item["Descripcion"]).ToUpper()
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

       
    }
}
