public class PresupuestosViewModel
{
    public List<Clientes> ListaClientes { get; set; }
    public Presupuestos NuevoPresupuesto { get; set; }

    public PresupuestosViewModel()
    {
        ListaClientes = new List<Clientes>();
        NuevoPresupuesto = new Presupuestos();
    }
}
