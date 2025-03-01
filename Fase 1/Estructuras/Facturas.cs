
using System;
using System.Text;
using System.Runtime.InteropServices;
public unsafe struct NodoFactura
{
    public int Id;
    public int IdOrden;
    public double Total;
    public NodoFactura* Siguiente;

    public NodoFactura(int id, int idOrden, double total)
    {
        Id = id;
        IdOrden = idOrden;
        Total = total;
        Siguiente = null;
    }
}