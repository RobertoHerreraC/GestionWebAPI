using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DAutenticar
    {
        Conexion cn = new Conexion();
        public async Task<LoginResponse> Autenticar(AutenticarRequest modelo)
        {
            var resultado = new LoginResponse();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ValidarAutenticacion", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Usuario", modelo.Usuario);
                        cmd.Parameters.AddWithValue("@Pass", modelo.Contrasena);
                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                resultado.Mensaje = item["Mensaje"].ToString();
                                resultado.Valor = (bool)item["Valor"];
                            }
                        }
                    }
                }
                return resultado;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al autenticar: " + ex.ToString(), ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al autenticar: " + ex.ToString(), ex);
            }


        }
    }
}
