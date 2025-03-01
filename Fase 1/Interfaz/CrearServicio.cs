using Gdk;
using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Json;

public class CrearServicio : Gtk.Window
{
    private Entry entradaId, entradaIdRepuesto, entradaIdVehiculo, entradaDetalles, entradaCosto;
    private Button botonGuardar, botonRegresar;

    private MenuPrincipal ventanaPrincipal;
    public CrearServicio(MenuPrincipal ventanaPrincipal) : base("Crear Servicio")
    {
        this.ventanaPrincipal = ventanaPrincipal;
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += OnDeleteEvent;

        Fixed fixedLayout = new Fixed();
        Add(fixedLayout);

        Label etiquetaId = new Label("ID");
        entradaId = new Entry();
        fixedLayout.Put(etiquetaId, 20, 20);
        fixedLayout.Put(entradaId, 150, 20);

        Label etiquetaIdRepuesto = new Label("Id_Repuesto");
        entradaIdRepuesto = new Entry();
        fixedLayout.Put(etiquetaIdRepuesto, 20, 60);
        fixedLayout.Put(entradaIdRepuesto, 150, 60);

        Label etiquetaIdVehiculo = new Label("Id_Vehiculo");
        entradaIdVehiculo = new Entry();
        fixedLayout.Put(etiquetaIdVehiculo, 20, 100);
        fixedLayout.Put(entradaIdVehiculo, 150, 100);

        Label etiquetaDetalles = new Label("Detalles");
        entradaDetalles = new Entry();
        fixedLayout.Put(etiquetaDetalles, 20, 140);
        fixedLayout.Put(entradaDetalles, 150, 140);

        Label etiquetaCosto = new Label("Costo");
        entradaCosto = new Entry();
        fixedLayout.Put(etiquetaCosto, 20, 180);
        fixedLayout.Put(entradaCosto, 150, 180);

        botonGuardar = new Button("Guardar");
        botonRegresar = new Button("Regresar");

        fixedLayout.Put(botonGuardar, 150, 240);
        fixedLayout.Put(botonRegresar, 250, 240);

        botonGuardar.Clicked += OnBotonGuardarClicked;
        botonRegresar.Clicked += OnBotonRegresarClicked;

        ShowAll();
    }

    private void OnBotonGuardarClicked(object sender, EventArgs a)
    {
        Console.WriteLine("Datos del servicio guardados");
        Console.WriteLine($"ID: {entradaId.Text}");
        Console.WriteLine($"Id_Repuesto: {entradaIdRepuesto.Text}");
        Console.WriteLine($"Id_Vehiculo: {entradaIdVehiculo.Text}");
        Console.WriteLine($"Detalles: {entradaDetalles.Text}");
        Console.WriteLine($"Costo: {entradaCosto.Text}");
    }

    private void OnBotonRegresarClicked(object sender, EventArgs a)
    {
        ventanaPrincipal.Show();
        this.Destroy();   
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

}