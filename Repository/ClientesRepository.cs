using Microsoft.Data.Sqlite;

public class ClientesRepository
{
    private const string connectionString = @"Data Source=db\Tienda.db;Cache=Shared";

    public void CreateCliente(Clientes cliente)
    {
        string queryString = @"INSERT INTO Clientes (Nombre, Email, Telefono) 
                            VALUES (@Nombre, @Email, @Telefono)";
        
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@Nombre", cliente.Nombre));
            command.Parameters.Add(new SqliteParameter("@Email", cliente.Email));
            command.Parameters.Add(new SqliteParameter("@Telefono", cliente.Telefono));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void UpdateCliente(int idBuscado, Clientes cliente)
    {
        string queryString = @"UPDATE Clientes
                            SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono
                            WHERE ClienteId = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("Nombre", cliente.Nombre));
            command.Parameters.Add(new SqliteParameter("Email", cliente.Email));
            command.Parameters.Add(new SqliteParameter("Telefono", cliente.Telefono));
            command.Parameters.Add(new SqliteParameter("idBuscado", idBuscado));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<Clientes> GetAllClientes()
    {
        List<Clientes> ListaClientes = new List<Clientes>();

        string queryString = @"SELECT * FROM Clientes";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Clientes clienteNuevo = new Clientes();

                    clienteNuevo.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    clienteNuevo.Nombre = Convert.ToString(reader["Nombre"]);
                    clienteNuevo.Email = Convert.ToString(reader["Email"]);
                    clienteNuevo.Telefono = Convert.ToString(reader["Telefono"]);

                    ListaClientes.Add(clienteNuevo);
                }
            }

            connection.Close();
        }

        return ListaClientes;
    }

    public void DeleteCliente(int idBuscado)
    {
        string queryStringDetalle = @"DELETE FROM PresupuestosDetalles 
                                    WHERE idPresupuesto IN 
                                                        (SELECT idPresupuesto FROM Presupuestos 
                                                        WHERE ClienteId = @idBuscado);";

        string queryStringPresupuesto = @"DELETE FROM Presupuestos WHERE ClienteId = @idBuscado";

        string queryStringCliente = @"DELETE FROM Clientes WHERE ClienteId = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand commandDetalle = new SqliteCommand(queryStringDetalle, connection);

            connection.Open();

            commandDetalle.Parameters.Add(new SqliteParameter("idBuscado", idBuscado));

            commandDetalle.ExecuteNonQuery();
            

            SqliteCommand commandPresupuesto = new SqliteCommand(queryStringPresupuesto, connection);

            commandPresupuesto.Parameters.Add(new SqliteParameter("idBuscado", idBuscado));

            commandPresupuesto.ExecuteNonQuery();


            SqliteCommand commandCliente = new SqliteCommand(queryStringCliente, connection);

            commandCliente.Parameters.Add(new SqliteParameter("idBuscado", idBuscado));

            commandCliente.ExecuteNonQuery();

            connection.Close();
        }
    }
}