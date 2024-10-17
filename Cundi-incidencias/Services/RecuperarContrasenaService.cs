using Cundi_incidecnias.Utility;
using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;

namespace Cundi_incidencias.Services
{
    public class RecuperarContrasenaService
    {
        private readonly RecuperarContrasenaRepository _recuperarContrasenaRepository;

        public RecuperarContrasenaService(RecuperarContrasenaRepository recuperarContrasenaRepository)
        {
            _recuperarContrasenaRepository = recuperarContrasenaRepository;
        }
        public async Task<RecuperarContrasenaDto> Codigo(RecuperarContrasenaDto recuperarContrasena)
        {
            CodigoRecuperacionUtility token = new CodigoRecuperacionUtility();
            CorreoUtility correo = new CorreoUtility();
          
          
            RecuperarContrasenaDto recuperarContrasena1 = new RecuperarContrasenaDto();
            int codigo = token.NumeroAleatorio();
             string cod = codigo.ToString();
            string destinatario=await _recuperarContrasenaRepository.ObtenerCorreoPorID(recuperarContrasena.id_usuario);
                correo.enviarCorreoContrasena(destinatario, cod);
            recuperarContrasena.token = codigo;
            recuperarContrasena1 = await _recuperarContrasenaRepository.Codigo(recuperarContrasena);
            return recuperarContrasena;

        }
        public async Task<bool> CambiarContrasenaSiCodigoEsValido(int id_usuario, string token, string nuevaContrasena)
        {
            // Obtener el registro de recuperación
            var recuperarContrasenaDto = await _recuperarContrasenaRepository.ObtenerRecuperacionPorUsuarioYToken(id_usuario, token);

            if (recuperarContrasenaDto != null && DateTime.Now <= recuperarContrasenaDto.fecha_expiracion)
            {
                await _usuarioRepository.ActualizarContrasena(id_usuario, nuevaContrasena);
                return true;
            }

            return false;
        }

        public async Task<bool> ValidarCodigoYActualizarContrasena(int id_usuario, string token, string nuevaContrasena)
        {
            return await _recuperarContrasenaRepository.ValidarCodigoYActualizarContrasena(id_usuario, token, nuevaContrasena);
        }


    }
}
