public interface IDatosUsuarioRepository
{
    void CreateUsuario(DatosUsuario usuario);
    DatosUsuario GetUsuarioById(int idBuscado);
    List<DatosUsuario> GetAllUsuarios();
    void UpdateDatosUsuario(int idBuscado, DatosUsuario usuario);
    void DeleteDatosUsuario(int idBuscado);
}