using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe struct NodoRepuesto
{
    public int Id;
    public string Repuesto;
    public string Detalles;
    public double Costo;
    public NodoRepuesto* Siguiente;

    public NodoRepuesto(int id, string repuesto, string detalles, double costo)
    {
        Id = id;
        Repuesto = repuesto;
        Detalles = detalles;
        Costo = costo;
        Siguiente = null;
    }
}