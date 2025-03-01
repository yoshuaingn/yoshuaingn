using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe struct NodoServicio
{
    public int Id;
    public int IdRepuesto;
    public int IdVehiculo;
    public string Detalles;
    public double Costo;
    public NodoServicio* Siguiente;

    public NodoServicio(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
    {
        Id = id;
        IdRepuesto = idRepuesto;
        IdVehiculo = idVehiculo;
        Detalles = detalles;
        Costo = costo;
        Siguiente = null;
    }
}