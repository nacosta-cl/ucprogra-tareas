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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tarea_5_Backend;

namespace Tarea_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Mapa map;
        public Canvas[,] cancha = new Canvas[50, 30];
        private int rGoles = 0;
        private int aGoles = 0;
        private int agoles
        {
            get {return this.aGoles;}
            set {
                pAzul.Content = value.ToString();
                this.aGoles = value;
            }
        }
        private int rgoles
        {
            get {return this.rGoles;}
            set {
                pRojo.Content = value.ToString();
                this.rGoles = value;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown; //Evento de movimiento
            for (int i = 0; i < 30; i++)
            {
                RowDefinition nfil = new RowDefinition();
                nfil.Height = new GridLength(20);
                GrillaCancha.RowDefinitions.Add(nfil);
            }
            for (int i = 0; i < 50; i++)
            {
                ColumnDefinition ncol = new ColumnDefinition();
                ncol.Width = new GridLength(20);
                GrillaCancha.ColumnDefinitions.Add(ncol);
            }
            initMap();
            Border bordetotal = new Border();
            bordetotal.BorderThickness = new Thickness(1.0);
            bordetotal.BorderBrush = new SolidColorBrush(Colors.Black);
            Grid.SetColumn(bordetotal, 0);
            MasterGrid.Children.Add(bordetotal);
            GrillaCancha.ShowGridLines = false;
        }
        private void initMap()
        {
            map = new Mapa();
            Entidad[,] mapi = map.Cancha;
            GrillaCancha.Children.Clear();
            for (int j = 0; j < 30; j++)
            {
                Canvas c = new Canvas();
                GrillaCancha.Children.Add(c);
                Grid.SetRow(c, j);
                Grid.SetColumn(c, 0);
                if (13 < j && j < 18)
                {
                    c.Background = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    c.Background = new SolidColorBrush(Colors.Black);
                }
                cancha[0, j] = c;

                Canvas c2 = new Canvas();
                GrillaCancha.Children.Add(c2);
                Grid.SetRow(c2, j);
                Grid.SetColumn(c2, 49);
                if (13 < j && j < 18)
                {
                    c2.Background = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    c2.Background = new SolidColorBrush(Colors.Black);
                }
                cancha[0, j] = c2;
            }
            for (int j = 0; j < mapi.GetLength(1); j++)
            {
                for (int i = 0; i < mapi.GetLength(0); i++)
                {
                    Entidad sw = mapi[i,j];
                    if (sw is Jugador)
                    {
                        Canvas ctmp = new Canvas();
                        cancha[i, j] = ctmp;
                        if (sw.Equipo == equipo.azul)
                        {
                            BitmapImage img = new BitmapImage();
                            img.BeginInit();
                            img.UriSource = new Uri("pack://application:,,,/res/bFarmer.png", UriKind.Absolute);
                            img.EndInit();
                            ctmp.Background = new ImageBrush(img);
                        }
                        else if(sw.Equipo==equipo.rojo)
                        {
                            BitmapImage img = new BitmapImage();
                            img.BeginInit();
                            img.UriSource = new Uri("pack://application:,,,/res/rFarmer.png", UriKind.Absolute);
                            img.EndInit();
                            ctmp.Background = new ImageBrush(img);
                        }
                        Grid.SetColumn(ctmp, sw.PX);
                        Grid.SetRow(ctmp, sw.PY);
                        GrillaCancha.Children.Add(ctmp);
                    }
                    else if (sw is Oveja)
                    {
                        Canvas ctmp = new Canvas();
                        cancha[i, j] = ctmp;
                        ctmp.Tag = "ov";
                        if (sw.Equipo == equipo.azul)
                        {
                            BitmapImage img = new BitmapImage();
                            img.BeginInit();
                            img.UriSource = new Uri("pack://application:,,,/res/aoveja.png", UriKind.Absolute);
                            img.EndInit();
                            ctmp.Background = new ImageBrush(img);//new SolidColorBrush(Colors.DarkBlue);
                        }
                        else if (sw.Equipo == equipo.rojo)
                        {
                            BitmapImage img = new BitmapImage();
                            img.BeginInit();
                            img.UriSource = new Uri("pack://application:,,,/res/roveja.png", UriKind.Absolute);
                            img.EndInit();
                            ctmp.Background = new ImageBrush(img);//new SolidColorBrush(Colors.DarkBlue);
                        }
                        Grid.SetColumn(ctmp, sw.PX);
                        Grid.SetRow(ctmp, sw.PY);
                        GrillaCancha.Children.Add(ctmp);
                    }
                    else if (sw is Pelota)
                    {
                        Canvas ctmp = new Canvas();
                        cancha[i, j] = ctmp;
                        ctmp.Tag = "pl";
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri("pack://application:,,,/res/bola.png", UriKind.Absolute);
                        img.EndInit();
                        ctmp.Background = new ImageBrush(img);
                        Grid.SetColumn(ctmp, sw.PX);
                        Grid.SetRow(ctmp, sw.PY);
                        GrillaCancha.Children.Add(ctmp);
                    }  
                }
            }
            map.J1movio += moverJugador;
            map.J2movio += moverJugador;
            map.ovejaEnMovimientoWPF += moverOveja;
            map.pelotaEnMovimientoWPF += moverPelota;
        }
        public void moverJugador(Jugador j)
        {
            Jugador jugtmp = j;
            Canvas ct = cancha[j.antx, j.anty];
            Grid.SetColumn(ct, j.PX);
            Grid.SetRow(ct, j.PY);
            cancha[j.PX, j.PY] = ct;
            cancha[j.antx, j.anty] = null;
            j.antx = 0; //Incorporar en la propiedad
            j.anty = 0;
        }
        public void moverOveja(Oveja ov)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    Canvas ct = cancha[ov.antx, ov.anty];
                    if ((string)ct.Tag == "ov")
                    {
                        Grid.SetColumn(ct, ov.PX);
                        Grid.SetRow(ct, ov.PY);
                        cancha[ov.PX, ov.PY] = ct;
                        cancha[ov.antx, ov.anty] = null;
                        ov.antx = 0; //Incorporar en la propiedad
                        ov.anty = 0;
                    }
                }
                catch
                { 
                }
            }));
            
        }
        public void moverPelota(Pelota ov)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Canvas ct = cancha[ov.antx, ov.anty];
                if ((string)ct.Tag == "pl")
                {
                    Grid.SetColumn(ct, ov.PX);
                    Grid.SetRow(ct, ov.PY);
                    cancha[ov.PX, ov.PY] = ct;
                    cancha[ov.antx, ov.anty] = null;
                    ov.antx = 0; //Incorporar en la propiedad
                    ov.anty = 0;

                    if (ov.PX == 0 || ov.PX == 49)
                    {
                        if (ov.ultimoTirador == equipo.azul)
                        {
                            if (ov.PX == 49)
	                        {
                                this.agoles++;
	                        }
                            else
	                        {
                                this.rgoles++;
                                MessageBox.Show("derp");
	                        }
                            this.initnMap();
                        }
                        else if (ov.ultimoTirador == equipo.rojo)
                        {
                            if (ov.PX == 49)
                            {
                                this.agoles++;
                                MessageBox.Show("derp");
                            }
                            else
                            {
                                this.rgoles++;
                            }
                            this.initnMap();
                        }
                    }
                }
            }));

        }

        private void initnMap()
        {
            map.eliminar();
            this.initMap();
        }
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    map.moverJugador(2, 0, -1);
                    break;
                case Key.Down:
                    map.moverJugador(2, 0, 1);
                    break;
                case Key.Left:
                    map.moverJugador(2, -1, 0);
                    break;
                case Key.Right:
                    map.moverJugador(2, 1, 0);
                    break;
                case Key.W:
                    map.moverJugador(1, 0, -1);
                    break;
                case Key.A:
                    map.moverJugador(1, -1, 0);
                    break;
                case Key.S:
                    map.moverJugador(1, 0, 1);
                    break;
                case Key.D:
                    map.moverJugador(1, 1, 0);
                    break;
                case Key.D1:
                    map.ordenarOvejas(estado.ordencirculo, 1);
                    break;
                case Key.D2:
                    map.ordenarOvejas(estado.ordenflecha, 1);
                    break;
                case Key.D3:
                    map.ordenarOvejas(estado.ordenlinea, 1);
                    break;
                case Key.U:
                    map = new Mapa();
                    this.initnMap();
                    break;
            }
        }
    }


public class CanvasCDC : Canvas
{
    public double px = 0;
    public double py = 0;

    public CanvasCDC(double x, double y, double w, double h)
    {
        this.Margin = new Thickness();
        this.px = x;
        this.py = y;
        this.Width = w;
        this.Height = h;
    }
    public bool detectarColision(CanvasCDC p)
    {
        double bottom = this.py + this.Height;
        double top = this.py;
        double left = this.px;
        double right = this.px + this.Width;
        return !((bottom < p.py) ||
                 (top > p.py + p.Height) ||
                 (left > p.px + p.Width) ||
                 (right < p.px));
    }
}
}