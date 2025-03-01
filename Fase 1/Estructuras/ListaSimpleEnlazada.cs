using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe class ListaSimpleEnlazada
{
    public Nodo* cabeza;
    public ListaSimpleEnlazada()
    {
        cabeza = null;
    }

    public string AgregarUsuario(int id, string nombres, string apellidos, string correo, string contrasenia)
    {
    
        if (BuscarUsuario(id))
        {
            return "existente"; 
        }

        Nodo* nuevoNodo = (Nodo*)Marshal.AllocHGlobal(sizeof(Nodo));
        *nuevoNodo = new Nodo(id, nombres, apellidos, correo, contrasenia);
        nuevoNodo->Siguiente = cabeza;
        cabeza = nuevoNodo;

        return null; 
    }

    public bool BuscarUsuario(int id)
    {
        Nodo* actual = cabeza;
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
    public string MostrarUsuariosComoString()
    {
        StringBuilder sb = new StringBuilder();
        Nodo* actual = cabeza;

        while (actual != null)
        {
            sb.AppendLine("{");
            sb.AppendLine($"  \"ID\": {actual->Id},");
            sb.AppendLine($"  \"Nombres\": \"{actual->Nombres}\",");
            sb.AppendLine($"  \"Apellidos\": \"{actual->Apellidos}\",");
            sb.AppendLine($"  \"Correo\": \"{actual->Correo}\",");
            sb.AppendLine($"  \"Contrasenia\": \"{actual->Contrasenia}\"");
            sb.AppendLine("}");

            actual = actual->Siguiente;
        }

        return sb.ToString();
    }

    public void EditarUsuario(int id, string nombres, string apellidos, string correo)
    {
        Nodo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                actual->Nombres = nombres;
                actual->Apellidos = apellidos;
                actual->Correo = correo;
                return;
            }
            actual = actual->Siguiente;
        }
    }

    public void EliminarUsuario(int id)
    {
        if (cabeza == null) return;

        if (cabeza->Id == id)
        {
            Nodo* temp = cabeza;
            cabeza = cabeza->Siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
            return;
        }

        Nodo* actual = cabeza;
        while (actual->Siguiente != null)
        {
            if (actual->Siguiente->Id == id)
            {
                Nodo* temp = actual->Siguiente;
                actual->Siguiente = actual->Siguiente->Siguiente;
                Marshal.FreeHGlobal((IntPtr)temp);
                return;
            }
            actual = actual->Siguiente;
        }
    }

        public string BuscarNombresPorId(int id)
    {
        Nodo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                return actual->Nombres;
            }
            actual = actual->Siguiente;
        }
        return null; 
    }

    public string BuscarApellidosPorId(int id)
    {
        Nodo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                return actual->Apellidos;
            }
            actual = actual->Siguiente;
        }
        return null; 
    }

    public string BuscarCorreoPorId(int id)
    {
        Nodo* actual = cabeza;
        while (actual != null)
        {
            if (actual->Id == id)
            {
                return actual->Correo;
            }
            actual = actual->Siguiente;
        }
        return null; 
    }

    ~ListaSimpleEnlazada()
    {
        Nodo* actual = cabeza;
        while (actual != null)
        {
            Nodo* siguiente = actual->Siguiente;
            Marshal.FreeHGlobal((IntPtr)actual);
            actual = siguiente;
        }
    }

    public string GenerarCodigoGraphviz()
    {
        StringBuilder codigoGraphviz = new StringBuilder();
        codigoGraphviz.AppendLine("digraph ListaUsuarios {");
        codigoGraphviz.AppendLine("    node [shape=box];"); // Usar cajas para los nodos

        Nodo* actual = cabeza;
        while (actual != null)
        {
            codigoGraphviz.AppendLine($"    nodo{actual->Id} [label=\"ID: {actual->Id}\\nNombre: {actual->Nombres} {actual->Apellidos}\\nCorreo: {actual->Correo}\"];");
            if (actual->Siguiente != null)
            {
                codigoGraphviz.AppendLine($"    nodo{actual->Id} -> nodo{actual->Siguiente->Id};");
            }
            actual = actual->Siguiente;
        }

        codigoGraphviz.AppendLine("}");
        return codigoGraphviz.ToString();
    }

}