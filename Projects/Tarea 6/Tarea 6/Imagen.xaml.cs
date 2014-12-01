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
using System.IO;
using System.Drawing;

namespace Tarea_6
{
   
    /// <summary>
    /// Interaction logic for Imagen.xaml
    /// </summary>
    public partial class Imagen : UserControl
    {
        private int pX = 0;
        private int pY = 0;
        Image img;
        public MemoryStream mem;
        bool exito = false;
        bool lockdown = false;
        string ext;
        public int X
        {
            get { return this.pX; }
            set { Canvas.SetLeft(this, value);
                    this.pX=value; }
        }
        public int Y
        {
            get { return this.pY; }
            set { Canvas.SetTop(this, value); 
                  this.pY = value; }
        }
        public bool Exito 
        { 
            get { return this.exito; } 
            set { ;} 
        }
        public string extension 
        {
            get { return this.ext;}
            set { ;} 
        }
        public Imagen(string path, string ext)
        {
            InitializeComponent();
            this.ext = ext;
            img = new Image();
            Stream stfile = File.Open(path,FileMode.Open,FileAccess.Read,FileShare.Read);
            mem = new MemoryStream();
            stfile.CopyTo(mem);
            try
            {
                if (ext == "JPG" || ext == "jpg")
                {
                    JpegBitmapDecoder decoder = new JpegBitmapDecoder(mem, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    BitmapSource bmpsrc = decoder.Frames[0];
                    img.Source = bmpsrc;
                    img.Stretch = Stretch.Fill;
                }
                else if (ext == "PNG" || ext == "png")
                {
                    PngBitmapDecoder decoder = new PngBitmapDecoder(mem, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    BitmapSource bmpsrc = decoder.Frames[0];
                    img.Source = bmpsrc;
                    img.Stretch = Stretch.Fill;
                }
                else if (ext == "BMP" || ext == "bmp")
                {
                    BmpBitmapDecoder decoder = new BmpBitmapDecoder(mem, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    BitmapSource bmpsrc = decoder.Frames[0];
                    img.Source = bmpsrc;
                    img.Stretch = Stretch.Fill;
                }
                exito = true;
            }
            catch
            {

            }
            if (exito)
            {
                this.bordeLocal.BorderThickness = new Thickness(0);
                this.MouseDown += Imagen_MouseDown;
                this.MouseLeave += Imagen_MouseLeave;
                this.MouseUp += Imagen_MouseLeave;
            }
        }

        void Imagen_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!lockdown)
            {
                this.bordeLocal.BorderThickness = new Thickness(0);
            }
            
        }

        void Imagen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!lockdown)
            {
                this.bordeLocal.BorderThickness = new Thickness(1);
            }
            
        }

        public Imagen(Stream stm, string ext)
        {
            InitializeComponent();
            this.ext = ext;
            img = new Image();
            mem = (MemoryStream)stm;
            mem.Position = 0;
            if (ext == "JPG" || ext == "jpg")
            {
                JpegBitmapDecoder decoder = new JpegBitmapDecoder(mem, BitmapCreateOptions.None, BitmapCacheOption.Default);
                BitmapSource bmpsrc = decoder.Frames[0];
                img.Source = bmpsrc;
                img.Stretch = Stretch.Fill;
            }
            else if (ext == "PNG" || ext == "png")
            {
                PngBitmapDecoder decoder = new PngBitmapDecoder(mem, BitmapCreateOptions.None, BitmapCacheOption.Default);
                BitmapSource bmpsrc = decoder.Frames[0];
                img.Source = bmpsrc;
                img.Stretch = Stretch.Fill;
            }
            else if (ext == "BMP" || ext == "bmp")
            {
                BmpBitmapDecoder decoder = new BmpBitmapDecoder(mem, BitmapCreateOptions.None, BitmapCacheOption.Default);
                BitmapSource bmpsrc = decoder.Frames[0];
                img.Source = bmpsrc;
                img.Stretch = Stretch.Fill;
            }
            exito = true;
        }
        public void inicializar(int w, int h, Point puntoInit)
        {
            imag.Background = new ImageBrush(img.Source);
            this.X = (int)puntoInit.X;
            this.Y = (int)puntoInit.Y;
            this.Width = w;
            this.Height = h;
            this.bordeLocal.Width = w;
            this.bordeLocal.Height = h;
            imag.Width = this.Width;
            imag.Height = this.Height;
        }
    }
}
