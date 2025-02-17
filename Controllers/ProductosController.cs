using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_PabloCampanini.Models;


public class ProductosController : Controller
{
    private readonly IProductosRepository productosRep;

    public ProductosController(IProductosRepository productosRepository)
    {
        productosRep = productosRepository;
    }

    public IActionResult Index()
    {
        return View(productosRep.GetAllProductos());
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        if (HttpContext.Session.GetString("rol") != "admin")
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new Productos());
    }

    [HttpPost]
    public IActionResult CrearProducto(Productos productoNuevo)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
        {
            return RedirectToAction("Index", "Home");
        }
        productosRep.CreateProducto(productoNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarProducto(int idBuscado)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
        {
            return RedirectToAction("Index", "Home");
        }
        return View(productosRep.GetAllProductos().FirstOrDefault(prod => prod.IdProducto == idBuscado));
    }

    [HttpPost]
    public IActionResult ModificarProducto(Productos productoBuscado)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
        {
            return RedirectToAction("Index", "Home");
        }
        productosRep.UpdateProducto(productoBuscado.IdProducto, productoBuscado);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarProducto(int idProducto)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
        {
            return RedirectToAction("Index", "Home");
        }
        return View(productosRep.GetAllProductos().FirstOrDefault(prod => prod.IdProducto == idProducto));
    }

    [HttpPost]
    public IActionResult EliminarProducto(Productos productoBuscado)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
        {
            return RedirectToAction("Index", "Home");
        }
        productosRep.DeleteProducto(productoBuscado.IdProducto);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}