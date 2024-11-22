using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]

public class ProductoController : Controller
{
    // private Productos producto;
    private ProductoRepository productosRep;

    public ProductoController()
    {
        productosRep = new ProductoRepository();
    }

    [HttpGet]
    public IActionResult GetProductos()
    {
        return View(productosRep.GetAllProductos());
    }
}