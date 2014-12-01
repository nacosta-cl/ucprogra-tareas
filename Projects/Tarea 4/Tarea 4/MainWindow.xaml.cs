using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
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
using Tarea_4_back;
using System.Xml.Serialization;

namespace Tarea_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool tipo = true;
        bool animar = false;
        List<int[]> highsc;
        List<juego> hjuegos = new List<juego>();
        public MainWindow()
        {
            InitializeComponent();
            //Abrir highscores
            FileStream archivo = new FileStream("./2048puntuaciones.xml", FileMode.OpenOrCreate);
            XmlSerializer serializador = new XmlSerializer(typeof(List<int[]>));
            try
            {
                highsc = (List<int[]>)serializador.Deserialize(archivo);
            }
            catch (InvalidOperationException)
            {
                highsc = new List<int[]>();
            }
            archivo.Close();
            anchoSlider.ValueChanged += anchoSlider_ValueChanged;
            this.Closed += MainWindow_Closed;
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            FileStream archivo = new FileStream("./2048puntuaciones.xml", FileMode.OpenOrCreate);
            XmlSerializer serializador = new XmlSerializer(typeof(List<int[]>));
            serializador.Serialize(archivo, highsc);
            foreach (juego j in hjuegos)
            {
                j.Close();
            }
            archivo.Close();
        }
        void anchoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            valslider.Content = anchoSlider.Value;
            int tam = 0;
            if (valslider.Content is string)
            {
                tam = int.Parse((string)valslider.Content);
            }
            else if (valslider.Content is double)
            {
                tam = Convert.ToInt32(valslider.Content);
            }
            if (tam > 15)
            {
                adv.Content = "Advertencia: \n Podria haber \n lentitud al jugar \n con animaciones";
            }
            else
            {
                adv.Content = "";
            }
        }
        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            //Ayudantes
            tipo = true;
        }
        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            //Numeros
            tipo = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Iniciar juego
            int tam = 0;
            if(valslider.Content is string)
            {
                tam = int.Parse((string)valslider.Content);
            }
            else if (valslider.Content is double)
            {
                tam = Convert.ToInt32(valslider.Content);
            }
            Mapa map = new Mapa(tam);
            juego ventanaJuego = new juego(map,tipo, this.animar, 2, highsc);
            ventanaJuego.Show();
        }
        void Revivir(object sender, RoutedEventArgs e)
        {
            this.Activate();
        }
        //Cargar
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opener = new Microsoft.Win32.OpenFileDialog();
            opener.DefaultExt = ".2ks";
            opener.Filter = "2048 Tableros |*.2ks";
            Nullable<bool> result = opener.ShowDialog();
            if (result == true)
            {
                string filename = opener.FileName;
                FileStream archivo = new FileStream(filename, FileMode.Open);
                try
                {
                    BinaryFormatter binary = new BinaryFormatter();
                    Mapa map = (Mapa)binary.Deserialize(archivo);
                    juego ventanaJuego = new juego(map, tipo, animar, 2, highsc);
                    ventanaJuego.Show();
                }
                catch(System.Runtime.Serialization.SerializationException)
                {
                    MessageBoxButton botones = MessageBoxButton.OK;
                    MessageBoxImage icono = MessageBoxImage.Error;
                    MessageBox.Show("Error, archivo inválido o corrupto","Error",botones,icono);
                }
                archivo.Close();
            }
        }

        private void CheckBox(object sender, RoutedEventArgs e)
        {
            animar = true;
        }
        private void UncheckedBox(object sender, RoutedEventArgs e)
        {
            animar = false;
        }
    }
}
