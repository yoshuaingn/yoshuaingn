using System;
using System.Text;
using System.Runtime.InteropServices;


public unsafe class PilaFacturas
{
    private NodoFactura* cima;
    private int ultimoIdFactura;
    private int ultimoIdOrden;

    public PilaFacturas()
    {
        cima = null;
        ultimoIdFactura = 0;
        ultimoIdOrden = 0;
    }

    public void Push(double total)
    {
        ultimoIdFactura++;
        ultimoIdOrden++;
        NodoFactura* nuevoNodo = (NodoFactura*)Marshal.AllocHGlobal(sizeof(NodoFactura));
        *nuevoNodo = new NodoFactura(ultimoIdFactura, ultimoIdOrden, total);
        nuevoNodo->Siguiente = cima;
        cima = nuevoNodo;
    }

    public NodoFactura* Pop()
    {
        if (cima == null)
        {
            return null; // Pila vacía
        }

        NodoFactura* temp = cima;
        cima = cima->Siguiente;
        return temp;
    }

    public string MostrarFacturasComoString()
    {
        StringBuilder sb = new StringBuilder();
        NodoFactura* actual = cima;

        while (actual != null)
        {
            sb.AppendLine("{");
            sb.AppendLine($"  \"ID\": {actual->Id},");
            sb.AppendLine($"  \"ID_Orden\": {actual->IdOrden},");
            sb.AppendLine($"  \"Total\": {actual->Total}");
            sb.AppendLine("}");

            actual = actual->Siguiente;
        }

        return sb.ToString();
    }

    public void LimpiarPila()
    {
        while (cima != null)
        {
            NodoFactura* temp = cima;
            cima = cima->Siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        }
        ultimoIdFactura = 0;
        ultimoIdOrden = 0;
    }

    ~PilaFacturas()
    {
        LimpiarPila();
    }
}