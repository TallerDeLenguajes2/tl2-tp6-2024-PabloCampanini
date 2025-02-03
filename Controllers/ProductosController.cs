using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


public class ProductosController : Controller
{
    private ProductosRepository productosRep;
    private List<Productos> productos = new List<Productos>();

    public ProductosController()
    {
        productosRep = new ProductosRepository();
        productos = productosRep.GetAllProductos();
    }

    public IActionResult Index()
    {
        return View(productos);
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        return View(new Productos());
    }

    [HttpPost]
    public IActionResult CrearProducto(Productos productoNuevo)
    {
        productoNuevo.IdProducto = productos.Count() + 1;
        productos.Add(productoNuevo);
        return RedirectToAction("Index");
    }
}