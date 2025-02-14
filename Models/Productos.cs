using System.ComponentModel.DataAnnotations;

public class Productos
{
    private int idProducto;
    private string descripcion;
    private double precio;

    public Productos()
    {
        
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }

    [StringLength(250)]
    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Required] [Range(0.01, double.MaxValue)]
    public double Precio { get => precio; set => precio = value; }
}