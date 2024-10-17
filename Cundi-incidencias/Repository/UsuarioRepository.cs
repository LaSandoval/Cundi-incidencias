
using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Cundi_incidencias.Utility;
using Microsoft.Data.SqlClient;

namespace Cundi_incidencias.Repository
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<UsuarioInDto> RegistroUsuario(UsuarioInDto usuario)
        {
            usuario.persona.fecha_registro=DateTime.Now.Date;
            string insertQuery = @"INSERT INTO usuario (id_rol, nombre_usuario, apellido_usuario, correo, contrasena, id_programa, id_semestre, direccion, fecha_registro, telefono)     
                           VALUES (@id_rol, @nombre_usuario, @apellido_usuario, @correo, @contrasena, @id_programa, @id_semestre, @direccion, @fecha_registro,  @telefono)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand programaCmd = new SqlCommand(insertQuery, con))
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id_rol", usuario.persona.id_rol);
                        cmd.Parameters.AddWithValue("@nombre_usuario", usuario.persona.nombre);
                        cmd.Parameters.AddWithValue("@apellido_usuario", usuario.persona.apellido);
                        cmd.Parameters.AddWithValue("@correo", usuario.persona.correo);
                        cmd.Parameters.AddWithValue("@contrasena", usuario.persona.contrasena);
                        cmd.Parameters.AddWithValue("@id_programa", usuario.id_programa);
                        cmd.Parameters.AddWithValue("@id_semestre", usuario.id_semestre);
                        cmd.Parameters.AddWithValue("@direccion", usuario.persona.direccion);
                        cmd.Parameters.AddWithValue("@fecha_registro", usuario.persona.fecha_registro);
                        cmd.Parameters.AddWithValue("@telefono", usuario.persona.telefono);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    await con.CloseAsync();
                }

                return usuario;
            }
        }

        public async Task<bool> BuscarPersona(string correo)
        {
            string query = "SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";
            int personaEncontrado = 0;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);
                    personaEncontrado = (int)cmd.ExecuteScalar();
                }
                await con.CloseAsync();
                if (personaEncontrado > 0)
                {
                    return true;
                }
                else
                {
                    return false;

                }
            }
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



        public async Task<UsuarioInDto> ObtenerUsuarioPorCorreo(string correo)
        {
            UsuarioInDto usuario = null;

            string query = @"SELECT * FROM usuario WHERE correo = @correo";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            usuario = new UsuarioInDto
                            {
                                persona = new PersonaDto
                                {
                                    id_rol = (int)reader["id_rol"],
                                    nombre = (string)reader["nombre_usuario"],
                                    apellido = (string)reader["apellido_usuario"],
                                    correo = (string)reader["correo"],
                                    contrasena = (string)reader["contrasena"],
                                    direccion = (string)reader["direccion"],
                                    telefono = (string)reader["telefono"],
                                    fecha_registro = (DateTime)reader["fecha_registro"]
                                },
                                id_programa = (int)reader["id_programa"],
                                id_semestre = (int)reader["id_semestre"]
                            };
                        }
                    }
                }
                await con.CloseAsync();
            }

            return usuario;
        }
        public async Task<int> ActualizarUsuario(string correo, string programa, string semestre, string direccion, string telefono)
        {
            int filasactualizadas = 0;
            string query = @"UPDATE usuario 
                     SET programa = @programa, 
                         semestre = @semestre, 
                         direccion = @direccion, 
                         telefono = @telefono 
                     WHERE correo = @correo";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@programa", programa);
                        cmd.Parameters.AddWithValue("@semestre", semestre);
                        cmd.Parameters.AddWithValue("@direccion", direccion);
                        cmd.Parameters.AddWithValue("@telefono", telefono);

                        filasactualizadas = await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el usuario: " + ex.Message);
            }

            return filasactualizadas;
        }
        public async Task<int> EliminarUsuario(string correo)
        {
            int filasEliminadas = 0;
            string query = @"DELETE FROM usuario WHERE correo = @correo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);

                        filasEliminadas = await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el usuario: " + ex.Message);
            }

            return filasEliminadas;
        }
        public async Task ActualizarContrasena(int idUsuario, string nuevaContrasena)
        {
            string query = @"UPDATE usuario SET contrasena = @nuevaContrasena WHERE id_usuario = @id_usuario";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@nuevaContrasena", nuevaContrasena);
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    await cmd.ExecuteNonQueryAsync();
                }
                await con.CloseAsync() ;
            }
        }



    }
}

