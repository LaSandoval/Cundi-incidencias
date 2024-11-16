using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;

namespace Cundi_incidencias.Services
{
    public class PersonaService
    {
        private readonly PersonaRepository _personaRepository;

        public PersonaService(PersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }
        public async Task<bool> IniciarSesion(string correo, string contrasena)
        {
            if (await _personaRepository.LoginUsuario(correo, contrasena) == true)
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        public async Task <PersonaDto> TraerDatosPersona(string correo)
        {
            PersonaDto persona=new PersonaDto();
            persona=await _personaRepository.TraerDatosPersona(correo);
            return persona;
        }
    }
}
