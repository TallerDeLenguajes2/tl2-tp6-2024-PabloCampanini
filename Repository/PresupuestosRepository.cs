using Microsoft.Data.Sqlite;

public class PresupuestosRepository : IPresupuestosRepository
{
    private readonly string _ConnectionString;
    public PresupuestosRepository(string ConnectionString)
    {
        _ConnectionString = ConnectionString;
    }

    public void CreatePresupuesto(Presupuestos presupuestoNuevo)
    {
        try
        {
            string queryString = "INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) " +
                                 "VALUES (@NombreDestinatario, @FechaCreacion);";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);

                connection.Open();

                command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuestoNuevo.NombreDestinatario));
                command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuestoNuevo.FechaCreacion));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al crear el presupuesto.", ex);
        }
    }

    public List<Presupuestos> GetAllPresupuestos()
    {
        try
        {
            string queryString = "SELECT * FROM Presupuestos;";
            List<Presupuestos> ListaPresupuestos = new List<Presupuestos>();

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Presupuestos pres = new Presupuestos
                        {
                            IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                            NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
                        };

                        ListaPresupuestos.Add(pres);
                    }
                }
            }

            return ListaPresupuestos;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener la lista de presupuestos.", ex);
        }
    }

    public Presupuestos GetDetalleDePresupuesto(int idBuscado)
    {
        try
        {
            string queryString = @"SELECT p.idPresupuesto, p.NombreDestinatario, p.FechaCreacion, 
                                          pre.idProducto, pre.Cantidad, 
                                          pro.Descripcion, pro.Precio 
                                   FROM Presupuestos AS p 
                                   INNER JOIN PresupuestosDetalle AS pre ON p.idPresupuesto = pre.idPresupuesto 
                                   INNER JOIN Productos AS pro ON pre.idProducto = pro.idProducto 
                                   WHERE p.idPresupuesto = @idBuscado;";

            Presupuestos pres = null;

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

                connection.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (pres == null)
                        {
                            pres = new Presupuestos
                            {
                                IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]),
                                NombreDestinatario = Convert.ToString(reader["NombreDestinatario"]),
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
                            };
                        }

                        Productos prod = new Productos
                        {
                            IdProducto = Convert.ToInt32(reader["idProducto"]),
                            Descripcion = Convert.ToString(reader["Descripcion"]),
                            Precio = Convert.ToInt32(reader["Precio"])
                        };

                        PresupuestosDetalle prodDet = new PresupuestosDetalle();
                        prodDet.CargarProducto(Convert.ToInt32(reader["Cantidad"]), prod);

                        pres.AgregarDetalle(prodDet);
                    }
                }
            }

            if (pres == null)
            {
                throw new Exception($"No se encontró el presupuesto con ID {idBuscado}");
            }

            return pres;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener los detalles del presupuesto con ID {idBuscado}.", ex);
        }
    }

    public void UpdatePresupuesto(int idBuscado, int idProducto, int cantidad)
    {
        try
        {
            string queryString = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, cantidad)
                                 VALUES (@idBuscado, @idProducto, @Cantidad);";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                SqliteCommand command = new SqliteCommand(queryString, connection);
                connection.Open();

                command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));
                command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
                command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al actualizar el presupuesto con ID {idBuscado}.", ex);
        }
    }

    public void DeletePresupuesto(int idBuscado)
    {
        try
        {
            string queryStringDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @idBuscado;";
            string queryStringPresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @idBuscado;";

            using (SqliteConnection connection = new SqliteConnection(_ConnectionString))
            {
                connection.Open();

                using (SqliteCommand commandDetalle = new SqliteCommand(queryStringDetalle, connection))
                {
                    commandDetalle.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));
                    commandDetalle.ExecuteNonQuery();
                }

                using (SqliteCommand commandPresupuesto = new SqliteCommand(queryStringPresupuesto, connection))
                {
                    commandPresupuesto.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));
                    commandPresupuesto.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al eliminar el presupuesto con ID {idBuscado}.", ex);
        }
    }
}
