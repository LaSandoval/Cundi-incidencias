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

        public async Task<RecuperarContrasenaDto> Codigo(RecuperarContrasenaDto recuperarContrasena)
        {
            recuperarContrasena.fecha_solicitud = DateTime.Now;
            recuperarContrasena.fecha_expiracion = recuperarContrasena.fecha_solicitud.AddHours(3);

            string insertQuery = @"INSERT INTO recuperar_contrasena ( id_usuario, token, fecha_solicitud, fecha_expiracion)     
                                   VALUES ( @id_usuario, @token, @fecha_solicitud, @fecha_expiracion)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {

                    cmd.Parameters.AddWithValue("@id_usuario", recuperarContrasena.id_usuario);
                    cmd.Parameters.AddWithValue("@token", recuperarContrasena.token);
                    cmd.Parameters.AddWithValue("@fecha_solicitud", recuperarContrasena.fecha_solicitud);
                    cmd.Parameters.AddWithValue("@fecha_expiracion", recuperarContrasena.fecha_expiracion);

                    await cmd.ExecuteNonQueryAsync();
                }

                await con.CloseAsync();
            }

            return recuperarContrasena;
        }
        public async Task<RecuperarContrasenaDto> ObtenerRecuperacionPorUsuarioYToken(int idUsuario, string token)
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
                    // Asigna los valores de los parámetros
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                    cmd.Parameters.AddWithValue("@token", token);

                    // Ejecuta la consulta y lee los resultados
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Llena el objeto con los valores de la fila encontrada
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
        public async Task<bool> ValidarCodigoYActualizarContrasena(int id_usuario, string token, string nuevaContrasena)
        {
            string query = @"SELECT token, fecha_expiracion FROM recuperar_contrasena 
                                 WHERE id_usuario = @id_usuario AND token = @token";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                // Primero, validamos el código y la fecha de expiración
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

                // Si la validación fue exitosa, actualizamos la contraseña
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

    }
}
