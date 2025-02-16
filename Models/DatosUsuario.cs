public class DatosUsuario
{
    private int idUsuario;
    private string nombre;
    private string usuario;
    private string contraseña;
    private string rol;

    public DatosUsuario()
    {
    }

    public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Usuario { get => usuario; set => usuario = value; }
    public string Contraseña { get => contraseña; set => contraseña = value; }
    public string Rol { get => rol; set => rol = value; }
}