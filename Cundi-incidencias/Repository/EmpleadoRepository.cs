using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;

namespace Cundi_incidencias.Repository
{
    public class EmpleadoRepository
    {
        private readonly string _connectionString;
        public EmpleadoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<EmpleadoDto> CrearEmpleado(EmpleadoDto empleado)
        {
            empleado.persona.fecha_registro = DateTime.Now.Date;
            string insertQuery = @"INSERT INTO usuario (id_rol, nombre_usuario, apellido_usuario, correo, contrasena, direccion, fecha_registro, telefono, id_cargo, id_turno, id_estado)     
                           VALUES (@id_rol, @nombre_usuario, @apellido_usuario, @correo, @contrasena, @direccion, @fecha_registro, @telefono, @id_cargo, @id_turno, @id_estado)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@id_rol", empleado.persona.id_rol);
                    cmd.Parameters.AddWithValue("@nombre_usuario", empleado.persona.nombre);
                    cmd.Parameters.AddWithValue("@apellido_usuario", empleado.persona.apellido);
                    cmd.Parameters.AddWithValue("@correo", empleado.persona.correo); 
                    cmd.Parameters.AddWithValue("@contrasena", empleado.persona.contrasena);
                    cmd.Parameters.AddWithValue("@direccion", empleado.persona.direccion);
                    cmd.Parameters.AddWithValue("@fecha_registro", empleado.persona.fecha_registro);
                    cmd.Parameters.AddWithValue("@telefono", empleado.persona.telefono);
                    cmd.Parameters.AddWithValue("@id_estado", empleado.persona.id_estado);
                    cmd.Parameters.AddWithValue("@id_cargo", empleado.id_cargo);
                    cmd.Parameters.AddWithValue("@id_turno", empleado.id_turno);

                    await cmd.ExecuteNonQueryAsync();
                }

                await con.CloseAsync();
            }

            return empleado;
        }
        public async Task<int> ActualizarEmpleado(int id_usuario, string direccion, string telefono)
        {
            int filasactualizadas = 0;
            string query = @"UPDATE usuario 
                     SET  direccion = @direccion, 
                         telefono = @telefono 
                     WHERE id_usuario = @id_usuario";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                        cmd.Parameters.AddWithValue("@direccion", direccion);
                        cmd.Parameters.AddWithValue("@telefono", telefono);

                        filasactualizadas = await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el Empleado: " + ex.Message);
            }

            return filasactualizadas;
        }
        public async Task<int> EliminarEmpleado(int id_usuario)
        {
            int filasEliminadas = 0;
            string query = @"DELETE FROM usuario WHERE id_usuario = @id_usuario";

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
                Console.WriteLine("Error al eliminar el empleado: " + ex.Message);
            }

            return filasEliminadas;
        }
        public async Task<List<EmpleadoDto>> MostrarEmpleado()
        {
            List<EmpleadoDto> listEmpleados = new List<EmpleadoDto>();
            string sql = "SELECT id_usuario,nombre_usuario, apellido_usuario, direccion, telefono, id_cargo, id_turno FROM usuario WHERE id_rol=2";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PersonaDto persona;
                                persona = new PersonaDto
                                {
                                    id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                    nombre = reader["nombre_usuario"].ToString(),
                                    apellido = reader["apellido_usuario"].ToString(),
                                    direccion = reader["direccion"].ToString(),
                                    telefono = reader["telefono"].ToString()
                                };
                                EmpleadoDto empleado = new EmpleadoDto
                                {
                                    id_cargo = Convert.ToInt32(reader["id_cargo"]),
                                    id_turno = Convert.ToInt32(reader["id_turno"])
                                };

                                empleado.persona = persona;
                                listEmpleados.Add(empleado);
                            }
                        }

                        await con.CloseAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar empleado", ex);
            }
            return listEmpleados;
        }

    }


}
