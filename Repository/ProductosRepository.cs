using Microsoft.Data.Sqlite;

public class ProductosRepository
{
    //Direccion de la db
    private const string connectionString = @"Data Source=db\Tienda.db;Cache=Shared";

    public void CreateProducto(Productos producto)
    {
        //Query a realizar
        string queryString = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";

        //Conexion a la db
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            //Relaciono la query con la conexion
            SqliteCommand command = new SqliteCommand(queryString, connection);

            //Abro conexion
            connection.Open();

            //Cargo los datos en los parametros de esta manera para evitar SQL injection
            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));

            //Ejecuto la query
            command.ExecuteNonQuery();

            //Cierro la conexion
            connection.Close();
        }
    }
    public void UpdateProducto(int idBuscado, Productos producto)
    {
        string queryString = "UPDATE Productos " +
                             "SET Descripcion = @Descripcion " +
                             "WHERE idProducto = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<Productos> GetAllProductos()
    {
        List<Productos> ListaProductos = new List<Productos>();

        string queryString = "SELECT * FROM Productos";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Productos productoNuevo = new Productos();

                    productoNuevo.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    productoNuevo.Descripcion = Convert.ToString(reader["Descripcion"]);
                    productoNuevo.Precio = Convert.ToInt32(reader["Precio"]);

                    ListaProductos.Add(productoNuevo);
                }
            }

            connection.Close();
        }

        return ListaProductos;
    }

    public Productos GetDetalleDeProducto(int idBuscado)
    {
        Productos productoBuscado = new Productos();

        string queryString = "SELECT * FROM Productos WHERE idProducto = @idBuscado";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    productoBuscado.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    productoBuscado.Descripcion = Convert.ToString(reader["Descripcion"]);
                    productoBuscado.Precio = Convert.ToInt32(reader["Precio"]);
                }
            }

            connection.Close();
        }

        return productoBuscado;
    }

    public void DeleteProducto(int idBuscado)
    {
        string queryString = "DELETE FROM Productos WHERE idProducto = @idBuscado";

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