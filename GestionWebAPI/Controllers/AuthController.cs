using GestionWebAPI.Datos;
using GestionWebAPI.DTO;
using GestionWebAPI.Modelo;
using GestionWebAPI.Utilidad;
using Microsoft.AspNetCore.Mvc;

namespace GestionWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AutenticarRequest modelo)
        {
            
            var objDatos = new DAutenticar();
            try
            {
                LoginResponse r = await objDatos.Autenticar(modelo);
                var rsp = new Resp<LoginResponse>
                {
                    status = true,
                    value = r
                };
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                var rsp = new Resp<object>
                {
                    status = false,
                    msg = ex.Message  
                };
                return BadRequest(rsp);
            }


        }
    }
}
