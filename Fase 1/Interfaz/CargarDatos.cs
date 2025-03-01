using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class Datos
{   
    public ListaSimpleEnlazada Usuarios;

    public ListaDobleEnlazada Vehiculo;

    public ListaCircular Repuesto;

    public Datos()
    {
        Usuarios = new ListaSimpleEnlazada();
        Vehiculo = new ListaDobleEnlazada();
        Repuesto = new ListaCircular();

    }

    public static Datos CargarDatos(string rutaArchivo, string tipoDato)
    {
            try
        {
            string json = File.ReadAllText(rutaArchivo);
            JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            Datos datos = new Datos();
           
            switch (tipoDato)
            {
                case "Usuario":
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement usuario in root.EnumerateArray())
                        {
                            int id = usuario.GetProperty("ID").GetInt32();
                            string nombres = usuario.GetProperty("Nombres").GetString();
                            string apellidos = usuario.GetProperty("Apellidos").GetString();
                            string correo =usuario.GetProperty("Correo").GetString();
                            string contrasenia = usuario.GetProperty("Contrasenia").GetString();
                            datos.Usuarios.AgregarUsuario(id, nombres, apellidos, correo, contrasenia);
                        }
                        string codigoGraphviz = datos.Usuarios.GenerarCodigoGraphviz();
                        System.IO.File.WriteAllText("lista_usuarios.dot", codigoGraphviz);
                        GenerarYMostrarImagenGraphviz("lista_usuarios.dot", "lista_usuarios.png");
                    }
                    break;
                case "Vehiculos":
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement vehiculo in root.EnumerateArray())
                        {
                            int id = vehiculo.GetProperty("ID").GetInt32();
                            int id_usuario = vehiculo.GetProperty("ID_Usuario").GetInt32();
                            string marca = vehiculo.GetProperty("Marca").GetString();
                            int modelo = vehiculo.GetProperty("Modelo").GetInt32();
                            string placa = vehiculo.GetProperty("Placa").GetString();
                            datos.Vehiculo.AgregarVehiculo(id, id_usuario, marca, modelo, placa);
                        }
                    }
                    break;
                case "Repuestos":
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement repuesto in root.EnumerateArray())
                        {
                            int id = repuesto.GetProperty("ID").GetInt32();
                            string nombreRepuesto = repuesto.GetProperty("Repuesto").GetString();
                            string detalles = repuesto.GetProperty("Detalles").GetString();
                            double costo = repuesto.GetProperty("Costo").GetDouble();
                            datos.Repuesto.AgregarRepuesto(id, nombreRepuesto, detalles, costo);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Tipo de dato no válido.");
                    return null;
            }
            
            return datos;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos: {ex.Message}");
            return null;
        }
    }
    
    public static void GenerarYMostrarImagenGraphviz(string archivoDot, string archivoPng)
    {
        try
        {
            // Ejecutar Graphviz para generar la imagen PNG
            ProcessStartInfo psi = new ProcessStartInfo("dot", $"-Tpng {archivoDot} -o {archivoPng}");
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process process = Process.Start(psi);
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                string error = process.StandardError.ReadToEnd();
                Console.WriteLine($"Error al ejecutar Graphviz: {error}");
                return;
            }

            Console.WriteLine($"Imagen generada: {archivoPng}");

            // Abrir la imagen generada (Linux)
            Process.Start("xdg-open", archivoPng); // Intenta abrir con el visor de imágenes predeterminado
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

}