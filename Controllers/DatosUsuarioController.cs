using Microsoft.AspNetCore.Mvc;

public class DatosUsuarioController : Controller
{
    private readonly IDatosUsuarioRepository usuarioRep;
    private readonly ILogger<DatosUsuarioController> ilogger;

    public DatosUsuarioController(IDatosUsuarioRepository usuarioRepository, ILogger<DatosUsuarioController> logger)
    {
        usuarioRep = usuarioRepository;
        ilogger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            return View(new UsuarioViewModel());
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al cargar la vista de inicio de sesión.");
            return BadRequest();
        }
    }

    [HttpPost]
    public IActionResult Login(UsuarioViewModel usuarioCargado)
    {
        try
        {
            var usuario = usuarioRep.GetAllUsuarios().FirstOrDefault(u => u.Usuario == usuarioCargado.Usuario && u.Contrasenia == usuarioCargado.Contrasenia);

            if (usuario == null)
            {
                string accesoRechazado = "Intento de acceso invalido - Usuario: " + usuarioCargado.Usuario + " - Clave: " + usuarioCargado.Contrasenia;

                Console.WriteLine(accesoRechazado);
                ilogger.LogWarning(accesoRechazado);

                return View(usuarioCargado);
            }

            string accesoExitoso = "El usuario " + usuario.Nombre + " ingreso correctamente";

            Console.WriteLine(accesoExitoso);
            ilogger.LogInformation(accesoExitoso);

            HttpContext.Session.SetInt32("idIngresado", usuario.IdUsuario);
            HttpContext.Session.SetString("rol", usuario.Rol);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al intentar iniciar sesión.");
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        try
        {
            HttpContext.Session.Clear(); // Borra los datos de la sesión

            string mensajeLogout = "Sesión cerrada correctamente.";
            Console.WriteLine(mensajeLogout);
            ilogger.LogInformation(mensajeLogout);

            return RedirectToAction("Index", "DatosUsuario");
        }
        catch (Exception ex)
        {
            ilogger.LogError(ex, "Error al cerrar sesión.");
            return BadRequest();
        }
    }
}
