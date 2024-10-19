using Cundi_incidecnias.Utility;
using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;

namespace Cundi_incidencias.Services
{
    public class RecuperarContrasenaService
    {
        private readonly RecuperarContrasenaRepository _recuperarContrasenaRepository;
        private readonly UsuarioRepository _usuarioRepository;

        public RecuperarContrasenaService(RecuperarContrasenaRepository recuperarContrasenaRepository, UsuarioRepository usuarioRepository)
        {
            _recuperarContrasenaRepository = recuperarContrasenaRepository;
            _usuarioRepository = usuarioRepository;
        }
        public async Task<RecuperarContrasenaDto> Codigo(RecuperarContrasenaDto recuperarContrasena)
        {
            CodigoRecuperacionUtility token = new CodigoRecuperacionUtility();
            CorreoUtility correo = new CorreoUtility();
          
          
            RecuperarContrasenaDto recuperarContrasena1 = new RecuperarContrasenaDto();
            int codigo = token.NumeroAleatorio();
            string cod = codigo.ToString();
            await _recuperarContrasenaRepository.EliminarToken(recuperarContrasena.id_usuario);
            string destinatario=await _recuperarContrasenaRepository.ObtenerCorreoPorID(recuperarContrasena.id_usuario);
            correo.enviarCorreoContrasena(destinatario, cod);
            recuperarContrasena.token = codigo;
            recuperarContrasena1 = await _recuperarContrasenaRepository.Codigo(recuperarContrasena);
            return recuperarContrasena;

        }
        public async Task<bool> CambiarContrasena(int id_usuario, int token, string nuevaContrasena)
        {
            // Obtener el registro de recuperación
            var recuperarContrasenaDto = await _recuperarContrasenaRepository.ObtenerRecuperacionPorUsuarioYToken(id_usuario, token);

            if (recuperarContrasenaDto != null && DateTime.Now <= recuperarContrasenaDto.fecha_expiracion)
            {
                BycriptUtility bycriptUtility = new BycriptUtility();
                nuevaContrasena = bycriptUtility.HashPassword(nuevaContrasena);
                await _usuarioRepository.ActualizarContrasena(id_usuario, nuevaContrasena);
                await _recuperarContrasenaRepository.EliminarToken(id_usuario);
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
