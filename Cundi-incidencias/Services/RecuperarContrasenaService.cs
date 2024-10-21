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

        public async Task<int> Codigo(string correoU)
        {
            RecuperarContrasenaDto recuperarContrasena=new RecuperarContrasenaDto();
            CodigoRecuperacionUtility token = new CodigoRecuperacionUtility();
            CorreoUtility correo = new CorreoUtility();
            int id_usuario= await _recuperarContrasenaRepository.BuscarId(correoU);
            int codigo = token.NumeroAleatorio();
            string cod = codigo.ToString();
            await _recuperarContrasenaRepository.EliminarToken(id_usuario);
            string destinatario=await _recuperarContrasenaRepository.ObtenerCorreoPorID(id_usuario);
            correo.enviarCorreoContrasena(destinatario, cod);
            recuperarContrasena.token = codigo;
            int filas= await _recuperarContrasenaRepository.Codigo(id_usuario, codigo);
            return filas ;
        }
        public async Task<bool> CambiarContrasena( int token, string nuevaContrasena)
        {
            int id_usuario = await _recuperarContrasenaRepository.BuscarIdToken(token);
            var recuperarContrasenaDto = await _recuperarContrasenaRepository.ActualizarContrasena(id_usuario, token);

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
