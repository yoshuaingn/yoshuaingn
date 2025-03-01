using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe class Nodo
{
    public int Id;
    public string Nombres;
    public string Apellidos;
    public string Correo;
    public string Contrasenia;
    public Nodo* Siguiente;

    public Nodo(int id, string nombres, string apellidos, string correo, string contrasenia)
    {
        Id = id;
        Nombres = nombres;
        Apellidos = apellidos;
        Correo = correo;
        Contrasenia = contrasenia;
        Siguiente = null;
    }
}