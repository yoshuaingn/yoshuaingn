using System;
using System.Text;
using System.Runtime.InteropServices;



public unsafe class ListaCircular
{
    private NodoRepuesto* cabeza;

    public ListaCircular()
    {
        cabeza = null;
    }

    public string AgregarRepuesto(int id, string repuesto, string detalles, double costo)
    {
        if (BuscarRepuesto(id))
        {
            return "existente";
        }

        NodoRepuesto* nuevoNodo = (NodoRepuesto*)Marshal.AllocHGlobal(sizeof(NodoRepuesto));
        *nuevoNodo = new NodoRepuesto(id, repuesto, detalles, costo);

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
            nuevoNodo->Siguiente = cabeza; // Apunta a sí mismo para formar el círculo
        }
        else
        {
            NodoRepuesto* ultimo = cabeza;
            while (ultimo->Siguiente != cabeza)
            {
                ultimo = ultimo->Siguiente;
            }
            ultimo->Siguiente = nuevoNodo;
            nuevoNodo->Siguiente = cabeza;
        }

        return null;
    }

    private bool BuscarRepuesto(int id)
    {
        if (cabeza == null) return false;

        NodoRepuesto* actual = cabeza;
        do
        {
            if (actual->Id == id)
            {
                return true;
            }
            actual = actual->Siguiente;
        } while (actual != cabeza);

        return false;
    }

    public void AgregarLista(ListaCircular otraLista)
    {
        NodoRepuesto* actual = otraLista.cabeza;
        if (actual == null) return;

        do
        {
            AgregarRepuesto(actual->Id, actual->Repuesto, actual->Detalles, actual->Costo);
            actual = actual->Siguiente;
        } while (actual != otraLista.cabeza);
    }

    public ListaCircular CopiarLista()
    {
        ListaCircular nuevaLista = new ListaCircular();
        NodoRepuesto* actual = cabeza;
        if (actual == null) return nuevaLista;

        do
        {
            nuevaLista.AgregarRepuesto(actual->Id, actual->Repuesto, actual->Detalles, actual->Costo);
            actual = actual->Siguiente;
        } while (actual != cabeza);

        return nuevaLista;
    }

    public string MostrarRepuestosComoString()
    {
        StringBuilder sb = new StringBuilder();
        if (cabeza == null) return "";

        NodoRepuesto* actual = cabeza;
        do
        {
            sb.AppendLine("{");
            sb.AppendLine($"  \"ID\": {actual->Id},");
            sb.AppendLine($"  \"Repuesto\": \"{actual->Repuesto}\",");
            sb.AppendLine($"  \"Detalles\": \"{actual->Detalles}\",");
            sb.AppendLine($"  \"Costo\": {actual->Costo}");
            sb.AppendLine("}");

            actual = actual->Siguiente;
        } while (actual != cabeza);

        return sb.ToString();
    }

    public void EditarRepuesto(int id, string repuesto, string detalles, double costo)
    {
        if (cabeza == null) return;

        NodoRepuesto* actual = cabeza;
        do
        {
            if (actual->Id == id)
            {
                actual->Repuesto = repuesto;
                actual->Detalles = detalles;
                actual->Costo = costo;
                return;
            }
            actual = actual->Siguiente;
        } while (actual != cabeza);

        Console.WriteLine($"Repuesto con ID {id} no encontrado.");
    }

    public void EliminarRepuesto(int id)
    {
        if (cabeza == null) return;

        if (cabeza->Id == id)
        {
            NodoRepuesto* ultimo = cabeza;
            while (ultimo->Siguiente != cabeza)
            {
                ultimo = ultimo->Siguiente;
            }
            if (cabeza->Siguiente == cabeza)
            {
                Marshal.FreeHGlobal((IntPtr)cabeza);
                cabeza = null;
            }
            else
            {
                NodoRepuesto* temp = cabeza;
                cabeza = cabeza->Siguiente;
                ultimo->Siguiente = cabeza;
                Marshal.FreeHGlobal((IntPtr)temp);
            }
            return;
        }

        NodoRepuesto* actual = cabeza;
        while (actual->Siguiente != cabeza)
        {
            if (actual->Siguiente->Id == id)
            {
                NodoRepuesto* temp = actual->Siguiente;
                actual->Siguiente = actual->Siguiente->Siguiente;
                Marshal.FreeHGlobal((IntPtr)temp);
                return;
            }
            actual = actual->Siguiente;
        }
        Console.WriteLine($"Repuesto con ID {id} no encontrado.");
    }

    public void LimpiarLista()
    {
        if (cabeza == null) return;

        NodoRepuesto* actual = cabeza;
        do
        {
            NodoRepuesto* siguiente = actual->Siguiente;
            Marshal.FreeHGlobal((IntPtr)actual);
            actual = siguiente;
        } while (actual != cabeza);

        cabeza = null;
    }

    ~ListaCircular()
    {
        LimpiarLista();
    }
}