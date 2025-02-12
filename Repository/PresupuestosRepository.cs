using Microsoft.Data.Sqlite;

public class PresupuestosRepository
{
    private const string connectionString = @"Data Source=db\Tienda.db;Cache=Shared";

    public void CreatePresupuesto(Presupuestos presupuestoNuevo)
    {
        string queryString = "INSERT INTO Presupuestos (ClienteId, FechaCreacion) " +
                             "VALUES (@ClienteId, @FechaCreacion);";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@ClienteId", presupuestoNuevo.Cliente.ClienteId));
            command.Parameters.Add(new SqliteParameter("@FechaCreacion", presupuestoNuevo.FechaCreacion));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<Presupuestos> GetAllPresupuestos()
    {
        string queryString = "SELECT  p.idPresupuesto AS idPresupuesto, " +
                                    "c.ClienteId AS ClienteId, " +
                                    "c.Nombre AS Nombre, " +
                                    "c.Email AS Email, " +
                                    "c.Telefono AS Telefono, " +
                                    "p.FechaCreacion AS FechaCreacion " +
                            "FROM Presupuestos AS p " +
                            "INNER JOIN Clientes AS c ON p.ClienteId = c.ClienteId;";

        List<Presupuestos> ListaPresupuestos = new List<Presupuestos>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuestos pres = new Presupuestos();

                    pres.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    pres.Cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    pres.Cliente.Nombre = Convert.ToString(reader["Nombre"]);
                    pres.Cliente.Email = Convert.ToString(reader["Email"]);
                    pres.Cliente.Telefono = Convert.ToString(reader["Telefono"]);
                    pres.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

                    ListaPresupuestos.Add(pres);
                }
            }

            connection.Close();
        }

        return ListaPresupuestos;
    }

    public Presupuestos GetDetalleDePresupuesto(int idBuscado)
    {
        string queryString = @"SELECT p.idPresupuesto AS idPresupuesto, 
                                    c.ClienteId AS ClienteId,
                                    c.Nombre AS Nombre,
                                    c.Email AS Email,
                                    c.Telefono AS Telefono,
                                    p.FechaCreacion AS FechaCreacion,
                                    pro.idProducto AS idProducto,
                                    pre.Cantidad AS Cantidad,
                                    pro.Descripcion AS Descripcion, 
                                    pro.Precio AS Precio 
                            FROM Presupuestos AS p 
                            INNER JOIN Clientes AS c ON p.ClienteId = c.ClienteId
                            INNER JOIN PresupuestosDetalle AS pre ON p.idPresupuesto = pre.idPresupuesto 
                            INNER JOIN Productos AS pro ON pre.idProducto = pro.idProducto 
                            WHERE p.idPresupuesto = @idBuscado;";

        Presupuestos pres = new Presupuestos();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();
            
            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Productos prod = new Productos();

                    prod.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    prod.Descripcion = Convert.ToString(reader["Descripcion"]);
                    prod.Precio = Convert.ToInt32(reader["Precio"]);

                    PresupuestosDetalle prodDet = new PresupuestosDetalle();

                    int cantidad = Convert.ToInt32(reader["Cantidad"]);
                    prodDet.CargarProducto(cantidad, prod);


                    pres.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    pres.Cliente.ClienteId = Convert.ToInt32(reader["ClienteId"]);
                    pres.Cliente.Nombre = Convert.ToString(reader["Nombre"]);
                    pres.Cliente.Email = Convert.ToString(reader["Email"]);
                    pres.Cliente.Telefono = Convert.ToString(reader["Telefono"]);
                    pres.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);
                    pres.AgregarDetalle(prodDet);
                }
            }
        }

        return pres;
    }

    public void UpdatePresupuesto(int idBuscado, int idProducto, int cantidad)
    {
        string queryString = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, cantidad)
                             VALUES (@idBuscado, @idProducto, @Cantidad);";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand command = new SqliteCommand(queryString, connection);

            connection.Open();

            command.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));
            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void DeletePresupuesto(int idBuscado)
    {
        string queryStringDetalle = "DELETE FROM PresupuestosDetalle " +
                                    "WHERE idPresupuesto = @idBuscado;";

        string queryStringPresupuesto = "DELETE FROM Presupuestos " +
                                        "WHERE idPresupuesto = @idBuscado;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            SqliteCommand commandDetalle = new SqliteCommand(queryStringDetalle, connection);

            connection.Open();

            commandDetalle.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            commandDetalle.ExecuteNonQuery();


            SqliteCommand commandPresupuesto = new SqliteCommand(queryStringPresupuesto, connection);

            commandPresupuesto.Parameters.Add(new SqliteParameter("@idBuscado", idBuscado));

            commandPresupuesto.ExecuteNonQuery();

            connection.Close();
        }
    }
}