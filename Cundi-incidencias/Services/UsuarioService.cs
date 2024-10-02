using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;

using System.Text;

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

            usuario1= await _usuarioRepository.RegistroUsuario(usuario);
            return usuario;

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
