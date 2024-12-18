﻿using Cundi_incidencias.Dto;
using Cundi_incidencias.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cundi_incidencias.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecuperarContrasenaController : ControllerBase
    {
        private readonly RecuperarContrasenaService _recuperarContrasenaService;
        public RecuperarContrasenaController(RecuperarContrasenaService recuperarContrasenaService)
        {
            _recuperarContrasenaService = recuperarContrasenaService;
        }


        [HttpPost("EnviarCodigo")]
        
        public async Task<IActionResult> EnviarCodigo([FromForm] string correo)
        {
            try
            {
                await _recuperarContrasenaService.Codigo(correo);
                return Ok(new { mensaje = "CODIGO HECHO EXITOSAMENTE" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }
        [HttpPost("ActualizarContrasena")]
        
        public async Task<IActionResult> ActualizarContrasena( [FromForm] int token ,[FromQuery] string nuevaContrasena)
        {
            try
            {
                bool resultado = await _recuperarContrasenaService.CambiarContrasena( token,  nuevaContrasena  );

                if (resultado)
                    return Ok(new { mensaje = "CONTRASEÑA ACTUALIZADA EXITOSAMENTE" });
                else
                    return BadRequest(new { mensaje = "CÓDIGO INVALIDO O EXPIRADO" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "ERROR: " + ex.Message);
            }
        }


    }
}
