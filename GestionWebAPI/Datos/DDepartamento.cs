using GestionWebAPI.Configuracion;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DDepartamento
    {
        Conexion cn = new Conexion();

        public async Task<List<Departamento>> Obtener()
        {
            var objDepartemento = new List<Departamento>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ListarDepartamentos", sql))
                    {
                        await sql.OpenAsync();
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objDepartemento.Add(new Departamento
                                {
                                    DepartamentoID = (int)item["DepartamentoID"],
                                    NombreDepartamento = ((string)item["Departamento"]).ToUpper()
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


            return objDepartemento;
        }
    }
}
