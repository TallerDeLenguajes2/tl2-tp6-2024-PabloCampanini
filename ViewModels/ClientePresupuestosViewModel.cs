public class ClientePresupuestosViewModel
{
    public List<Clientes> ListaClientes { get; set; }
    public Presupuestos NuevoPresupuesto { get; set; }

    public ClientePresupuestosViewModel()
    {
        ListaClientes = new List<Clientes>();
        NuevoPresupuesto = new Presupuestos();
    }
}
