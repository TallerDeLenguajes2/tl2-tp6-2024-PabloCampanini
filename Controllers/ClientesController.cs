using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller
{
    private ClientesRepository clientesRep;

    public ClientesController()
    {
        clientesRep = new ClientesRepository();
    }

    public IActionResult Index()
    {
        return View(clientesRep.GetAllClientes());
    }

    [HttpGet]
    public IActionResult CargarCliente()
    {
        return View(new Clientes());
    }

    [HttpPost]
    public IActionResult CargarCliente(Clientes clienteNuevo)
    {
        if (!ModelState.IsValid)
        {
            return View(clienteNuevo);
        }

        clientesRep.CreateCliente(clienteNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarCliente(int ClienteId)
    {var cliente = clientesRep.GetAllClientes().FirstOrDefault(cliente => cliente.ClienteId == ClienteId);

        return View(cliente);
    }

    [HttpPost]
    public IActionResult ModificarCliente(Clientes clienteModificar)
    {
        if (!ModelState.IsValid)
        {
            return View(clienteModificar);
        }

        clientesRep.UpdateCliente(clienteModificar.ClienteId, clienteModificar);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult EliminarCliente(int ClienteId)
    {
        return View(clientesRep.GetAllClientes().FirstOrDefault(cliente => cliente.ClienteId == ClienteId));
    }

    [HttpPost]
    public IActionResult EliminarCliente(Clientes clienteBorrar)
    {
        clientesRep.DeleteCliente(clienteBorrar.ClienteId);
        return RedirectToAction("Index");
    }
}