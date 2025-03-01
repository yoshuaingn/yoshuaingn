using Gdk;
using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Json;

public class CancelarFactura : Gtk.Window
{
    private MenuPrincipal ventanaPrincipal;
    public CancelarFactura(MenuPrincipal ventanaPrincipal, int id, int idOrden, double total) : base("Facturación")
    {
        this.ventanaPrincipal = ventanaPrincipal;
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += OnDeleteEvent;

        Fixed fixedLayout = new Fixed();
        Add(fixedLayout);

        Label etiquetaId = new Label("Id");
        Label valorId = new Label(id.ToString());
        fixedLayout.Put(etiquetaId, 20, 20);
        fixedLayout.Put(valorId, 150, 20);

        Label etiquetaIdOrden = new Label("Id_Orden");
        Label valorIdOrden = new Label(idOrden.ToString());
        fixedLayout.Put(etiquetaIdOrden, 20, 60);
        fixedLayout.Put(valorIdOrden, 150, 60);

        Label etiquetaTotal = new Label("Total");
        Label valorTotal = new Label(total.ToString());
        fixedLayout.Put(etiquetaTotal, 20, 100);
        fixedLayout.Put(valorTotal, 150, 100);

        Button botonRegresar = new Button("Regresar");
        fixedLayout.Put(botonRegresar, 100, 150);
        botonRegresar.Clicked += OnBotonRegresarClicked;

        ShowAll();
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