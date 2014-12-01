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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using Tarea_4_back;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Tarea_4
{
    /// <summary>
    /// Interaction logic for juego.xaml. Constructor de la ventana
    /// </summary>
    public partial class juego : Window
    {
        //Variables gráficas
        bool animado;
        bool tipo;
        //Variables del tablero
        Mapa map;
        Rectangle[,] grilla;
        List<int[]> highsc;
        //Variables de flujo
        bool newhigh = false;
        int highscore = 0;
        int[,] repr;
        Stack<int[,]> stackmapas = new Stack<int[,]>();
        private int undosRestantes;
        bool undo = false;
        string origenImagenes;
        int target = 2048;
        //Variables de drag del ratón
        bool puedeDrag = false;
        bool indrag = false;
        int[] pOrigen = { 0, 0 };
        //otros
        Random rnd = new Random();
        /// <summary>
        /// Inicializa una ventana del tipo juego
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="tipo"></param>
        public juego(Mapa mp,bool tipo, bool animado, int undos, List<int[]> highsc)
        {
            this.highsc = highsc;
            this.undosRestantes = undos;
            this.tipo = tipo;
            this.animado = animado;
            if (!tipo)
            {
                origenImagenes = "numeros/";
            }
            else
            {
                origenImagenes = "rostros/";
            }

            InitializeComponent();
            Salirbutton.Click += Salir;
            map = mp;
            int tam = map.Size;
            bool hiexiste = false;
            foreach (int[] puntaje in highsc)
            {
                if (puntaje[0] == tam)
                {
                    this.highscore = puntaje[1];
                    Highscore.Content = this.highscore;
                    hiexiste = true;
                }
            }
            if (!hiexiste)
            {
                int[] data = { tam, 0 };
                highsc.Add(data);
            }
            grilla=new Rectangle[tam,tam];
            //Cada cuadro mide 32
            //Se crea de inmediato el canvas mayor
            MasterCanvas.Height = (10 + tam * 32 + tam * 5+5);
            MasterCanvas.Width = (10 + tam * 32 + tam * 5+5);
            repr = map.mapaEnNumeros();
            dibujar();
            this.stackmapas = new Stack<int[,]>();
            KeyDown += juego_KeyDown;
            //
            this.Height = MasterCanvas.Height+56;
            this.Width = MasterCanvas.Width + MasterGrid.Width;
            MasterGrid.Margin = new Thickness(Convert.ToInt32(MasterCanvas.Width), 0, 0, 0);
            borde.Height = MasterCanvas.Height;
            borde.Width = MasterCanvas.Width;
            MasterGrid.Height = MasterCanvas.Height;
            MasterGrid.Width = 200;
            MasterCanvas.MouseEnter += MasterCanvas_MouseEnter;
            MasterCanvas.MouseLeave += MasterCanvas_MouseLeave;
            MasterCanvas.MouseLeftButtonDown += MasterCanvas_MouseLeftButtonDown;
            MasterCanvas.MouseLeftButtonUp += MasterCanvas_MouseLeftButtonUp;
            this.Closed += juego_Closed;
        }

        private void juego_Closed(object sender, EventArgs e)
        {
            foreach (int[] puntaje in highsc)
            {
                if (puntaje[0] == map.Size)
                {
                    puntaje[1] = this.highscore;
                }
            }
            XmlSerializer serializador = new XmlSerializer(typeof(List<int[]>));
            FileStream archivo = new FileStream("./2048puntuaciones.xml", FileMode.OpenOrCreate);
            serializador.Serialize(archivo, highsc);
            archivo.Close();
        }
        /// <summary>
        /// Dibuja el objeto map, en el MasterCanvas, mediante su representacion en un arreglo 2D
        /// </summary>
        private void dibujar()
        {
            MasterCanvas.Children.Clear();
            int tam = map.Size;
            grilla = new Rectangle[tam, tam];
            if (!undo)
            {
                stackmapas.Push((int[,])repr.Clone());
                map.addrandom(false);
                map.addrandom(false);
            }
            repr = map.mapaEnNumeros();
            for (int i = 0; i < tam; i++)
            {
                for (int j = 0; j < tam; j++)
                {
                    Rectangle cuad = new Rectangle();
                    cuad.Width = 32;
                    cuad.Height = 32;
                    cuad.SetValue(Canvas.LeftProperty, (double)(10+32*i+5*i));
                    cuad.SetValue(Canvas.TopProperty, (double)(10+32*j+5*j));
                    BitmapImage img = new BitmapImage();
                    int[] sqdata = {repr[i, j],i,j};
                    cuad.Tag = sqdata;
                    if (repr[i,j] == target)
                    {
                        if (target == 2048)
                        {
                            MessageBoxButton botones = MessageBoxButton.YesNo;
                            MessageBoxImage icono = MessageBoxImage.Information;
                            MessageBoxResult respuesta = MessageBox.Show("Has logrado crear un cuadro de numero 2048. ¿Aceptas el desfio de llegar a 4096?", "Error", botones, icono);
                            
                            if (respuesta == MessageBoxResult.No || respuesta == MessageBoxResult.None)
                            {
                                this.Terminar();
                            }
                            target = 4096;
                        }
                        else if (target == 4096)
                        {
                            MessageBoxButton botones = MessageBoxButton.OK;
                            MessageBoxImage icono = MessageBoxImage.Information;
                            MessageBoxResult respuesta = new MessageBoxResult();
                            MessageBox.Show("¡Has logrado el desafío final!", "Éxito", botones, icono, respuesta);
                            this.Terminar();
                        }
                    }
                    img.BeginInit();
                    img.UriSource = new Uri("pack://application:,,,/Recursos/"+ origenImagenes + repr[i, j] + ".png", UriKind.Absolute);
                    img.EndInit();
                    cuad.Fill = new ImageBrush(img);
                    cuad.IsMouseDirectlyOverChanged += cuad_IsMouseDirectlyOverChanged;
                    MasterCanvas.Children.Add(cuad);
                    grilla[i, j] = cuad;
                }
            }
        }
        /// <summary>
        /// Asociado al botón salir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Salir(object sender, RoutedEventArgs e)
        {
            this.Terminar();
        }
        /// <summary>
        /// Cierra la ventana
        /// </summary>
        private void Terminar()
        {
            
            this.Close();
        }
        /// <summary>
        /// Asociado a la pulsacion de una tecla. Se gatillan eventos en el backend independiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void juego_KeyDown(object sender, KeyEventArgs e)
        {
            if (undo) undo = false;
            int[][] dat;
            Storyboard sb = new Storyboard();
            if (e.Key == Key.Down)
            {
                dat = map.Mover(0);
            }
            else if (e.Key == Key.Left)
            {
                dat = map.Mover(3);
            }
            else if (e.Key == Key.Up)
            {
                dat = map.Mover(2);
            }
            else if (e.Key == Key.Right)
            {
                dat = map.Mover(1);
            }
            Puntaje.Content = map.Puntaje;
            if (map.Puntaje > highscore)
            {
                highscore = map.Puntaje;
                Highscore.Content = map.Puntaje;
            }
            int tam = map.Size;
            dibujar();
            if (this.map.noMovs())
            {
                MessageBoxButton botones = MessageBoxButton.OK;
                MessageBoxImage icono = MessageBoxImage.Information;
                MessageBox.Show("Has sido derrotado", "Lástima", botones, icono);
                this.Terminar();
            }
        }
        /// <summary>
        /// Cambia la imagen de un rectangulo, segun su valor
        /// </summary>
        private void cuad_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Rectangle culp = (Rectangle) sender;
            int[] info = (int[])culp.Tag;
            if(culp.IsMouseOver)
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/Recursos/" + origenImagenes + info[0] + "alt.png", UriKind.Absolute);
                img.EndInit();
                culp.Fill = new ImageBrush(img);
            }
            else
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/Recursos/" + origenImagenes + info[0] + ".png", UriKind.Absolute);
                img.EndInit();
                culp.Fill = new ImageBrush(img);
            }
        }
        /// <summary>
        /// Guarda una instancia del objeto mapa actual, para posterior uso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.CreateDirectory("./Save");
            Microsoft.Win32.SaveFileDialog dialogoSalvar = new Microsoft.Win32.SaveFileDialog();
            dialogoSalvar.FileName = "juego";
            dialogoSalvar.DefaultExt = ".2ks";
            dialogoSalvar.Filter = "2048 Tableros |*.2ks";
            Nullable<bool> result = dialogoSalvar.ShowDialog();
            if (result == true)
            {
                string nombreFile = dialogoSalvar.FileName;
                BinaryFormatter binary = new BinaryFormatter();
                FileStream savfile = new FileStream(nombreFile, FileMode.Create);
                binary.Serialize(savfile, this.map);
                savfile.Close();
            }
        }
        /// <summary>
        /// Usando un stack ya definido, deshace las acciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (stackmapas.Count != 0 && undosRestantes>0)
            {
                undosRestantes--;
                int[,] mapatmp = stackmapas.Pop();
                this.map.numerosEnMapa(mapatmp);
                undo = true;
                this.dibujar();
                Undo.Content = "Deshacer (" + undosRestantes + ")";
                if (undosRestantes == 0)
                {
                    Undo.Content = "Agotado";
                }
            }
            else
            {
                undo = false;
            }
        }
        // Funciones que se ocupan del drag para mover
        private void MasterCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (indrag)
            {
                Point p = Mouse.GetPosition(this);
                int[] pi = pOrigen;
                int[] pf = { Convert.ToInt32(p.X), Convert.ToInt32(p.Y) };
                int[] vector = { (pf[0] - pi[0]), (pf[1] - pi[1]) };
                string mover = "";
                if (vector[0] >= 0 && Math.Abs(vector[0]) >= Math.Abs(vector[1])) //Derecha
                {
                    mover = "der";
                }
                else if (vector[0] <= 0 && Math.Abs(vector[0]) >= Math.Abs(vector[1])) //izquierda
                {
                    mover = "izq";
                }
                else if (vector[1] >= 0 && Math.Abs(vector[1]) >= Math.Abs(vector[0])) //abajo
                {
                    mover = "aba";
                }
                else if (vector[1] <= 0 && Math.Abs(vector[1]) >= Math.Abs(vector[0])) //Arriba
                {
                    mover = "arr";
                }
                indrag = false;
                if (undo) undo = false;
                int[][] dat;
                Storyboard sb = new Storyboard();
                if (mover == "aba")
                {
                    dat = map.Mover(0);
                }
                else if (mover == "izq")
                {
                    dat = map.Mover(3);
                }
                else if (mover == "arr")
                {
                    dat = map.Mover(2);
                }
                else if (mover == "der")
                {
                    dat = map.Mover(1);
                }
                if (mover != "")
                {
                    Puntaje.Content = map.Puntaje;
                    int tam = map.Size;
                    dibujar();
                    if (this.map.noMovs())
                    {
                        MessageBoxButton botones = MessageBoxButton.OK;
                        MessageBoxImage icono = MessageBoxImage.Information;
                        MessageBox.Show("Has sido derrotado", "Lástima", botones, icono);
                        this.Terminar();
                    }
                    if (map.Puntaje > highscore)
                    {
                        highscore = map.Puntaje;
                        Highscore.Content = map.Puntaje;
                        newhigh = true;
                    }
                }
            }
        }
        private void MasterCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (puedeDrag)
            {
                Point p = Mouse.GetPosition(MasterCanvas);
                int[] dem = { Convert.ToInt32(p.X), Convert.ToInt32(p.Y) };
                deb.Content = "" + dem[0] + dem[1];
                pOrigen = dem;
                indrag = true;
            }
        }
        private void MasterCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            puedeDrag = false;
        }
        private void MasterCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            puedeDrag = true;
        }
        //fin
    }
}


//Codigo no utilizado
//foreach (int[] data in dat)
//{
//    Rectangle tmp = grilla[data[0], data[1]];
//    tmp.RenderTransform = new TranslateTransform();
//    DoubleAnimation da = new DoubleAnimation(tmp.Margin.Top, tmp.Margin.Top + 37, new Duration(new TimeSpan(0, 0, 0, 1, 0)));
//    Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
//    sb.Children.Add(da);
//    DoubleAnimation daret = new DoubleAnimation(tmp.Margin.Top, tmp.Margin.Top - 37, new Duration(new TimeSpan(0, 0, 0, 1, 0)));
//    Storyboard.SetTargetProperty(daret, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
//    sb.Children.Add(daret);
//    tmp.BeginStoryboard(sb);
//    Rectangle ctmp = grilla[data[2], data[3]];
//    tmp.Fill = new SolidColorBrush(Colors.BurlyWood);
//    ctmp.Fill = new SolidColorBrush(Colors.Red);

//}

//Rectangle rect = new Rectangle();
//rect.Fill = new SolidColorBrush(Colors.Aqua);
//TransformGroup group = new TransformGroup();
//grillaJuego.Children.Add(rect);
//Grid.SetColumn(rect, 1);
//for (int i = 0; i < 10; i++)
//{
//    group.Children.Add(new TranslateTransform(rnd.Next(0,10),rnd.Next(0,10)));
//    rect.RenderTransform = group;
//}

//---------------- CLAVE

//testrect.RenderTransform = new TranslateTransform();
//testrect2.RenderTransform = new TranslateTransform();
//Storyboard sb = new Storyboard();
//DoubleAnimation da = new DoubleAnimation(testrect.Margin.Top, testrect.Margin.Top+42, new Duration(new TimeSpan(0, 0, 1)));
//DoubleAnimation da1 = new DoubleAnimation(testrect2.Margin.Top, testrect2.Margin.Top+42, new Duration(new TimeSpan(0, 0, 1)));
////DoubleAnimation da1 = new DoubleAnimation(testrect.Margin.Left, testrect.Margin.Left, new Duration(new TimeSpan(0, 0, 1)));
//Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
//Storyboard.SetTargetProperty(da1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
////Storyboard.SetTargetProperty(da1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
//sb.Children.Add(da);
//sb.Children.Add(da1);
//testrect.BeginStoryboard(sb);
//testrect2.BeginStoryboard(sb);
//sb.Begin(this);
//-------


//DoubleAnimation myDoubleAnimation = new DoubleAnimation();
//myDoubleAnimation.From = 10;
//myDoubleAnimation.To = 1000;
//myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
//storyboard = new Storyboard();
//storyboard.Children.Add(myDoubleAnimation);
//Storyboard.SetTargetName(myDoubleAnimation, testrect.Name);
//Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.XProperty)"));

//storyboard.Begin();
/*
                foreach (int[] data in dat)
                {
                    Rectangle tmp = grilla[data[0], data[1]];
                    Rectangle ctmp = grilla[data[2], data[3]];
                    int[] info = (int[])tmp.Tag;
                    int[] cinfo = (int[])ctmp.Tag;
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri("pack://application:,,,/Recursos/" + origenImagenes + info[0] + "f.png", UriKind.Absolute);
                    img.EndInit();
                    tmp.Fill = new ImageBrush(img);
                    BitmapImage cimg = new BitmapImage();
                    cimg.BeginInit();
                    cimg.UriSource = new Uri("pack://application:,,,/Recursos/" + origenImagenes + cinfo[0] + "f.png", UriKind.Absolute);
                    cimg.EndInit();
                    tmp.Fill = new ImageBrush(cimg);
                    tmp.RenderTransform = new TranslateTransform(); //Creamos el objeto a animar
                    DoubleAnimation damov = new DoubleAnimation(tmp.Margin.Top, tmp.Margin.Top - 37, new Duration(new TimeSpan(0, 0, 0, 1))); //Ejecutamos la animacion
                    DoubleAnimation redamov = new DoubleAnimation(tmp.Margin.Top, tmp.Margin.Top + 37, new Duration(new TimeSpan(0, 0, 0, 0, 1))); //Ejecutamos la animacion
                    //damov.Completed += damov_Completed;
                    damov.BeginTime = TimeSpan.FromSeconds(t);
                    t++;
                    Storyboard.SetTargetProperty(damov, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                     //Creamos el objeto a animar
                    redamov.Completed += damov_Completed;
                    redamov.BeginTime = TimeSpan.FromSeconds(t);
                    Storyboard.SetTargetProperty(redamov, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                    sb.Children.Add(damov);
                    sb.Children.Add(redamov);
                    tmp.BeginStoryboard(sb);
                    //no Actualizamos el valor del otro rectangulo pues se dibujara de nuevo
                }
                 */

