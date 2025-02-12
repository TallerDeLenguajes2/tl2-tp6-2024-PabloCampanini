public class Presupuestos
{
    private int idPresupuesto;
    private Clientes cliente;
    private DateTime fechaCreacion;
    List<PresupuestosDetalle> detalle;
    public Presupuestos()
    {
        detalle = new List<PresupuestosDetalle>();
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public Clientes Cliente { get => cliente; set => cliente = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestosDetalle> Detalle { get => detalle; }

    public void AgregarDetalle(PresupuestosDetalle nuevoDetalle)
    {
        detalle.Add(nuevoDetalle);
        
    }
    public double MontoPresupuesto()
    {
        double monto = 0;

        foreach (var producto in Detalle)
        {
            monto += (producto.Cantidad * producto.Producto.Precio);    
        }

        return monto;
    }

    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21; //IVA = 21%
    }

    public int CantidadProductos()
    {
        int cantidad = 0;

        foreach (var producto in Detalle)
        {
            cantidad += producto.Cantidad;
        }
        
        return cantidad;
    }
}