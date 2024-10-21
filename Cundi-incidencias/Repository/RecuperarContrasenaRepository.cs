using Cundi_incidencias.Dto;
using Cundi_incidencias.Repository;
using Microsoft.Data.SqlClient;

namespace Cundi_incidencias.Repository
{
    public class RecuperarContrasenaRepository
    {
        private readonly string _connectionString;
        public RecuperarContrasenaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<int> BuscarId(string correo)
        {
            string query = "SELECT id_usuario FROM usuario WHERE correo = @correo";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);

                    var id_usuario = await cmd.ExecuteScalarAsync();

                    if (id_usuario == null)
                    {
                        throw new Exception("No se encontró ningún usuario con el correo proporcionado.");
                    }
                  return Convert.ToInt32(id_usuario);
                }
            }
        }


        public async Task<int> Codigo(int id_usuario, int token)
        {
            int filas = 0;
            DateTime fecha_solicitud = DateTime.Now;
            DateTime fecha_expiracion = fecha_solicitud.AddMinutes(10);

            string insertQuery = @"INSERT INTO recuperar_contrasena ( id_usuario, token, fecha_solicitud, fecha_expiracion)     
                                   VALUES ( @id_usuario, @token, @fecha_solicitud, @fecha_expiracion)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@fecha_solicitud", fecha_solicitud);
                    cmd.Parameters.AddWithValue("@fecha_expiracion", fecha_expiracion);

                    await cmd.ExecuteNonQueryAsync();
                }
                filas = 1;
                await con.CloseAsync();
            }

            return filas;
        }
        public async Task<int> BuscarIdToken(int token)
        {
            string query = "SELECT id_usuario FROM recuperar_contrasena WHERE token = @token";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@token", token);

                    var id_usuario = await cmd.ExecuteScalarAsync();

                    if (id_usuario == null)
                    {
                        throw new Exception("No se encontró ningún usuario con el token proporcionado.");
                    }
                    return Convert.ToInt32(id_usuario);
                }
            }
        }

        public async Task<RecuperarContrasenaDto> ActualizarContrasena(int id_usuario, int token)
        {
            string query = @"SELECT id_usuario, token, fecha_solicitud, fecha_expiracion 
                     FROM recuperar_contrasena 
                     WHERE id_usuario = @id_usuario AND token = @token";
            RecuperarContrasenaDto recuperarContrasena = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                    cmd.Parameters.AddWithValue("@token", token);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            recuperarContrasena = new RecuperarContrasenaDto
                            {
                                id_usuario = (int)reader["id_usuario"],
                                token = (int)reader["token"],
                                fecha_solicitud = (DateTime)reader["fecha_solicitud"],
                                fecha_expiracion = (DateTime)reader["fecha_expiracion"]
                            };
                        }
                    }
                }
                await con.CloseAsync();
            }

            return recuperarContrasena;
        }

        public async Task<string> ObtenerCorreoPorID(int id)
        {

            string correo = null;

            string query = @"SELECT correo FROM usuario WHERE id_usuario = @id_usuario";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", id);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {


                            correo = (string)reader["correo"];
                                  
                        }
                    }
                }
                await con.CloseAsync();
            }

            return correo;
        }
        public async Task<bool> ActualizarContrasena(int id_usuario, string token, string nuevaContrasena)
        {
            string query = @"SELECT token, fecha_expiracion FROM recuperar_contrasena 
                                 WHERE id_usuario = @id_usuario AND token = @token";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                    cmd.Parameters.AddWithValue("@token", token);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            DateTime fechaExpiracion = (DateTime)reader["fecha_expiracion"];

                            if (fechaExpiracion < DateTime.Now)
                                return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                string queryAct = @"UPDATE usuario SET contrasena = @nuevaContrasena 
                                             WHERE id_usuario = @id_usuario";

                using (SqlCommand cmd = new SqlCommand(queryAct, con))
                {
                    string hashedContrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);

                    cmd.Parameters.AddWithValue("@nuevaContrasena", hashedContrasena);
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);

                    await cmd.ExecuteNonQueryAsync();
                }

                await con.CloseAsync();
            }

            return true; 
        }
        public async Task<int> EliminarToken(int id_usuario)
        {
            int filasEliminadas = 0;
            string query = @"DELETE FROM recuperar_contrasena WHERE id_usuario = @id_usuario";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_usuario);

                        filasEliminadas = await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el token: " + ex.Message);
            }

            return filasEliminadas;
        }

    }
}
