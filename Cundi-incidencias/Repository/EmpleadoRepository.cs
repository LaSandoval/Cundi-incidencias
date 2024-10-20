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
        public async Task<int> Asignar(int id_usuario, int id_incidencia)
        {
            int comando = 0;
            // Query para insertar en empleado_incidencia y actualizar el estado de la incidencia
            string query = @"
        BEGIN TRANSACTION;

        -- Insertar en empleado_incidencia
        INSERT INTO empleado_incidencia (id_usuario, id_incidencia) 
        VALUES (@id_usuario, @id_incidencia);

        -- Actualizar el estado de la incidencia
        UPDATE incidencia 
        SET id_estado = 2 
        WHERE id_incidencia = @id_incidencia;

        COMMIT TRANSACTION;
    ";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Agregar los parámetros
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                    cmd.Parameters.AddWithValue("@id_incidencia", id_incidencia);

                    // Ejecutar el query
                    await cmd.ExecuteNonQueryAsync();
                }
                comando = 1;
                await con.CloseAsync();
            }

            return comando;
        }
 public async Task<List<IncidenciaDto>> TraerIncidenciasAsignadas(int id_usuario)
        {
            List<IncidenciaDto> incidencias = new List<IncidenciaDto>();    
            string query= @"
        SELECT i.id_incidencia,
               i.nombre_incidencia,
               i.descripcion,
               i.imagen,
               i.fecha_inicio,
               i.fecha_fin,
               i.id_usuario,
               i.id_estado,
               i.id_categoria,
               i.id_ubicacion,
               i.fecha_creacion,
               ei.id_empleado
        FROM [Cundi_Incidencias].[dbo].[empleado_incidencia] ei
        JOIN [Cundi_Incidencias].[dbo].[incidencia] i 
            ON ei.id_incidencia = i.id_incidencia
        WHERE ei.id_usuario = @id_usuario
          AND i.id_estado = 2;
    ";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_usuario", id_usuario);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var incidencia = new IncidenciaDto
                            {
                                id_incidencia = reader.GetInt32(0),
                                nombre_incidencia = reader.GetString(1),
                                descripcion = reader.GetString(2),
                                imagen = reader.GetString(3),
                                fecha_inicio = reader.GetString(4),
                                fecha_fin = reader.GetString(5),
                                id_usuario = reader.GetInt32(6),
                                id_estado = reader.GetInt32(7),
                                id_categoria = reader.GetInt32(8),
                                id_ubicacion = reader.GetInt32(9),

                            };
                            incidencias.Add(incidencia);
                        }
                    }
                }
            }

            return incidencias;
        }
        public async Task<int> ActualizarIncidencia(int id_incidencia, string descripcion, string imagen)
        {
            int resultado = 0;

            string query = @"
        UPDATE incidencia
        SET descripcion = @descripcion,
            imagen = @imagen,
            id_estado = 3
        WHERE id_incidencia = @id_incidencia;
    ";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                   
                    cmd.Parameters.AddWithValue("@id_incidencia", id_incidencia);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@imagen", imagen);

                    resultado = await cmd.ExecuteNonQueryAsync();
                }

                await con.CloseAsync();
            }

            return resultado;  
        }

 


    }

}

