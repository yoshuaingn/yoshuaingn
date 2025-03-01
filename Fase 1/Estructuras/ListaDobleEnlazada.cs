using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe class ListaDobleEnlazada
{
    private NodoVehiculo* cabeza;
    private NodoVehiculo* cola;

    public ListaDobleEnlazada()
    {
        cabeza = null;
        cola = null;
    }

    public string AgregarVehiculo(int id, int idUsuario, string marca, int modelo, string placa)
    {
        if (BuscarVehiculo(id))
        {
            return "existente";
        }

        NodoVehiculo* nuevoNodo = (NodoVehiculo*)Marshal.AllocHGlobal(sizeof(NodoVehiculo));
        *nuevoNodo = new NodoVehiculo(id, idUsuario, marca, modelo, placa);
        nuevoNodo->Siguiente = null;
        nuevoNodo->Anterior = cola;

        if (cabeza == null)
        {
            cabeza = nuevoNodo;
        }
        else
        {
            cola->Siguiente = nuevoNodo;
        }

        cola = nuevoNodo;
        return null;
    }

    private bool BuscarVehiculo(int id)
    {
        NodoVehiculo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                return true;
            }
            actual = actual->Siguiente;
        }
        return false;
    }

    public void EditarVehiculo(int id, int idUsuario, string marca, int modelo, string placa)
    {
        NodoVehiculo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                actual->IdUsuario = idUsuario;
                actual->Marca = marca;
                actual->Modelo = modelo;
                actual->Placa = placa;
                return;
            }
            actual = actual->Siguiente;
        }
        Console.WriteLine($"Vehiculo con ID {id} no encontrado.");
    }

    public void EliminarVehiculo(int id)
    {
        NodoVehiculo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                if (actual->Anterior != null)
                {
                    actual->Anterior->Siguiente = actual->Siguiente;
                }
                else
                {
                    cabeza = actual->Siguiente;
                }

                if (actual->Siguiente != null)
                {
                    actual->Siguiente->Anterior = actual->Anterior;
                }
                else
                {
                    cola = actual->Anterior;
                }

                Marshal.FreeHGlobal((IntPtr)actual);
                return;
            }
            actual = actual->Siguiente;
        }
        Console.WriteLine($"Vehiculo con ID {id} no encontrado.");
    }
    ~ListaDobleEnlazada()
    {
        NodoVehiculo* actual = cabeza;
        while (actual != null)
        {
            NodoVehiculo* siguiente = actual->Siguiente;
            Marshal.FreeHGlobal((IntPtr)actual);
            actual = siguiente;
        }
    }
}