using Microsoft.AspNetCore.Mvc;

public class DatosUsuarioController : Controller
{
    IDatosUsuarioRepository usuarioRep;

    public DatosUsuarioController(IDatosUsuarioRepository usuarioRepository)
    {
        usuarioRep = usuarioRepository;
    }

    public IActionResult Index()
    {
        return View(new UsuarioViewModel());
    }

    [HttpPost]
    public IActionResult Login(UsuarioViewModel usuarioCargado)
    {
        var usuario = usuarioRep.GetAllUsuarios().FirstOrDefault(u => u.Usuario == usuarioCargado.Usuario && u.Contrasenia == usuarioCargado.Contrasenia);

        if (usuario == null)
        {
            ViewBag.Error = "Debe ingresar datos validos";

            return View(usuarioCargado);
        }

        HttpContext.Session.SetInt32("idIngresado", usuario.IdUsuario);
        HttpContext.Session.SetString("rol", usuario.Rol);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Borra los datos de la sesión
        return RedirectToAction("Index", "DatosUsuario");
    }
}