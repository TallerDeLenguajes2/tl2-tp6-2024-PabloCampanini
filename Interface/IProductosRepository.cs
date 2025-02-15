public interface IProductosRepository
{
    void CreateProducto(Productos producto);
    void UpdateProducto(int idBuscado, Productos producto);
    List<Productos> GetAllProductos();
    Productos GetDetalleDeProducto(int idBuscado);
    void DeleteProducto(int idBuscado);
}