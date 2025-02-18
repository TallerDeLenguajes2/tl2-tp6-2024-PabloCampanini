using Microsoft.Data.Sqlite;

public class ProductosRepository : IProductosRepository
{
    private readonly string _ConnectionString;
    public ProductosRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }
    
    public void CreateProducto(Productos producto)
    {
        try
        {
            string queryString = "INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";
            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
                command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el producto.", ex);
        }
    }

    public void UpdateProducto(int idBuscado, Productos producto)
    {
        try
        {
            string queryString = "UPDATE Productos " +
                                 "SET Descripcion = @Descripcion, Precio = @Precio " +
                                 "WHERE idProducto = @idBuscado";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
                command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("El producto no existe en la base de datos.");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el producto.", ex);
        }
    }

    public List<Productos> GetAllProductos()
    {
        try
        {
            List<Productos> ListaProductos = new List<Productos>();

            string queryString = "SELECT * FROM Productos";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Productos productoNuevo = new Productos
                        {
                            IdProducto = Convert.ToInt32(reader["idProducto"]),
                            Descripcion = Convert.ToString(reader["Descripcion"]),
                            Precio = Convert.ToInt32(reader["Precio"])
                        };

                        ListaProductos.Add(productoNuevo);
                    }
                }
            }

            if (ListaProductos.Count == 0)
            {
                throw new Exception("No hay productos en la base de datos.");
            }

            return ListaProductos;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los productos.", ex);
        }
    }

    public Productos GetDetalleDeProducto(int idBuscado)
    {
        try
        {
            Productos productoBuscado = null;

            string queryString = "SELECT * FROM Productos WHERE idProducto = @idBuscado";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productoBuscado = new Productos
                        {
                            IdProducto = Convert.ToInt32(reader["idProducto"]),
                            Descripcion = Convert.ToString(reader["Descripcion"]),
                            Precio = Convert.ToInt32(reader["Precio"])
                        };
                    }
                }
            }

            if (productoBuscado == null)
            {
                throw new Exception("El producto no existe en la base de datos.");
            }

            return productoBuscado;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el detalle del producto.", ex);
        }
    }

    public void DeleteProducto(int idBuscado)
    {
        try
        {
            string queryString = "DELETE FROM Productos WHERE idProducto = @idBuscado";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("El producto no existe en la base de datos.");
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el producto.", ex);
        }
    }
}
