using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using Tarea_6_Backend;

namespace Tarea_6
{
    public enum modoCursor
    {
        normal, poligono, pincel, imagen, linea
    }
    public class pincelElem
    {
        public int grosor = 1;
        public Color color = new Color();
        public Color relleno = new Color();
    }
    
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : Window
    {
        const int alto = 655;
        const int largo = 800;
        public Cliente clienteAsociado;
        private const int dpi = 96;
        List<Canvas> Capas = new List<Canvas>();
        List<int> Capascount = new List<int>();
        int capaActual = 0;
        modoCursor cursorActual = modoCursor.normal;
        bool cursorActivo = false;
        UIElement elementoActivo = null;
        pincelElem pincel = new pincelElem();
        int largoImg = 100;
        int altoImg = 100;
        Imagen imagenActiva;
        Color colorChat;
        Random rnd = new Random();
        bool introImg = true;

        public Editor(Cliente cliente)
        {
            InitializeComponent();
            //Zona Networking
            colorChat = new Color();
            colorChat.R = (byte)rnd.Next(0, 255);
            colorChat.B = (byte)(255-colorChat.R);
            colorChat.G = (byte)rnd.Next(0,255);
            colorChat.A = 255;
            clienteAsociado = cliente;
            clienteAsociado.pushRecibido += clienteAsociado_pushRecibido;
            clienteAsociado.ConexionPerdida += clienteAsociado_ConexionPerdida;
            clienteAsociado.editorIniciado.Set();
            this.Closed += Editor_Closed;
            capaActual = 0;
            //Inscripcion de los botones
            //Chat
            chatEnviar.Click+=chatEnviar_Click;
            saveArea.Click += saveArea_Click;
            //Interaccion con el area de trabajo
            MasterGrid.MouseDown += areaDibujo_MouseDown;
            MasterGrid.MouseMove += areaDibujo_MouseMove;
            MasterGrid.MouseUp += areaDibujo_MouseUp;
            //Botones de la barra
            addCapaBot.Click += addCapaBot_Click; //Añadir capa
            addImagenBot.Click += addImagenBot_Click; //Añadir imagen a la capa actual
            crearPoligono.Click += crearPoligono_Click;
            norm.Click += norm_Click;
            listaCapas.SelectionChanged += listaCapas_SelectionChanged; //Seleccionar otra capa
            elimCapaBot.Click += elimCapaBot_Click; //Eliminar capa actual
            crearPincel.Click += CrearPincel_Click;
            crearLinea.Click += crearLinea_Click;
            //Control de color
            selectorColorRed.TextChanged += localTextChanged;
            selectorColorGreen.TextChanged += localTextChanged;
            selectorColorBlue.TextChanged += localTextChanged;
            selectorColorAlpha.TextChanged += localTextChanged;
            selectorLinewidth.TextChanged += selectorLinewidth_TextChanged;
            //Control de imagen
            altoInput.TextChanged += altoInput_TextChanged;
            largoInput.TextChanged += largoInput_TextChanged;

            listaCapas.SelectedIndex = 0;
            pincel.color.A = 255;
            pincel.color.B = 0;
            pincel.color.G = 0;
            pincel.color.R = 0;
        }

        void altoInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int tryint = int.Parse(altoInput.Text);
                if (tryint > alto - 1)
                {
                    MessageBox.Show("Error, el parámetro no debe superar el valo r" + (alto - 1), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    altoInput.Text = "100";
                }
                else
                {
                    altoImg = tryint;
                }
            }
            catch
            {
                MessageBox.Show("Error, el parámetro debe ser un número", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void largoInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int tryint = int.Parse(largoInput.Text);
                if (tryint > largo - 1)
                {
                    MessageBox.Show("Error, el parámetro no debe superar el valor " + (largo - 1), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    largoInput.Text = "100";
                }
                else
                {
                    largoImg = tryint;
                }
            }
            catch
            {
                MessageBox.Show("Error, el parámetro debe ser un número", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Funciones de networking
        void clienteAsociado_ConexionPerdida()
        {
            Dispatcher.BeginInvoke(new Action(() =>
                    {
                if (this.IsActive)
                {
                    MessageBoxResult res = new MessageBoxResult();
                    
                    MessageBox.Show("Advertencia: Se perdió la conexión " + '\n' + "¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, res, MessageBoxOptions.None);
                    
                    if (res == MessageBoxResult.Yes)
                    {

                    }
                    else if (res == MessageBoxResult.None)
                    {
                        //Dispatcher.BeginInvoke(new Action(() =>
                        //{
                            this.Close();
                        //}));
                    }
                }
                    }));
        }
        void clienteAsociado_pushRecibido(PaqueteEdit msg)
        {
            if (msg.Tipo == tipoMensaje.Chat)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ListBoxItem item = new ListBoxItem();
                    Color col = new Color();
                    col.A = msg.A;
                    col.B = msg.B;
                    col.R = msg.R;
                    col.G = msg.G;
                    item.Foreground = new SolidColorBrush(col);
                    item.Content = msg.Firma + " dice: " + (string)msg.obj + '\n';
                    chatView.Items.Add(item);
                }));
            }
            if (msg.Tipo == tipoMensaje.Linea)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Line l = new Line();
                    l.X1 = (double)msg.obj;
                    l.X2 = (double)msg.obj1;
                    l.Y1 = (double)msg.obj2;
                    l.Y2 = (double)msg.obj3;
                    l.StrokeThickness = (double)msg.obj4;
                    Color ctmp= new Color();
                    ctmp.A = msg.A;
                    ctmp.R = msg.R;
                    ctmp.G = msg.G;
                    ctmp.B = msg.B;  
                    l.Stroke = new SolidColorBrush(ctmp);
                    Capas[(int)msg.obj5].Children.Add(l);
                    
                }));
            }
            if (msg.Tipo == tipoMensaje.Rectangulo)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Canvas rect = new Canvas();
                    rect.Height = (double)msg.obj;
                    rect.Width = (double)msg.obj1;
                    Canvas.SetLeft(rect, (double)msg.obj2);
                    Canvas.SetTop(rect, (double)msg.obj3);
                    Color ctmp = new Color();
                    ctmp.A = msg.A;
                    ctmp.R = msg.R;
                    ctmp.G = msg.G;
                    ctmp.B = msg.B;
                    rect.Background = new SolidColorBrush(ctmp);
                    //
                    Capas[(int)msg.obj4].Children.Add(rect);
                }));
            }
            if (msg.Tipo == tipoMensaje.AddCapa)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    string name = msg.obj as string;
                    listaCapas.Items.Add(name);
                    Canvas tmpc = new Canvas();
                    tmpc.Tag = name;
                    tmpc.Width = 800;
                    tmpc.Height = 655;
                    areaDibujo.Children.Add(tmpc);
                    Capas.Add(tmpc);
                }));
            }
            if (msg.Tipo==tipoMensaje.Imagen)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Imagen img = new Imagen((Stream)msg.obj,(string)msg.obj5);
                    Point pt = new Point((double)msg.obj3, (double)msg.obj4);
                    img.inicializar((int)msg.obj1, (int)msg.obj2, pt);
                    Capas[(int)msg.obj6].Children.Add(img);
                    //PaqueteEdit pq = new PaqueteEdit(tipoMensaje.Imagen);
                    //pq.obj = img.mem;
                    //pq.obj1 = largoImg;
                    //pq.obj2 = altoImg;
                    //pq.obj3 = pt.X;
                    //pq.obj4 = pt.Y;
                    //clienteAsociado.enviarMensaje(pq);
                }));
            }
            if (msg.Tipo == tipoMensaje.ElimCapa)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    string MFD = msg.obj as string;
                    int i = 0;
                    int target = 0;
                    foreach (Canvas child0 in Capas)
                    {
                        if ((string)child0.Tag == MFD)
                        {
                            target = i;
                        }
                        i++;
                    }
                    Capas.RemoveAt(target);
                    i = 0;
                    target = 0;
                    foreach (object child in areaDibujo.Children)
                    {
                        if (child is Canvas)
                        {
                            Canvas ctmp = child as Canvas;
                            if ((string)ctmp.Tag == (string)MFD)
                            {
                                target = i;
                            }
                        }
                        i++;
                    }
                    areaDibujo.Children.RemoveAt(target);
                    i = 0;
                    target = 0;
                    foreach (string child in listaCapas.Items)
                    {
                        if (child == (string)MFD)
                        {
                            target = i;
                        }
                        i++;
                    }
                    listaCapas.Items.RemoveAt(target);
                    listaCapas.SelectedIndex = 0;
                    capaActual = 0;
                    
                }));
            }
            
        }
        void Editor_Closed(object sender, EventArgs e)
        {
            this.clienteAsociado.eliminar();
        }

        //Evento de cambio 
        private void localTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textSender = sender as TextBox;
            byte nvalor = 0;
            Color colorActual = pincel.color;
            try
            {
                nvalor = byte.Parse(textSender.Text);
            }
            catch
            {
                MessageBox.Show("Valor erroneo");
                switch (textSender.Name)
                {
                    case ("selectorColorAlpha"):
                        textSender.Text = colorActual.A.ToString();
                        break;
                    case ("selectorColorRed"):
                        textSender.Text = colorActual.R.ToString();
                        break;
                    case ("selectorColorGreen"):
                        textSender.Text = colorActual.G.ToString();
                        break;
                    case ("selectorColorBlue"):
                        textSender.Text = colorActual.B.ToString();
                        break;
                }
                return;
            }
            int vimp = nvalor;
            switch(textSender.Name)
            {
                case ("selectorColorAlpha"):
                    pincel.color.A = nvalor;
                    textSender.Text = nvalor.ToString();
                    break;
                case ("selectorColorRed"):
                    pincel.color.R = nvalor;
                    textSender.Text = nvalor.ToString();
                    break;
                case ("selectorColorGreen"):
                    pincel.color.G = nvalor;
                    textSender.Text = nvalor.ToString();
                    break;
                case ("selectorColorBlue"):
                    pincel.color.B = nvalor;
                    textSender.Text = nvalor.ToString();
                    break;
            }
            selectorColorProbe.Fill = new SolidColorBrush(pincel.color);// new SolidColorBrush(colorActual);


        }

        //Botones
        void selectorLinewidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox input = sender as TextBox;
            bool formatoOK = false;
            int valor = 1;
            try
            {
                valor = Int32.Parse(input.Text);
                if (valor < 1 || valor > 10)
                {
                    throw new FormatException();
                }
                formatoOK = true;
            }
            catch (Exception)
            {
                input.Text = "";
            }
            if (formatoOK)
	        {
		       selectorLineExample.Width = valor;
               selectorLineExample.Height = valor;
               pincel.grosor = valor;
	        }
        }
        void norm_Click(object sender, RoutedEventArgs e)
        {
            elementoActivo = null;
            cursorActivo = false;
            cursorActual = modoCursor.normal;
        }
        void crearPoligono_Click(object sender, RoutedEventArgs e)
        {
            cursorActual = modoCursor.poligono;
        }
        void crearLinea_Click(object sender, RoutedEventArgs e)
        {
            cursorActual = modoCursor.linea;
        }
        void CrearPincel_Click(object sender, RoutedEventArgs e)
        {
            cursorActual = modoCursor.pincel;
        }
        void addCapaBot_Click(object sender, RoutedEventArgs e)
        {
            string tag = "Capa @ " + DateTime.Now.Hour+ ':' + DateTime.Now.Minute + ':' + DateTime.Now.Second;
            listaCapas.Items.Add(tag);
            Canvas tmpc = new Canvas();
            tmpc.Tag = tag;
            tmpc.Width = 800;
            tmpc.Height = 655;
            areaDibujo.Children.Add(tmpc);
            //MasterGrid.Children.Add(tmpc);
            //Grid.SetColumn(tmpc, 1);
            //Grid.SetRow(tmpc, 1);
            Capas.Add(tmpc);
            PaqueteEdit pq = new PaqueteEdit(tipoMensaje.AddCapa);
            pq.obj = tmpc.Tag;
            clienteAsociado.enviarMensaje(pq);
            //enviarObjeto(tmpc, null, tipoMensaje.AddCapa);
        } 
        void elimCapaBot_Click(object sender, RoutedEventArgs e)
        {
            if (Capas.Count != 1)
            {
                Canvas MFD = Capas[capaActual];
                Capas.RemoveAt(capaActual);
                int i = 0;
                int target = 0;
                foreach (object child in areaDibujo.Children)
                {
                    if (child is Canvas)
                    {
                        Canvas ctmp = child as Canvas;
                        if ((string)ctmp.Tag == (string)MFD.Tag)
                        {
                            target = i;
                        }
                    }
                    i++;
                }
                areaDibujo.Children.RemoveAt(target);
                i = 0;
                target = 0;
                foreach (string child1 in listaCapas.Items)
                {
                    if (child1 == (string)MFD.Tag)
                    {
                        target = i;
                    }
                    i++;
                }
                listaCapas.Items.RemoveAt(target);
                PaqueteEdit pq = new PaqueteEdit(tipoMensaje.ElimCapa);
                pq.obj = MFD.Tag;
                pq.obj1 = capaActual;
                clienteAsociado.enviarMensaje(pq);
                listaCapas.SelectedIndex = 0;
                capaActual = 0;
                
            }
        } //Pendiente
        void addImagenBot_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opener = new Microsoft.Win32.OpenFileDialog();
            opener.Filter = "Imagenes JPG, PNG, BMP|*.jpg;*.png;*.bmp";
            opener.Multiselect = false;
            Nullable<bool> result = opener.ShowDialog();
            if ((bool)result)
            {
                Imagen img = new Imagen(opener.FileName, opener.FileName.Substring(opener.FileName.Length - 3));
                if (img.Exito)
                {
                    if (introImg)
                    {
                        MessageBox.Show("Para añadir la imagen, haz click en el punto " + '\n' + "en que quieras agregarla", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        introImg = false;
                    }
                    cursorActual = modoCursor.imagen;
                    imagenActiva = img;
                    cursorActivo = true;
                }
                else
                {
                    img = null;
                    cursorActual = modoCursor.normal;
                    MessageBox.Show("Error : La imagen no pudo abrirse."+'\n'+"Suele pasar con imagenes jpg","Error",MessageBoxButton.OK);
                }
            }
        }

        void listaCapas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lista = sender as ListBox;
            capaActual = lista.SelectedIndex;
        }

        void areaDibujo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            cursorActivo = false;
            if (elementoActivo != null)
            {
                if (cursorActual == modoCursor.linea || cursorActual==modoCursor.pincel)
                {
                    //Enviando una linea
                    Line l = elementoActivo as Line;
                    PaqueteEdit pq = new PaqueteEdit(tipoMensaje.Linea);
                    //
                    pq.obj = l.X1;
                    pq.obj1 = l.X2;
                    pq.obj2 = l.Y1;
                    pq.obj3 = l.Y2;
                    pq.obj4 = l.StrokeThickness;
                    pq.obj5 = capaActual;
                    pq.A = pincel.color.A;
                    pq.R = pincel.color.R;
                    pq.G = pincel.color.G;
                    pq.B = pincel.color.B;
                    //
                    clienteAsociado.enviarMensaje(pq);
                }
                else if (cursorActual == modoCursor.poligono)
                {
                    Canvas rect = elementoActivo as Canvas;
                    PaqueteEdit pq = new PaqueteEdit(tipoMensaje.Rectangulo);
                    pq.obj = rect.Height;
                    pq.obj1 = rect.Width;
                    pq.obj2 = Canvas.GetLeft(rect);
                    pq.obj3 = Canvas.GetTop(rect);
                    pq.obj4 = capaActual;
                    pq.A = pincel.color.A;
                    pq.R = pincel.color.R;
                    pq.G = pincel.color.G;
                    pq.B = pincel.color.B;
                    //
                    clienteAsociado.enviarMensaje(pq);
                }
            }
            elementoActivo = null;
        }
        void areaDibujo_MouseMove(object sender, MouseEventArgs e)
        {
            Point punto = e.GetPosition(areaDibujo);
            if (inpos(punto, areaDibujo))
            {
                this.Cursor = Cursors.Pen;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
            if (cursorActivo)
            {
                if (!inpos(punto, areaDibujo))
                {
                    areaDibujo_MouseUp(null, null);
                    elementoActivo = null;
                    cursorActivo = false;
                }
                else if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (cursorActual == modoCursor.poligono)
                    {
                        Canvas ea = elementoActivo as Canvas;
                        Point puntoinicial = (Point)ea.Tag;
                        int w = (int)(punto.X-puntoinicial.X);
                        int h = (int)(punto.Y-puntoinicial.Y);
                        if(w>0) ea.Width = w;
                        if(h>0) ea.Height = h;
                    }
                    else if(cursorActual == modoCursor.pincel) //Dibujando
                    {
                        Line la = elementoActivo as Line;
                        Line l = new Line();
                        la.X2 = punto.X;
                        la.Y2 = punto.Y;
                        l.X1 = punto.X;
                        l.Y1 = punto.Y;
                        l.X2 = punto.X;
                        l.Y2 = punto.Y;
                        l.Stroke = new SolidColorBrush(pincel.color);
                        l.StrokeThickness = pincel.grosor;
                        Capas[capaActual].Children.Add(l);
                        elementoActivo = l;
                        PaqueteEdit pq = new PaqueteEdit(tipoMensaje.Linea);
                        //
                        pq.obj = la.X1;
                        pq.obj1 = la.X2;
                        pq.obj2 = la.Y1;
                        pq.obj3 = la.Y2;
                        pq.obj4 = la.StrokeThickness;
                        pq.obj5 = capaActual;
                        pq.A = pincel.color.A;
                        pq.R = pincel.color.R;
                        pq.G = pincel.color.G;
                        pq.B = pincel.color.B;
                        //
                        clienteAsociado.enviarMensaje(pq);
                    }
                    else if (cursorActual==modoCursor.linea)
                    {
                        Line l = elementoActivo as Line;
                        l.X2 = punto.X;
                        l.Y2 = punto.Y;
                    }

                }
            }
        }
        void areaDibujo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point puntoInit = e.GetPosition(areaDibujo);
            if (inpos(puntoInit, areaDibujo))
            {
                if (cursorActual == modoCursor.poligono)
                {
                    Canvas rect = new Canvas();
                    rect.Background = new SolidColorBrush(pincel.color);
                    Canvas.SetLeft(rect, puntoInit.X);
                    Canvas.SetTop(rect, puntoInit.Y);
                    Capas[capaActual].Children.Add(rect);
                    rect.Tag = puntoInit;
                    cursorActivo = true;
                    elementoActivo = rect;
                    //Activar eventos para enviar data
                }
                if (cursorActual == modoCursor.pincel)
                {
                    Line l = new Line();
                    l.Stroke = new SolidColorBrush(pincel.color);
                    l.StrokeThickness = pincel.grosor;
                    l.X1 = puntoInit.X;
                    l.Y1 = puntoInit.Y;
                    l.X2 = puntoInit.X;
                    l.Y2 = puntoInit.Y;
                    Capas[capaActual].Children.Add(l);
                    elementoActivo = l;
                    cursorActivo = true;
                }
                if (cursorActual == modoCursor.imagen)
                {
                    Imagen img = imagenActiva as Imagen;
                    Point pt = e.GetPosition(areaDibujo);
                    if ((pt.X+largoImg)<largo && (pt.Y+altoImg)<alto)
	                {
                        img.inicializar(largoImg,altoImg,pt);
                        Capas[capaActual].Children.Add(img);
                        areaDibujo_MouseUp(null, null);
                        elementoActivo = null;
                        cursorActivo = false;
                        cursorActual = modoCursor.normal;
                        //enviando paquete
                        PaqueteEdit pq = new PaqueteEdit(tipoMensaje.Imagen);
                        pq.obj1 = largoImg;
                        pq.obj2 = altoImg;
                        pq.obj3 = pt.X;
                        pq.obj4 = pt.Y;
                        pq.obj5 = img.extension;
                        pq.obj6 = capaActual;
                        pq.obj = img.mem;
                        clienteAsociado.enviarMensaje(pq);
	                }
                    
                }
                if (cursorActual == modoCursor.linea)
                {
                    Line l = new Line();
                    l.Stroke = new SolidColorBrush(pincel.color);
                    l.StrokeThickness = pincel.grosor;
                    l.X1 = puntoInit.X;
                    l.Y1 = puntoInit.Y;
                    Capas[capaActual].Children.Add(l);
                    elementoActivo = l;
                    cursorActivo = true;
                }
            }
        }

        bool inpos(Point punto, Canvas rel)
        {
            if (punto.X >= 0 && punto.X <= rel.Width && punto.Y >= 0 && punto.Y <= rel.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void chatEnviar_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem();
            item.Foreground = new SolidColorBrush(colorChat);
            item.Content = this.clienteAsociado.Login + " dice: " + chatTexto.Text + '\n';
            chatView.Items.Add(item);
            PaqueteEdit pack = new PaqueteEdit(tipoMensaje.Chat);
            pack.obj = chatTexto.Text;
            pack.A = colorChat.A;
            pack.B = colorChat.B;
            pack.G = colorChat.G;
            pack.R = colorChat.R;
            clienteAsociado.enviarMensaje(pack);
            chatTexto.Text = "";
        }
        void saveArea_Click(object sender, RoutedEventArgs e)
        {
            SaveCanvas(this, Capas[capaActual]);
        }
        /// <summary>
        /// Guarda un canvas de una ventana, con cierta definicion en un archivo
        /// </summary>
        /// <param name="window">Ventana que contiene al canvas</param>
        /// <param name="canvas">Canvas a guardar, con hijos incluidos</param>
        /// <param name="dpi">Densidad de pixeles</param>
        public void SaveCanvas(Window window, Canvas canvas)
        {
            Size size = new Size(window.Width, window.Height);
            canvas.Measure(size);
            //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap((int)window.Width, (int)window.Height, dpi, dpi, PixelFormats.Pbgra32);
            rtb.Render(canvas);


            System.IO.Directory.CreateDirectory("./Save");
            Microsoft.Win32.SaveFileDialog dialogoSalvar = new Microsoft.Win32.SaveFileDialog();
            dialogoSalvar.FileName = "imagen";
            
            dialogoSalvar.Filter = "Imágenes BMP |*.bmp | Imágenes JPG |*.jpg | Imágenes PNG |*.png";
            Nullable<bool> result = dialogoSalvar.ShowDialog();
            if (result == true)
            {
                string nombreFile = dialogoSalvar.FileName;
                string ext = nombreFile.Substring(nombreFile.Length-3);
                if (ext == "bmp")
                {
                    SaveRTBAsBMP(rtb, nombreFile);
                }
                else if (ext == "jpg")
                {
                    SaveRTBAsJPG(rtb, nombreFile);
                }
                else if (ext == "png")
                {
                    SaveRTBAsPNG(rtb, nombreFile);
                }
                SaveRTBAsPNG(rtb, nombreFile);
            }
        }
        private void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
        private void SaveRTBAsBMP(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = File.Create(filename))
            {
                enc.Save(stm);
            }
        }
        private void SaveRTBAsJPG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));
            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}
