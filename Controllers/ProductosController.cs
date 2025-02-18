using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_PabloCampanini.Models;


public class ProductosController : Controller
{
    private readonly IProductosRepository productosRep;
    private readonly ILogger<ProductosController> ilogger;

    public ProductosController(IProductosRepository productosRepository, ILogger<ProductosController> logger)
    {
        productosRep = productosRepository;
        ilogger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            return View(productosRep.GetAllProductos());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CrearProducto()
    {
        try
        {
            if (HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new Productos());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult CrearProducto(Productos productoNuevo)
    {
        try
        {
            if (HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            productosRep.CreateProducto(productoNuevo);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult ModificarProducto(int idBuscado)
    {
        try
        {
            if (HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(productosRep.GetAllProductos().FirstOrDefault(prod => prod.IdProducto == idBuscado));
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ModificarProducto(Productos productoBuscado)
    {
        try
        {
            if (HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            productosRep.UpdateProducto(productoBuscado.IdProducto, productoBuscado);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EliminarProducto(int idProducto)
    {
        try
        {
            if (HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(productosRep.GetAllProductos().FirstOrDefault(prod => prod.IdProducto == idProducto));
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult EliminarProducto(Productos productoBuscado)
    {
        try
        {
            if (HttpContext.Session.GetString("rol") != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            productosRep.DeleteProducto(productoBuscado.IdProducto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}