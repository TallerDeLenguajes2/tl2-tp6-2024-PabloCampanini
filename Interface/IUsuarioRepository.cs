public interface IUsuarioRepository
{
    void CreateUsuario(DatosUsuario usuario);
    DatosUsuario ObtenerUsuarioById(int idBuscado);
    List<DatosUsuario> GetAllUsuarios();
    void UpdateDatosUsuario(int idBuscado, DatosUsuario usuario);
    void DeleteDatosUsuario(int idBuscado);
}