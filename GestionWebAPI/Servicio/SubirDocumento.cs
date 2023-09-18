using GestionWebAPI.Configuracion;
using GestionWebAPI.DTO;

namespace GestionWebAPI.Servicio
{
    public class SubirDocumento
    {
        ConfiguracionDocumento cd = new ConfiguracionDocumento();

        public string GuardarDocumento(string subruta, IFormFile archivo)
        {
            string rutaDocumento = string.Empty;
            
            try
            {
                if (!ValidarCarpeta(subruta))
                {
                    rutaDocumento = CrearCarpeta(subruta);
                }
                
                rutaDocumento = CrearCarpeta(subruta);
                rutaDocumento = Path.Combine(rutaDocumento, archivo.FileName);
                
                using (FileStream newFile = System.IO.File.Create(rutaDocumento))
                {
                    archivo.CopyTo(newFile);
                    newFile.Flush();
                }
                return rutaDocumento;
            }catch (Exception ex)
            {
                throw new Exception("Error al guardar documento: " + ex.Message, ex);
            }
        }
        public string ObtenerSubRuta(string subruta, IFormFile archivo)
        {
            string rutaDocumento = Path.Combine(subruta, archivo.FileName);
            return rutaDocumento;
            
        }
        public bool ValidarCarpeta(string subruta)
        {
            string rutaDocumento = Path.Combine(cd.Ruta(), subruta);
            try
            {
                if (Directory.Exists(rutaDocumento))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear carpeta: " + ex.Message, ex);
            }
        }

        public string CrearCarpeta(string subruta)
        {
            string rutaDocumento = Path.Combine(cd.Ruta(), subruta);
            try
            {
                if (!Directory.Exists(rutaDocumento))
                {
                    Directory.CreateDirectory(rutaDocumento);
                }
                return rutaDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear carpeta: " + ex.Message, ex);
            }
        }

        public string ObtenerRutaCompleta(string subruta)
        {
            string rutaDocumento = Path.Combine(cd.Ruta(), subruta);
            try
            {
                if (!Directory.Exists(rutaDocumento))
                {
                    Directory.CreateDirectory(rutaDocumento);
                }
                return rutaDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear carpeta: " + ex.Message, ex);
            }
        }
    }
}
