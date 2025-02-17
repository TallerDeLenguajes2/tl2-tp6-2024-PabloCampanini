using Microsoft.Data.Sqlite;

public class DatosUsuarioRepository : IDatosUsuarioRepository
{
    private const string connectionString = @"Data Source=db\Tienda.db;Cache=Shared";

    public void CreateUsuario(DatosUsuario usuario)
    {
        string queryString = @"INSERT INTO Usuarios (Nombre, Usuario, Contraseña, Rol)
                            VALUES (@Nombre, @Usuario, @Contraseña, @Rol)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@Nombre", usuario.Nombre));
            command.Parameters.Add(new SqliteParameter("@Usuario", usuario.Usuario));
            command.Parameters.Add(new SqliteParameter("@Contraseña", usuario.Contrasenia));
            command.Parameters.Add(new SqliteParameter("@Rol", usuario.Rol));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public DatosUsuario GetUsuarioById(int idBuscado)
    {
        DatosUsuario usuario = new DatosUsuario();

        string queryString = @"SELECT * FROM Usuarios WHERE idUsuario = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                usuario.IdUsuario = Convert.ToInt32(reader["idUsuario"]);
                usuario.Nombre = Convert.ToString(reader["Nombre"]);
                usuario.Usuario = Convert.ToString(reader["Usuario"]);
                usuario.Contrasenia = Convert.ToString(reader["Contraseña"]);
                usuario.Rol = Convert.ToString(reader["Rol"]);
            }

            connection.Close();
        }

        return usuario;
    }

    public List<DatosUsuario> GetAllUsuarios()
    {
        List<DatosUsuario> ListaUsuarios = new List<DatosUsuario>();

        string queryString = @"SELECT * FROM Usuarios";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DatosUsuario usuario = new DatosUsuario();

                    usuario.IdUsuario = Convert.ToInt32(reader["idUsuario"]);
                    usuario.Nombre = Convert.ToString(reader["Nombre"]);
                    usuario.Usuario = Convert.ToString(reader["Usuario"]);
                    usuario.Contrasenia = Convert.ToString(reader["Contraseña"]);
                    usuario.Rol = Convert.ToString(reader["Rol"]);

                    ListaUsuarios.Add(usuario);
                }
            }

            connection.Close();
        }

        return ListaUsuarios;
    }

    public void UpdateDatosUsuario(int idBuscado, DatosUsuario usuario)
    {
        string queryString = @"UPDATE Usuarios
                             SET Nombre = @Nombre, 
                                 Usuario = @Usuario,
                                 Contraseña = @Contraseña,
                                 Rol = @Rol
                             WHERE idUsuario = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@Nombre", usuario.Nombre));
            command.Parameters.Add(new SqliteParameter("@Usuario", usuario.Usuario));
            command.Parameters.Add(new SqliteParameter("@Contraseña", usuario.Contrasenia));
            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void DeleteDatosUsuario(int idBuscado)
    {
        string queryString = @"DELETE FROM Usuarios WHERE idUsuario = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}