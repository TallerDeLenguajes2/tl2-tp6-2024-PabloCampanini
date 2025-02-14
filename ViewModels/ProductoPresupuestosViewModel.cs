public class ProductoPresupuestosViewModel
{
    public int idPresupuesto { get; set; }
    public List<Productos> ListaProductos { get; set; }

    public PresupuestosDetalle NuevoDetalle { get; set; }

    public ProductoPresupuestosViewModel()
    {
        ListaProductos = new List<Productos>();
        NuevoDetalle = new PresupuestosDetalle();
    }
}