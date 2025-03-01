using System;
using System.Text;
using System.Runtime.InteropServices;


public unsafe class ColaServicios
{
    private NodoServicio* frente;
    private NodoServicio* final;

    public ColaServicios()
    {
        frente = null;
        final = null;
    }

    public void Enqueue(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
    {
        NodoServicio* nuevoNodo = (NodoServicio*)Marshal.AllocHGlobal(sizeof(NodoServicio));
        *nuevoNodo = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);

        if (final == null)
        {
            frente = nuevoNodo;
            final = nuevoNodo;
        }
        else
        {
            final->Siguiente = nuevoNodo;
            final = nuevoNodo;
        }
    }

    public NodoServicio* Dequeue()
    {
        if (frente == null)
        {
            return null; // Cola vacía
        }

        NodoServicio* temp = frente;
        frente = frente->Siguiente;

        if (frente == null)
        {
            final = null; // La cola quedó vacía
        }

        return temp;
    }

    public string MostrarServiciosComoString()
    {
        StringBuilder sb = new StringBuilder();
        NodoServicio* actual = frente;

        while (actual != null)
        {
            sb.AppendLine("{");
            sb.AppendLine($"  \"ID\": {actual->Id},");
            sb.AppendLine($"  \"ID_Repuesto\": {actual->IdRepuesto},");
            sb.AppendLine($"  \"ID_Vehiculo\": {actual->IdVehiculo},");
            sb.AppendLine($"  \"Detalles\": \"{actual->Detalles}\",");
            sb.AppendLine($"  \"Costo\": {actual->Costo}");
            sb.AppendLine("}");

            actual = actual->Siguiente;
        }

        return sb.ToString();
    }

    public void LimpiarCola()
    {
        while (frente != null)
        {
            NodoServicio* temp = frente;
            frente = frente->Siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        }
        final = null;
    }

    ~ColaServicios()
    {
        LimpiarCola();
    }
}