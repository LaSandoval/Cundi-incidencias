using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Cundi_incidencias.Utility;
using System.Text;

namespace Cundi_incidencias.Services
{
    public class PersonaService
    {
        private readonly PersonaRepository _personaRepository;

        public PersonaService(PersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<bool> IniciarSesion(string correo, string contrasenaEnBinario)
        {
            // Convertir la contraseña de binario a texto plano
            string contrasenaTextoPlano = BinaryToString(contrasenaEnBinario);

            // Encriptar la contraseña y verificar con la almacenada (bcrypt)
            if (await _personaRepository.LoginUsuario(correo, contrasenaTextoPlano))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PersonaDto> TraerDatosPersona(string correo)
        {
            return await _personaRepository.TraerDatosPersona(correo);
        }

        // Método para convertir de binario a texto plano
        private string BinaryToString(string binary)
        {
            var text = binary.Split(' ')
                             .Select(bin => Convert.ToChar(Convert.ToInt32(bin, 2)))
                             .ToArray();
            return new string(text);
        }
    }
}
