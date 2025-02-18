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

            ilogger.LogInformation("Se accedió a la lista de presupuestos.");
            return View(presupuestos);
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al obtener la lista de presupuestos.");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CargarPresupuesto()
    {
        try
        {
            ilogger.LogInformation("Vista de carga de presupuesto mostrada.");
            return View(new Presupuestos());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al cargar la vista de presupuestos.");
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

            ilogger.LogInformation($"Presupuesto creado con ID {idLast}.");
            return RedirectToAction("CargarDetalle", new { idPresupuesto = idLast });
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al crear un nuevo presupuesto.");
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

            ilogger.LogInformation($"Vista de carga de detalle para presupuesto ID {idPresupuesto} mostrada.");
            return View(new PresupuestosDetalle());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, $"Error al cargar la vista de detalle del presupuesto ID {idPresupuesto}.");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult CargarDetalle(PresupuestosDetalle nuevoDetalle, int idPresupuesto)
    {
        try
        {
            presupuestosRep.UpdatePresupuesto(idPresupuesto, nuevoDetalle.Producto.IdProducto, nuevoDetalle.Cantidad);

            ilogger.LogInformation($"Detalle agregado al presupuesto ID {idPresupuesto}. Producto ID: {nuevoDetalle.Producto.IdProducto}, Cantidad: {nuevoDetalle.Cantidad}.");
            return RedirectToAction("CargarOtroDetalle", new { idPresupuesto = idPresupuesto });
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, $"Error al agregar un detalle al presupuesto ID {idPresupuesto}.");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult CargarOtroDetalle(int idPresupuesto)
    {
        try
        {
            ViewBag.IdPresupuesto = idPresupuesto;

            ilogger.LogInformation($"Vista de opción para cargar otro detalle en presupuesto ID {idPresupuesto} mostrada.");
            return View();
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, $"Error al mostrar la opción de agregar otro detalle en el presupuesto ID {idPresupuesto}.");
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
                ilogger.LogInformation($"Usuario eligió agregar otro detalle al presupuesto ID {idPresupuesto}.");
                return RedirectToAction("CargarDetalle", new { idPresupuesto = idPresupuesto });
            }

            ilogger.LogInformation($"Usuario finalizó la carga de detalles para el presupuesto ID {idPresupuesto}.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, $"Error en la selección de cargar otro detalle en presupuesto ID {idPresupuesto}.");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto()
    {
        try
        {
            ilogger.LogInformation("Vista de modificación de presupuesto mostrada.");
            return View();
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al mostrar la vista de modificación de presupuesto.");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(int idPresupuesto)
    {
        try
        {
            if (presupuestosRep.GetAllPresupuestos().Exists(p => p.IdPresupuesto == idPresupuesto))
            {
                ilogger.LogInformation($"Se accedió a la modificación del presupuesto ID {idPresupuesto}.");
                return RedirectToAction("CargarDetalle", new { idPresupuesto = idPresupuesto });
            }

            ilogger.LogWarning($"Intento de modificar un presupuesto inexistente con ID {idPresupuesto}.");
            return RedirectToAction("ModificarPresupuesto");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, $"Error al modificar el presupuesto ID {idPresupuesto}.");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto()
    {
        try
        {
            ilogger.LogInformation("Vista de eliminación de presupuesto mostrada.");
            return View();
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al mostrar la vista de eliminación de presupuesto.");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult EliminarPresupuesto(int idPresupuesto)
    {
        try
        {
            if (presupuestosRep.GetAllPresupuestos().Exists(p => p.IdPresupuesto == idPresupuesto))
            {
                presupuestosRep.DeletePresupuesto(idPresupuesto);
                ilogger.LogInformation($"Presupuesto con ID {idPresupuesto} eliminado correctamente.");
                return RedirectToAction("Index");
            }

            ilogger.LogWarning($"Intento de eliminar un presupuesto inexistente con ID {idPresupuesto}.");
            return RedirectToAction("ModificarPresupuesto");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, $"Error al eliminar el presupuesto ID {idPresupuesto}.");
            return BadRequest();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
