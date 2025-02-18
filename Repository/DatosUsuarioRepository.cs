using Microsoft.Data.Sqlite;

public class DatosUsuarioRepository : IDatosUsuarioRepository
{
    private readonly string _ConnectionString;
    public DatosUsuarioRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

    public void CreateUsuario(DatosUsuario usuario)
    {
        try
        {
            string queryString = @"INSERT INTO Usuarios (Nombre, Usuario, Contraseña, Rol)
                                VALUES (@Nombre, @Usuario, @Contraseña, @Rol)";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.Parameters.Add(new SqliteParameter("@Nombre", usuario.Nombre));
                command.Parameters.Add(new SqliteParameter("@Usuario", usuario.Usuario));
                command.Parameters.Add(new SqliteParameter("@Contraseña", usuario.Contrasenia));
                command.Parameters.Add(new SqliteParameter("@Rol", usuario.Rol));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el usuario.", ex);
        }
    }

    public DatosUsuario GetUsuarioById(int idBuscado)
    {
        try
        {
            DatosUsuario usuario = null;
            string queryString = @"SELECT * FROM Usuarios WHERE idUsuario = @idBuscado";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                connection.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new DatosUsuario
                        {
                            IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            Usuario = Convert.ToString(reader["Usuario"]),
                            Contrasenia = Convert.ToString(reader["Contraseña"]),
                            Rol = Convert.ToString(reader["Rol"])
                        };
                    }
                }
            }

            if (usuario == null)
            {
                throw new Exception($"No se encontró el usuario con ID {idBuscado}.");
            }

            return usuario;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener el usuario con ID {idBuscado}.", ex);
        }
    }

    public List<DatosUsuario> GetAllUsuarios()
    {
        try
        {
            List<DatosUsuario> ListaUsuarios = new List<DatosUsuario>();
            string queryString = @"SELECT * FROM Usuarios";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DatosUsuario usuario = new DatosUsuario
                        {
                            IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                            Nombre = Convert.ToString(reader["Nombre"]),
                            Usuario = Convert.ToString(reader["Usuario"]),
                            Contrasenia = Convert.ToString(reader["Contraseña"]),
                            Rol = Convert.ToString(reader["Rol"])
                        };

                        ListaUsuarios.Add(usuario);
                    }
                }
            }

            return ListaUsuarios;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la lista de usuarios.", ex);
        }
    }

    public void UpdateDatosUsuario(int idBuscado, DatosUsuario usuario)
    {
        try
        {
            string queryString = @"UPDATE Usuarios
                                 SET Nombre = @Nombre, 
                                     Usuario = @Usuario,
                                     Contraseña = @Contraseña,
                                     Rol = @Rol
                                 WHERE idUsuario = @idBuscado";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@Nombre", usuario.Nombre));
                command.Parameters.Add(new SqliteParameter("@Usuario", usuario.Usuario));
                command.Parameters.Add(new SqliteParameter("@Contraseña", usuario.Contrasenia));
                command.Parameters.Add(new SqliteParameter("@Rol", usuario.Rol));
                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception($"No se encontró el usuario con ID {idBuscado} para actualizar.");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el usuario con ID {idBuscado}.", ex);
        }
    }

    public void DeleteDatosUsuario(int idBuscado)
    {
        try
        {
            string queryString = @"DELETE FROM Usuarios WHERE idUsuario = @idBuscado";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception($"No se encontró el usuario con ID {idBuscado} para eliminar.");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el usuario con ID {idBuscado}.", ex);
        }
    }
}
