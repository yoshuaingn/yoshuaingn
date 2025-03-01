using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;


public class CargaMasiva : Gtk.Window
{
    private ComboBox comboDato;
    private FileChooserButton fileChooser;
    private TextView txtVistaPrevia;
    private MenuPrincipal ventanaPrincipal;
    public delegate void DatoCargadoEventHandler(object sender, ListaSimpleEnlazada dato);
    public event DatoCargadoEventHandler DatoCargado;
    public CargaMasiva(MenuPrincipal ventanaPrincipal) : base("Carga Masiva")
    {
        this.ventanaPrincipal = ventanaPrincipal;   
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        DeleteEvent += OnDeleteEvent;

        VBox vboxPrincipal = new VBox(false, 10);
        vboxPrincipal.BorderWidth = 10;
        Add(vboxPrincipal);

        Label lblDato = new Label("Tipo de Dato:");
        comboDato = new ComboBoxText();

        ListStore model = new ListStore(typeof(string));
        comboDato.Model = model;

        model.AppendValues("Usuario");
        model.AppendValues("Vehiculos");
        model.AppendValues("Repuestos");
        comboDato.Active = 0;

        fileChooser = new FileChooserButton("Seleccionar Archivo JSON", FileChooserAction.Open);
        fileChooser.FileSet += OnFileSet;

        txtVistaPrevia = new TextView();
        txtVistaPrevia.Editable = false;

        Button btnCargar = new Button("Cargar");
        btnCargar.Clicked += OnCargarClicked;

        Button btnRegresar = new Button("Regresar");
        btnRegresar.Clicked += OnRegresarClicked;

        vboxPrincipal.PackStart(lblDato, false, false, 5);
        vboxPrincipal.PackStart(comboDato, false, false, 5);
        vboxPrincipal.PackStart(fileChooser, false, false, 5);
        vboxPrincipal.PackStart(txtVistaPrevia, false, false, 5);
        vboxPrincipal.PackStart(btnCargar, false, false, 10);
        vboxPrincipal.PackStart(btnRegresar, false, false, 5);

        ShowAll();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        a.RetVal = true;
        Application.Quit();
    }

    protected void OnFileSet(object sender, EventArgs e)
    {
        string filename = fileChooser.Filename;
        if (!string.IsNullOrEmpty(filename) && filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                string json = File.ReadAllText(filename);
            }
            catch (Exception ex)
            {
                MostrarError("Error al leer el archivo JSON: " + ex.Message);
            }
        }
        else 
        {
            MostrarError("Seleccione un archivo JSON válido.");
        }
    }

    protected void OnCargarClicked(object sender, EventArgs e)
    {
        string filename = fileChooser.Filename;
        string tipoDato = null;
        TreeIter iter;
        if (comboDato.GetActiveIter(out iter))
        {
            tipoDato = (string)comboDato.Model.GetValue(iter, 0);
        }
        else 
        {
            MostrarError("No se ha seleccionado ningún tipo de dato.");
            return;
        }

        if (string.IsNullOrEmpty(filename))
        {
            MostrarError("Seleccione un archivo JSON.");
            return;
        }

        try
        {   
            Datos dato = Datos.CargarDatos(filename, tipoDato);

            if (DatoCargado != null)
            {   
                DatoCargado(this, dato.Usuarios);    
                
            }
        }
        catch (Exception ex)
        {
            MostrarError("Error al cargar los datos: " + ex.Message);
        }
    }
    
    private void MostrarError(string mensaje)
    {
        MessageDialog mensajeError = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, mensaje);
        mensajeError.Run();
        mensajeError.Destroy(); 
    }

    protected void OnRegresarClicked(object sender, EventArgs e){
        ventanaPrincipal.Show();
        this.Hide();
    }

   
}


// Ingreso de usuarios
public class IngresoUsuarios: Gtk.Window
{
    private Entry idEntry, nombresEntry, apellidosEntry, correoEntry, contraseniaEntry;
    private MenuPrincipal ventanaPrincipal;
    public delegate void DatoCargadoEventHandler(object sender, ListaSimpleEnlazada dato);
    public event DatoCargadoEventHandler DatoCargado;

    public string existente;
    public IngresoUsuarios(MenuPrincipal ventanaPrincipal)  : base("Ingreso Manual")
    {
        this.ventanaPrincipal = ventanaPrincipal;
        Table table = new Table(6, 2, false); 
        table.BorderWidth = 10;
        table.ColumnSpacing = 5;
        table.RowSpacing = 5;

      
        Label idLabel = new Label("Id");
        idEntry = new Entry();
        Label nombresLabel = new Label("Nombres");
        nombresEntry = new Entry();
        Label apellidosLabel = new Label("Apellidos");
        apellidosEntry = new Entry();
        Label correoLabel = new Label("Correo");
        correoEntry = new Entry();
        Label contraseniaLabel = new Label("Contraseña");
        contraseniaEntry = new Entry();

        
        Button guardarButton = new Button("Guardar");
        guardarButton.Clicked += OnGuardarClicked;

        
        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += OnRegresarClicked;

       
        table.Attach(idLabel, 0, 1, 0, 1);
        table.Attach(idEntry, 1, 2, 0, 1);
        table.Attach(nombresLabel, 0, 1, 1, 2);
        table.Attach(nombresEntry, 1, 2, 1, 2);
        table.Attach(apellidosLabel, 0, 1, 2, 3);
        table.Attach(apellidosEntry, 1, 2, 2, 3);
        table.Attach(correoLabel, 0, 1, 3, 4);
        table.Attach(correoEntry, 1, 2, 3, 4);
        table.Attach(contraseniaLabel, 0, 1, 4, 5);
        table.Attach(contraseniaEntry, 1, 2, 4, 5);
        table.Attach(guardarButton, 1, 2, 5, 6);
        table.Attach(regresarButton, 0, 1, 5, 6); 


        Add(table);

        Resizable = false;
        WindowPosition = WindowPosition.Center;
        DeleteEvent += delegate { Application.Quit(); };

        ShowAll();
    
    } 

    protected void OnGuardarClicked(object sender, System.EventArgs e)
    {
        string id_string = idEntry.Text;
        int id = int.Parse(id_string);
        string nombres = nombresEntry.Text;
        string apellidos = apellidosEntry.Text;
        string correo = correoEntry.Text;
        string contrasenia = contraseniaEntry.Text;

        existente = ventanaPrincipal.datosCargados.AgregarUsuario(id,nombres,apellidos,correo,contrasenia);
        string codigoGraphviz = ventanaPrincipal.datosCargados.GenerarCodigoGraphviz();
        System.IO.File.WriteAllText("lista_usuarios.dot", codigoGraphviz);
        GenerarYMostrarImagenGraphviz("lista_usuarios.dot", "lista_usuarios.png");

        if (existente == "existente")
        {
            MostrarError("El usuario ya existe.");
        } else
        {
            if (DatoCargado != null)
            {
            DatoCargado(this, ventanaPrincipal.datosCargados);
            }
        }
          
        idEntry.Text = "";
        nombresEntry.Text = "";
        apellidosEntry.Text = "";
        correoEntry.Text = "";
        contraseniaEntry.Text = "";

        }

    private void GenerarYMostrarImagenGraphviz(string v1, string v2)
    {
        throw new NotImplementedException();
    }

    private void MostrarError(string mensaje)
    {
        MessageDialog mensajeError = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, mensaje);
        mensajeError.Run();
        mensajeError.Destroy(); 
    }
    protected void OnRegresarClicked(object sender, EventArgs e)
    {   
        ventanaPrincipal.Show();
        this.Destroy();
    }
}


// Gestion Usuarios
public class GestionUsuarios : Gtk.Window
{  
    private MenuPrincipal ventanaPrincipal;

    public IngresoUsuarios usuariomanual;

    public ListaSimpleEnlazada lista, lista2;
    public GestionUsuarios(MenuPrincipal ventanaPrincipal) : base("Gestión de Usuarios")
    {
        this.ventanaPrincipal = ventanaPrincipal;
        SetDefaultSize(300,200);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        Button verUsuarioButton = new Button("Ver Usuario");
        verUsuarioButton.Clicked += OnVerUsuarioClicked;

        Button editarUsuarioButton = new Button("Editar Usuario");
        editarUsuarioButton.Clicked += OnEditarUsuarioClicked;

        Button eliminarUsuarioButton = new Button("Eliminar Usuario");
        eliminarUsuarioButton.Clicked += OnEliminarUsuarioClicked;

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += OnRegresarClicked;

        vbox.PackStart(verUsuarioButton, false, false, 0);
        vbox.PackStart(editarUsuarioButton, false, false, 0);
        vbox.PackStart(eliminarUsuarioButton, false, false, 0);
        vbox.PackStart(regresarButton, false, false, 0);

        Add(vbox);

        Resizable = false;
        WindowPosition = WindowPosition.Center;
        DeleteEvent += new DeleteEventHandler(OnDeleteEvent);
   
        ShowAll();
    }

    protected void OnVerUsuarioClicked(object sender, System.EventArgs e)
    {   
        lista = ventanaPrincipal.datosCargados;

        if (lista != null)
        {
            VerUsuarios ventanaUsuarios = new VerUsuarios(this, lista);
            ventanaUsuarios.Show();
        } else
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "No hay datos cargados.");
            md.Run();
            md.Destroy();
        }
    }

    protected void OnEditarUsuarioClicked(object sender, System.EventArgs e)
    {
        if (ventanaPrincipal.datosCargados != null)
        {
            EditarUsuario ventanaEditarUsuarios = new EditarUsuario(this, ventanaPrincipal.datosCargados);
            ventanaEditarUsuarios.Show();
        } else
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "No hay datos cargados.");
            md.Run();
            md.Destroy();
        }
    }

    protected void OnEliminarUsuarioClicked(object sender, System.EventArgs e)
    {
        if (ventanaPrincipal.datosCargados != null)
        {
            EliminarUsuario ventanaEliminarUsuarios = new EliminarUsuario(this, ventanaPrincipal.datosCargados);
            ventanaEliminarUsuarios.Show();
        } else
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "No hay datos cargados.");
            md.Run();
            md.Destroy();
        }
        
    }

    protected void OnRegresarClicked(object sender, System.EventArgs e)
    {
        ventanaPrincipal.Show();
        this.Hide();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}

//Editar Usuario
public class EditarUsuario : Gtk.Window
{
    private Entry entradaIdBusqueda, entradaNombres, entradaApellidos, entradaCorreo;
    private Button botonActualizar, botonRegresar, botonBuscar;
    private GestionUsuarios ventanaGestionUsuarios;
    public ListaSimpleEnlazada usuarios;
    public EditarUsuario(GestionUsuarios ventanaGestionUsuarios, ListaSimpleEnlazada lista) : base("Editar Usuario")
    {
        this.ventanaGestionUsuarios = ventanaGestionUsuarios;
        this.usuarios = lista;
        // Configurar la ventana principal
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Table table = new Table(4, 3, false);
        table.ColumnSpacing = 5;
        table.RowSpacing = 5;

        Label idLabel = new Label("Id");
        entradaIdBusqueda = new Entry();
        Button buscarButton = new Button("Buscar");

        Label nombresLabel = new Label("Nombres");
        entradaNombres = new Entry();

        Label apellidosLabel = new Label("Apellidos");
        entradaApellidos = new Entry();

        Label correoLabel = new Label("Correo");
        entradaCorreo = new Entry();

        Button actualizarButton = new Button("Actualizar");

        Button RegresarButton = new Button("Regresar");

        table.Attach(idLabel, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaIdBusqueda, 1, 2, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(buscarButton, 2, 3, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(nombresLabel, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaNombres, 1, 2, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(apellidosLabel, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaApellidos, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(correoLabel, 0, 1, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaCorreo, 1, 2, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(actualizarButton, 1, 2, 4, 5, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(RegresarButton, 2, 3, 4, 5, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        Add(table);

        actualizarButton.Clicked += OnBotonActualizarClicked;
        buscarButton.Clicked += OnBotonBuscarClicked;
        RegresarButton.Clicked += OnBotonRegresarClicked;

        ShowAll();
    }
    public void OnBotonBuscarClicked(object sender, EventArgs a)
    {
        if (int.TryParse(entradaIdBusqueda.Text, out int idBuscado))
        {
            var usuarioEncontrado = usuarios.BuscarUsuario(idBuscado);

            if (usuarioEncontrado == true)
            {
                entradaNombres.Text = usuarios.BuscarNombresPorId(idBuscado);
                entradaApellidos.Text = usuarios.BuscarApellidosPorId(idBuscado);
                entradaCorreo.Text = usuarios.BuscarCorreoPorId(idBuscado);
            }
            else
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "Usuario no encontrado.");
                md.Run();
                md.Destroy();
                LimpiarCajasDeTexto();
            }
        }
        else
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "ID invalido.");
            md.Run();
            md.Destroy();
            LimpiarCajasDeTexto();
        }
    }

    private void LimpiarCajasDeTexto()
    {

        entradaNombres.Text = "";
        entradaApellidos.Text = "";
        entradaCorreo.Text = "";
    }

    private void OnBotonActualizarClicked(object sender, EventArgs a)
    {   
        string id = entradaIdBusqueda.Text;
        int id_int = int.Parse(id);
        string nombres = entradaNombres.Text;
        string apellidos = entradaApellidos.Text;
        string correo = entradaCorreo.Text;
        usuarios.EditarUsuario(id_int, nombres, apellidos, correo);
        string codigoGraphviz = usuarios.GenerarCodigoGraphviz();
        System.IO.File.WriteAllText("lista_usuarios.dot", codigoGraphviz);
        GenerarYMostrarImagenGraphviz("lista_usuarios.dot", "lista_usuarios.png");
        LimpiarCajasDeTexto();
    }

    private void GenerarYMostrarImagenGraphviz(string v1, string v2)
    {
        throw new NotImplementedException();
    }

    private void OnBotonRegresarClicked(object sender, EventArgs a)
    {
        ventanaGestionUsuarios.Show();
        this.Destroy();
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}

//Eliminar Usuario
public class EliminarUsuario : Gtk.Window
{
    private Entry entradaIdBusqueda, entradaNombres, entradaApellidos, entradaCorreo;
    private Button botonEliminar, botonRegresar, botonBuscar;
    private ListaSimpleEnlazada usuarios;
    private GestionUsuarios ventanaGestionUsuarios;
    public EliminarUsuario(GestionUsuarios ventanaGestionUsuarios, ListaSimpleEnlazada lista) : base("Eliminar Usuario")
    {
        this.ventanaGestionUsuarios = ventanaGestionUsuarios;
        this.usuarios = lista;
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Table table = new Table(4, 3, false);
        table.ColumnSpacing = 5;
        table.RowSpacing = 5;

        Label idLabel = new Label("Id");
        entradaIdBusqueda = new Entry();
        Button buscarButton = new Button("Buscar");

        Label nombresLabel = new Label("Nombres");
        entradaNombres = new Entry();

        Label apellidosLabel = new Label("Apellidos");
        entradaApellidos = new Entry();

        Label correoLabel = new Label("Correo");
        entradaCorreo = new Entry();

        Button eliminarButton = new Button("Eliminar");

        Button RegresarButton = new Button("Regresar");

        table.Attach(idLabel, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaIdBusqueda, 1, 2, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(buscarButton, 2, 3, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(nombresLabel, 0, 1, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaNombres, 1, 2, 1, 2, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(apellidosLabel, 0, 1, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaApellidos, 1, 2, 2, 3, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(correoLabel, 0, 1, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);
        table.Attach(entradaCorreo, 1, 2, 3, 4, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(eliminarButton, 1, 2, 4, 5, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        table.Attach(RegresarButton, 2, 3, 4, 5, AttachOptions.Fill, AttachOptions.Fill, 0, 0);

        Add(table);

        eliminarButton.Clicked += OnBotonEliminarClicked;
        buscarButton.Clicked += OnBotonBuscarClicked;
        RegresarButton.Clicked += OnBotonRegresarClicked;

        ShowAll();
    }
    public void OnBotonBuscarClicked(object sender, EventArgs a)
    {
        if (int.TryParse(entradaIdBusqueda.Text, out int idBuscado))
        {
            var usuarioEncontrado = usuarios.BuscarUsuario(idBuscado);

            if (usuarioEncontrado == true)
            {
                entradaNombres.Text = usuarios.BuscarNombresPorId(idBuscado);
                entradaApellidos.Text = usuarios.BuscarApellidosPorId(idBuscado);
                entradaCorreo.Text = usuarios.BuscarCorreoPorId(idBuscado);
            }
            else
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "Usuario no encontrado.");
                md.Run();
                md.Destroy();
                LimpiarCajasDeTexto();
            }
        }
        else
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Ok, "ID invalido.");
            md.Run();
            md.Destroy();
            LimpiarCajasDeTexto();
        }
    }

    private void LimpiarCajasDeTexto()
    {
        entradaIdBusqueda.Text = "";
        entradaNombres.Text = "";
        entradaApellidos.Text = "";
        entradaCorreo.Text = "";
    }

    private void OnBotonEliminarClicked(object sender, EventArgs a)
    {
        string id = entradaIdBusqueda.Text;
        int id_int = int.Parse(id);
        usuarios.EliminarUsuario(id_int);
        string codigoGraphviz = usuarios.GenerarCodigoGraphviz();
        System.IO.File.WriteAllText("lista_usuarios.dot", codigoGraphviz);
        GenerarYMostrarImagenGraphviz("lista_usuarios.dot", "lista_usuarios.png");
        LimpiarCajasDeTexto();
    }

    private void GenerarYMostrarImagenGraphviz(string v1, string v2)
    {
        throw new NotImplementedException();
    }

    private void OnBotonRegresarClicked(object sender, EventArgs a)
    {
        ventanaGestionUsuarios.Show();
        this.Destroy();
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}

// VerUsuario
public class VerUsuarios : Gtk.Window
{
    private TextView usuariosTextView;

    private GestionUsuarios ventanaGestionUsuarios;
    public VerUsuarios(GestionUsuarios ventanaGestionUsuarios, ListaSimpleEnlazada lista) : base("Ver Usuarios Registrados")
    {   
        this.ventanaGestionUsuarios = ventanaGestionUsuarios;
        SetDefaultSize(400,500);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        vbox.BorderWidth = 10;

        
        usuariosTextView = new TextView();
        usuariosTextView.Editable = false; 
        ScrolledWindow scrolledWindow = new ScrolledWindow();
        scrolledWindow.Add(usuariosTextView);
        scrolledWindow.ShadowType = ShadowType.In;
        scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic); 

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += OnRegresarClicked;
        
        vbox.PackStart(scrolledWindow, true, true, 0); 
        vbox.PackStart(regresarButton, false, false, 0);
        
        
        Add(vbox);

        
        Resizable = true;
        WindowPosition = WindowPosition.Center;
        DeleteEvent += new DeleteEventHandler(OnDeleteEvent);

        OnMostrarUsuarios(this, lista);

        ShowAll();

    }

    protected void OnMostrarUsuarios(object sender, ListaSimpleEnlazada lista)
    {
        TextBuffer buffer = usuariosTextView.Buffer;
        string dato = lista.MostrarUsuariosComoString();
        buffer.Text = ""; 
         try
        {
            string[] bloquesUsuario = Regex.Split(dato, @"}(?=\s*{)");

            foreach (string bloqueUsuario in bloquesUsuario)
            {
                if (!string.IsNullOrWhiteSpace(bloqueUsuario))
                {
                    int id = ExtraerValor(bloqueUsuario, "\"ID\":");
                    string nombres = ExtraerValorString(bloqueUsuario, "\"Nombres\":");
                    string apellidos = ExtraerValorString(bloqueUsuario, "\"Apellidos\":");
                    string correo = ExtraerValorString(bloqueUsuario, "\"Correo\":");
                    string contrasenia = ExtraerValorString(bloqueUsuario, "\"Contrasenia\":");

                    buffer.Text += $"\"ID\": {id}\n";
                    buffer.Text += $"\"Nombres\": \"{nombres}\"\n";
                    buffer.Text += $"\"Apellidos\": \"{apellidos}\"\n";
                    buffer.Text += $"\"Correo\": \"{correo}\"\n";
                    buffer.Text += $"\"Contrasenia\": \"{contrasenia}\"\n";
                    buffer.Text += "\n"; 
                }
            }
        }
        catch (Exception ex)
        {
            buffer.Text = $"Error al procesar la cadena: {ex.Message}";
        }
    }

    public int ExtraerValor(string bloqueUsuario, string clave)
    {
        string patron = clave + @"\s*(\d+)";
        Match match = Regex.Match(bloqueUsuario, patron);

        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        else
        {
            return 0;
        }
    }

    public string ExtraerValorString(string bloqueUsuario, string clave)
    {
        string patron = clave + @"\s*""([^""]*)""";
        Match match = Regex.Match(bloqueUsuario, patron);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        else
        {
            return "";
      
        }
    }
    protected void OnRegresarClicked(object sender, System.EventArgs e)
    {   
        ventanaGestionUsuarios.Show();
        this.Hide();   
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    
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