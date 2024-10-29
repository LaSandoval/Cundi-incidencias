using Cundi_incidencias.Dto;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Cundi_incidencias.Utility;
using Cundi_incidencias.Controllers;


namespace Cundi_incidencias.Repository
{
    public class IncidenciaRepository
    {
        private readonly string _connectionString;
        public IncidenciaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IncidenciaDto> CrearIncidencia(IncidenciaDto incidencia)
        {
            string insertQuery = @"INSERT INTO incidencia ( nombre_incidencia, descripcion, imagen, fecha_inicio, id_usuario, id_estado, id_categoria, id_ubicacion)     
                           VALUES ( @nombre_incidencia, @descripcion, @imagen, @fecha_inicio, @id_usuario, @id_estado, @id_categoria, @id_ubicacion)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand programaCmd = new SqlCommand(insertQuery, con))
                {
                    using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre_incidencia", incidencia.nombre_incidencia);
                        cmd.Parameters.AddWithValue("@descripcion", incidencia.descripcion);
                        cmd.Parameters.AddWithValue("@imagen", incidencia.imagen);
                        cmd.Parameters.AddWithValue("@fecha_inicio", incidencia.fecha_inicio);
                        cmd.Parameters.AddWithValue("@id_usuario", incidencia.id_usuario);
                        cmd.Parameters.AddWithValue("@id_estado", incidencia.id_estado);
                        cmd.Parameters.AddWithValue("@id_categoria", incidencia.id_categoria);
                        cmd.Parameters.AddWithValue("@id_ubicacion", incidencia.id_ubicacion);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    await con.CloseAsync();
                }

                return incidencia;
            }
        }
        public async Task<int> ActualizarIncidencia(int id_incidencia, string nombre_incidencia, string descripcion, string imagen, int id_categoria, int id_ubicacion)
        {
            int filasactualizadas = 0;
       //     string query = @"UPDATE incidencia  SET nombre_incidencia=@nombre_incidencia,imagen = @imagen, descripcion=@descripcion,id_categoria = @id_categoria, id_ubicacion = @id_ubicacion,  WHERE id_incidencia = @id_incidencia";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("dbo.UPD_INCIDENCIA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_incidencia", id_incidencia);
                        cmd.Parameters.AddWithValue("@nombre_incidencia", nombre_incidencia);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@imagen", imagen);
                        cmd.Parameters.AddWithValue("@id_categoria", id_categoria);
                        cmd.Parameters.AddWithValue("@id_ubicacion", id_ubicacion);
                        await cmd.ExecuteNonQueryAsync();
                        filasactualizadas = 1; 
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar la incidencia: " + ex.Message);
            }

            return filasactualizadas;
        }

        public async Task<IncidenciaDto> ObtenerIncidenciaId(int id_incidencia)
        {
            IncidenciaDto incidencia = null;
            string query = @" SELECT i.nombre_incidencia, i.descripcion, i.imagen, i.fecha_inicio, i.fecha_fin,  i.id_usuario, i.id_estado, e.nombre_estado, i.id_categoria, i.id_ubicacion
        FROM incidencia i  JOIN estado e ON i.id_estado = e.id_estado  WHERE i.id_incidencia = @id_incidencia";
            ;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_incidencia", id_incidencia);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            incidencia = new IncidenciaDto
                            {
                                nombre_incidencia = reader.GetString(0),
                                descripcion = reader.GetString(1),
                                imagen = reader.IsDBNull(2) ? null : reader.GetString(2),
                                fecha_inicio = reader.GetDateTime(3),
                                fecha_fin = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                id_usuario = reader.GetInt32(5),
                                id_estado = reader.GetInt32(6),
                                nombre_estado = reader.IsDBNull(7) ? null : reader.GetString(7),
                                id_categoria = reader.GetInt32(8),
                                id_ubicacion = reader.GetInt32(9)
                            };
                        }
                    }
                }
                await con.CloseAsync();
            }

            return incidencia;
        }

        public async Task<List<IncidenciaDto>> ObtenerHistorialIncidencia()
        {
            List<IncidenciaDto> incidencias = new List<IncidenciaDto>();
            string query = @"SELECT  i.id_incidencia, i.nombre_incidencia, i.descripcion, i.imagen, i.fecha_inicio, i.fecha_fin, i.id_usuario, 
                i.id_estado, e.nombre_estado, i.id_categoria, c.nombre_categoria AS nombre_categoria, i.id_ubicacion, u.nombre_ubicacion AS nombre_ubicacion  FROM incidencia i JOIN estado e ON i.id_estado = e.id_estado JOIN 
                categoria c ON i.id_categoria= c.id_categoria JOIN ubicacion u ON i.id_ubicacion=u.id_ubicacion";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
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
                                fecha_inicio = reader.GetDateTime(4),
                                fecha_fin = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                id_usuario = reader.GetInt32(6),
                                id_estado = reader.GetInt32(7),
                                nombre_estado = reader.IsDBNull(8) ? null : reader.GetString(8),
                                id_categoria = reader.GetInt32(9),
                                nombre_categoria= reader.GetString(10),
                                id_ubicacion = reader.GetInt32(11),
                                nombre_ubicacion= reader.GetString(12)
                            };
                            incidencias.Add(incidencia);
                        }
                    }
                }
            }
            return incidencias;
        }


    public async Task<int> EliminarIncidencia(int id_incidencia)
        {
            int filasEliminadas = 0;
           // string query = @"DELETE FROM incidencia WHERE id_incidencia = @id_incidencia";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("dbo.ELIMINAR_INCIDENCIA", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_incidencia", id_incidencia);
                        await cmd.ExecuteNonQueryAsync();
                        filasEliminadas = 1;
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar la incidencia: " + ex.Message);
            }

            return filasEliminadas;
        }

    }
}
