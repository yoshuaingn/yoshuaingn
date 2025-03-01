using Gdk;
using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http.Json;

public class Login : Gtk.Window
{
    private Entry txtusuario;
    private Entry txtContraseña;

    public Login() : base("Inicio de Sesión")
    {
        SetDefaultSize(300,200);
        SetPosition(WindowPosition.Center);
        DeleteEvent += OnDeleteEvent;

        VBox vboxPrincipal = new VBox(false, 10);
        vboxPrincipal.BorderWidth = 10;
        Add(vboxPrincipal);

        Label lblUsuario = new Label("Usuario:");
        txtusuario = new Entry();
        Label lblContraseña = new Label("Contraseña:");
        txtContraseña = new Entry();
        txtContraseña.Visibility = false;

        Button btnIniciarSesion = new Button("Iniciar Sesión");
        btnIniciarSesion.Clicked += OnIniciarSesionClicked;

        vboxPrincipal.PackStart(lblUsuario, false, false, 5);
        vboxPrincipal.PackStart(lblContraseña, false, false, 5);
        vboxPrincipal.PackStart(txtusuario, false, false, 5);
        vboxPrincipal.PackStart(txtContraseña, false, false, 5);
        vboxPrincipal.PackStart(btnIniciarSesion, false, false, 5);

        ShowAll();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnIniciarSesionClicked(object sender, System.EventArgs e)
    {
        string usuario = txtusuario.Text;
        string contraseña = txtContraseña.Text;

        if (usuario == "root@gmail.com" && contraseña == "root123")
        {
            new MenuPrincipal();
            this.Hide();
        } else
        {
            MessageDialog mensajeError = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Credenciales Incorrectar.");
            mensajeError.Run();
            mensajeError.Destroy();
        }
    }

    public static void Main(string[] args)
    {
        Application.Init();
        new Login();
        Application.Run();
    }
}