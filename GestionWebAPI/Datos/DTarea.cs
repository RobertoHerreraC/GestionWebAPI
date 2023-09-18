using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DTarea
    {
        Conexion cn = new Conexion();

        public async Task<TareaEstado> ObtenerAlta()
        {
            var objTareaEstado = new TareaEstado();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerPrimeraBitacora", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objTareaEstado = new TareaEstado
                                {
                                    TareaID = (int)item["TareaID"],
                                    Descripcion = ((string)item["Descripcion"]).ToUpper(),
                                    Estado = new Estado
                                    {
                                        Descripcion = ((string)item["EstadoDescripcion"]).ToUpper()
                                    },
                                    AccionSistema = (Boolean)item["AccionSistema"] == true ? 1 : 0,
                                    Orden = (int)item["Orden"]
                                };
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


            return objTareaEstado;
        }
    }
}
