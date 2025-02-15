public interface IPresupuestosRepository
{
    void CreatePresupuesto(Presupuestos presupuestoNuevo);
    List<Presupuestos> GetAllPresupuestos();
    Presupuestos GetDetalleDePresupuesto(int idBuscado);
    void UpdatePresupuesto(int idBuscado, int idProducto, int cantidad);
    void DeletePresupuesto(int idBuscado);
}