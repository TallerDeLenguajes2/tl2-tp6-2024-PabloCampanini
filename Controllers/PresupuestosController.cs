using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_PabloCampanini.Models;

public class PresupuestosController : Controller
{
    private ClientesRepository clientesRep;
    private ProductosRepository productosRep;
    private PresupuestosRepository presupuestosRep;
    private List<Presupuestos> presupuestos = new List<Presupuestos>();

    public PresupuestosController()
    {
        clientesRep = new ClientesRepository();
        productosRep = new ProductosRepository();
        presupuestosRep = new PresupuestosRepository();
        presupuestos = presupuestosRep.GetAllPresupuestos();
    }

    public IActionResult Index()
    {
        for (int i = 0; i < presupuestos.Count; i++)
        {
            presupuestos[i] = presupuestosRep.GetDetalleDePresupuesto(presupuestos[i].IdPresupuesto);
        }
        return View(presupuestos);
    }

    [HttpGet]
    public IActionResult CargarPresupuesto()
    {
        ViewBag.Clientes = clientesRep.GetAllClientes();
        return View(new Presupuestos());
    }

    [HttpPost]
    public IActionResult CargarPresupuesto(Presupuestos nuevoPresupuesto)
    {
        presupuestosRep.CreatePresupuesto(nuevoPresupuesto);
        int idLast = presupuestosRep.GetAllPresupuestos().Last().IdPresupuesto;
        return RedirectToAction("CargarDetalle", new { idPresupuesto = idLast });
    }

    [HttpGet]
    public IActionResult CargarDetalle(int idPresupuesto)
    {
        ViewBag.IdPresupuesto = idPresupuesto;
        ViewBag.Productos = productosRep.GetAllProductos();
        return View(new PresupuestosDetalle());
    }

    [HttpPost]
    public IActionResult CargarDetalle(PresupuestosDetalle nuevoDetalle, int idPresupuesto)
    {
        presupuestosRep.UpdatePresupuesto(idPresupuesto, nuevoDetalle.Producto.IdProducto, nuevoDetalle.Cantidad);
        return RedirectToAction("CargarOtroDetalle", new { idPresupuesto = idPresupuesto });
    }

    [HttpGet]
    public IActionResult CargarOtroDetalle(int idPresupuesto)
    {
        ViewBag.IdPresupuesto = idPresupuesto;
        return View();
    }

    [HttpPost]
    public IActionResult CargarOtroDetalle(string respuesta, int idPresupuesto)
    {
        if (respuesta == "Si")
        {
            return RedirectToAction("CargarDetalle", new { idPresupuesto = idPresupuesto });
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(int idPresupuesto)
    {
        if (presupuestosRep.GetAllPresupuestos().Exists(presupuesto => presupuesto.IdPresupuesto == idPresupuesto))
        {
            return RedirectToAction("CargarDetalle", new {idPresupuesto = idPresupuesto});
        }
        return RedirectToAction("ModificarPresupuesto");
    }

    [HttpGet]
    public IActionResult EliminarPresupuesto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult EliminarPresupuesto(int idPresupuesto)
    {
        if (presupuestosRep.GetAllPresupuestos().Exists(presupuesto => presupuesto.IdPresupuesto == idPresupuesto))
        {
            presupuestosRep.DeletePresupuesto(idPresupuesto);
            return RedirectToAction("Index");
        }
        return RedirectToAction("ModificarPresupuesto");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}