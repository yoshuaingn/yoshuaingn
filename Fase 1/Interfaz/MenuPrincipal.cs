using Gdk;
using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.ComponentModel;

public class MenuPrincipal : Gtk.Window
{
    public ListaSimpleEnlazada datosCargados;
    public MenuPrincipal() : base("Menú Principal")
    {   
        SetDefaultSize(300,200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += OnDeleteEvent;
    

        //Contenedor principal
        VBox vboxmenuPrincipal = new VBox(false, 10);
        vboxmenuPrincipal.BorderWidth = 10;
        Add(vboxmenuPrincipal);

        Button btnCargasMasivas = new Button("Cargas Masivas");
        Button btnIngresoIndividual = new Button("Ingreso Individual");
        Button btnGestionUsuarios = new Button("Gestión de Usuarios");
        Button btnGenerarServicio = new Button("Generar Servicio");
        Button btnCancelarFactura = new Button("Cancelar Factura");
        
        vboxmenuPrincipal.PackStart(btnCargasMasivas,false, false, 5);
        vboxmenuPrincipal.PackStart(btnIngresoIndividual,false, false, 5);
        vboxmenuPrincipal.PackStart(btnGestionUsuarios,false, false, 5);
        vboxmenuPrincipal.PackStart(btnGenerarServicio,false, false, 5);
        vboxmenuPrincipal.PackStart(btnCancelarFactura,false, false, 5);

        btnCargasMasivas.Clicked += OnCargasMasivasClicked;
        btnIngresoIndividual.Clicked += OnIngresoManualClicked;
        btnGestionUsuarios.Clicked += OnGestionUsuariosClicked;
        btnGenerarServicio.Clicked += OnGenerarServicioClicked;
        btnCancelarFactura.Clicked += OnCancelarFacturaClicked;

        ShowAll();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnCargasMasivasClicked(object sender, System.EventArgs e)
    {   
        CargaMasiva ventanaCargaMasiva = new CargaMasiva(this);
        ventanaCargaMasiva.DatoCargado += OnDatoCargado;
        ventanaCargaMasiva.Show();
        this.Hide();
    }
    protected void OnIngresoManualClicked(object sender, System.EventArgs e)
    {   
        IngresoUsuarios ventanaIngresoUsuarios = new IngresoUsuarios(this);
        ventanaIngresoUsuarios.DatoCargado += OnDatoCargado;
        ventanaIngresoUsuarios.Show();
        this.Hide();
    }

    protected void OnGestionUsuariosClicked(object sender, System.EventArgs e)
    {   
        GestionUsuarios ventanaGestionUsuarios = new GestionUsuarios(this);
        ventanaGestionUsuarios.Show();
        this.Hide();
    }

    protected void OnGenerarServicioClicked(object sender, System.EventArgs e)
    {
        CrearServicio ventanaCrearServicio = new CrearServicio(this);
        ventanaCrearServicio.Show();
        this.Hide();
    }

    protected void OnCancelarFacturaClicked(object sender, System.EventArgs e)
    {
        /*CancelarFactura ventanaCancelarFactura = new CancelarFactura(this);
        ventanaCancelarFactura.Show();
        this.Hide();*/
    }

    private void OnDatoCargado(object sender, ListaSimpleEnlazada dato)
    {
        datosCargados = dato;
    }
}