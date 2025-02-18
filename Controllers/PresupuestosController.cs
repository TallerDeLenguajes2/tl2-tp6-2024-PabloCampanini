using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_PabloCampanini.Models;

public class PresupuestosController : Controller
{
    private readonly IProductosRepository productosRep;
    private readonly IPresupuestosRepository presupuestosRep;
    private readonly ILogger<PresupuestosController> ilogger;

    public PresupuestosController(IProductosRepository productosRepository, IPresupuestosRepository presupuestosRepository, ILogger<PresupuestosController> logger)
    {
        productosRep = productosRepository;
        presupuestosRep = presupuestosRepository;
        ilogger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            var presupuestos = presupuestosRep.GetAllPresupuestos();

            for (int i = 0; i < presupuestos.Count; i++)
            {
                presupuestos[i] = presupuestosRep.GetDetalleDePresupuesto(presupuestos[i].IdPresupuesto);
            }
            return View(presupuestos);
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CargarPresupuesto()
    {
        try
        {
            return View(new Presupuestos());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult CargarPresupuesto(Presupuestos nuevoPresupuesto)
    {
        try
        {
            presupuestosRep.CreatePresupuesto(nuevoPresupuesto);
            int idLast = presupuestosRep.GetAllPresupuestos().Last().IdPresupuesto;
            return RedirectToAction("CargarDetalle", new { idPresupuesto = idLast });
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CargarDetalle(int idPresupuesto)
    {
        try
        {
            ViewBag.IdPresupuesto = idPresupuesto;
            ViewBag.Productos = productosRep.GetAllProductos();
            return View(new PresupuestosDetalle());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult CargarDetalle(PresupuestosDetalle nuevoDetalle, int idPresupuesto)
    {
        try
        {
            presupuestosRep.UpdatePresupuesto(idPresupuesto, nuevoDetalle.Producto.IdProducto, nuevoDetalle.Cantidad);
            return RedirectToAction("CargarOtroDetalle", new { idPresupuesto = idPresupuesto });
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CargarOtroDetalle(int idPresupuesto)
    {
        try
        {
            ViewBag.IdPresupuesto = idPresupuesto;
            return View();
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult CargarOtroDetalle(string respuesta, int idPresupuesto)
    {
        try
        {
            if (respuesta == "Si")
            {
                return RedirectToAction("CargarDetalle", new { idPresupuesto = idPresupuesto });
            }
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(int idPresupuesto)
    {
        try
        {
            if (presupuestosRep.GetAllPresupuestos().Exists(presupuesto => presupuesto.IdPresupuesto == idPresupuesto))
            {
                return RedirectToAction("CargarDetalle", new { idPresupuesto = idPresupuesto });
            }
            return RedirectToAction("ModificarPresupuesto");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult EliminarPresupuesto(int idPresupuesto)
    {
        try
        {
            if (presupuestosRep.GetAllPresupuestos().Exists(presupuesto => presupuesto.IdPresupuesto == idPresupuesto))
            {
                presupuestosRep.DeletePresupuesto(idPresupuesto);
                return RedirectToAction("Index");
            }
            return RedirectToAction("ModificarPresupuesto");
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