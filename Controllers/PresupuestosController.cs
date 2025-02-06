using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_PabloCampanini.Models;

public class PresupuestosController : Controller
{
    private PresupuestosRepository presupuestosRep;
    private List<Presupuestos> presupuestos = new List<Presupuestos>();

    public PresupuestosController()
    {
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
}