public class DatosUsuario
{
    private int idUsuario;
    private string nombre;
    private string usuario;
    private string contrasenia;
    private string rol;

    public DatosUsuario()
    {
    }

    public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Usuario { get => usuario; set => usuario = value; }
    public string Contrasenia { get => contrasenia; set => contrasenia = value; }
    public string Rol { get => rol; set => rol = value; }
}