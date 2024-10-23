using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Cundi_incidencias.Utility;
using Microsoft.Data.SqlClient;

namespace Cundi_incidencias.Repository
{
    public class PersonaRepository
    {
        private readonly string _connectionString;
        public PersonaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<bool> LoginUsuario(string correo, string contrasena)
        {
            BycriptUtility bycriptUtility = new BycriptUtility();
            string query = "SELECT contrasena FROM usuario WHERE correo = @correo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {

                                string contrasenaAlmacenada = reader["contrasena"].ToString();


                                if (bycriptUtility.VerifyPassword(contrasena, contrasenaAlmacenada))
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
                    await con.CloseAsync();
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar sesión: " + ex.Message);
                return false;
            }
        }
        public async Task<PersonaDto> TraerDatosPersona(string correo)
        {
            PersonaDto persona = new PersonaDto();
            string query = "SELECT id_usuario, id_rol FROM usuario WHERE correo=@correo";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    persona.id_usuario = reader.GetInt32(0);
                                    persona.id_rol = reader.GetInt32(1);
                                }
                            }
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de la persona", ex);
            }
            return persona;

        }
    }
}
