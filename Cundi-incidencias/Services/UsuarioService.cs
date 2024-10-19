using Cundi_incidecnias.Utility;
using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;

using System.Text;
using System.Xml.Linq;

namespace Cundi_incidencias.Services

{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<UsuarioInDto> RegistroUsuario(UsuarioInDto usuario)
        {
            UsuarioInDto usuario1=new UsuarioInDto();
            BycriptUtility bycriptUtility = new BycriptUtility();
            usuario.persona.contrasena = bycriptUtility.HashPassword(usuario.persona.contrasena);
            CodigoRecuperacionUtility tokenU=new CodigoRecuperacionUtility();
            usuario.persona.id_rol = 1;
            int token = tokenU.NumeroAleatorio();
            usuario.persona.token=token;
            usuario.persona.id_estado = 2;
            CorreoUtility correo=new CorreoUtility();
            correo.enviarCorreoCuenta(usuario.persona.correo, token);
            usuario1= await _usuarioRepository.RegistroUsuario(usuario);
            return usuario;

        }

        public async Task<int> ActivarCuenta(int token)
        {
            int filaactualizada = 0;
            filaactualizada = await _usuarioRepository.ActivarCuenta(token);
            return filaactualizada;
        }
        public async Task<bool> BuscarPersona(string correo)
        {
            if (await _usuarioRepository.BuscarPersona(correo)==false) { 
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<UsuarioInDto> EnviarDatosUsu(int id_usuario)
        {
            UsuarioInDto usuario = await _usuarioRepository.SeleccionarUsuarioAsync(id_usuario);
            return usuario;
        }
            public async Task<int> ActualizarUsuario(string correo, string programa, string semestre, string direccion, string telefono)
        {
            int filasactualizadas = 0;
            filasactualizadas = await _usuarioRepository.ActualizarUsuario (correo,programa, semestre, direccion, telefono);
            return filasactualizadas;

        }
        public async Task<int> EliminarUsuario(string correo)
        {
            int filaseliminadas = 0;
            filaseliminadas = await _usuarioRepository.EliminarUsuario(correo);
            return filaseliminadas;

        }
        public async Task<bool> IniciarSesion(string correo, string contrasena)
        {
            if(await _usuarioRepository.LoginUsuario(correo, contrasena) == true)
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }
    }
}
