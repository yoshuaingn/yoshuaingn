using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe struct NodoVehiculo
{
    public int Id;
    public int IdUsuario;
    public string Marca;
    public int Modelo;
    public string Placa;
    public NodoVehiculo* Siguiente;
    public NodoVehiculo* Anterior;

    public NodoVehiculo(int id, int idUsuario, string marca, int modelo, string placa)
    {
        Id = id;
        IdUsuario = idUsuario;
        Marca = marca;
        Modelo = modelo;
        Placa = placa;
        Siguiente = null;
        Anterior = null;
    }
}
