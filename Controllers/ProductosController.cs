using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_PabloCampanini.Models;


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
        productosRep.CreateProducto(productoNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarProducto(int idBuscado)
    {
        return View(productos.FirstOrDefault(product => product.IdProducto == idBuscado));
    }

    [HttpPost]
    public IActionResult ModificarProducto(Productos prodModificado)
    {
        productosRep.UpdateProducto(prodModificado.IdProducto, prodModificado);
        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}