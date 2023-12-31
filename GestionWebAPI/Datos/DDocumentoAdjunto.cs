﻿using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionWebAPI.Datos
{
    public class DDocumentoAdjunto
    {
        Conexion cn = new Conexion();
        public async Task Insertar(DocumentoAdjunto modelo)
        {
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("InsertarDocumentoAdjunto", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BitacoraID", modelo.BitacoraID);
                        cmd.Parameters.AddWithValue("@NombreDocumento", modelo.NombreDocumento);
                        cmd.Parameters.AddWithValue("@Ruta", modelo.Ruta);
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al guardar datos : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al guardar datos : " + ex.Message, ex);
            }


        }

        public async Task<DocumentoAdjuntoResponse> ObtenerDocumentoPorId(int id)
        {
            var objDocumento = new DocumentoAdjuntoResponse();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerSubrutaPorID", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DocumentoAdjuntoID", id);
                       

                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objDocumento.BitacoraID = (int)item["BitacoraID"];
                                objDocumento.NombreDocumento = (string)item["NombreDocumento"];
                                objDocumento.Ruta = (string)item["Ruta"];
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

            return objDocumento;
        }

        public async Task<List<DocumentoSolicitudDto>> ObtenerListaDocumentosPorSolicitud(int id,string nombreDocumento)
        {
            var objDocumento = new List<DocumentoSolicitudDto>();
            try
            {
                using (var sql = new SqlConnection(cn.CadenaSQL()))
                {
                    using (var cmd = new SqlCommand("ObtenerDocumentoSolicitud", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SolicitudID", id);
                        cmd.Parameters.AddWithValue("@NombreDocumento", nombreDocumento);



                        await sql.OpenAsync();
                        using (var item = await cmd.ExecuteReaderAsync())
                        {
                            while (await item.ReadAsync())
                            {
                                objDocumento.Add(new DocumentoSolicitudDto {
                                    Nombres = (string)item["Nombres"],
                                    ApellidoPaterno = (string)item["ApellidoPaterno"],
                                    ApellidoMaterno = (string)item["ApellidoMaterno"],
                                    AreaNombre = (string)item["AreaNombre"],
                                    NombreDocumento = (string)item["NombreDocumento"],
                                    Ruta = (string)item["Ruta"],
                                    DocumentoAdjuntoID = (int)item["DocumentoAdjuntoID"]

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

            return objDocumento;
        }
    }

}
