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
using Tarea_6_Backend;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tarea_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BinaryFormatter bf = new BinaryFormatter();
        List<Window> instancias = new List<Window>();
        bool hayServer = false;
        public MainWindow()
        {
            InitializeComponent();
            initCli.Click += initCli_Click;
            initSVCli.Click += initSV_Click;
        }

        void initSV_Click(object sender, RoutedEventArgs e)
        {
            if (!hayServer)
            {
                ServerPanel sp = new ServerPanel();
                Cliente cli = new Cliente();
                System.Net.IPAddress IP = System.Net.IPAddress.Parse("127.0.0.1");
                int port = 5000;
                bool errconn = false;
                if (loginField.Text.Length > 32 || loginField.Text.Length == 0)
                {
                    string mensajeError = "Error: Login Ausente o muy largo";
                    MessageBox.Show(mensajeError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    errconn = true;
                }
                else if (cli.conectar(IP, port, loginField.Text))
                {
                    sp.Show();
                    sp.Closed += sp_Closed;
                    Editor ed = new Editor(cli);
                    ed.Show();
                    ed.Closed += ed_Closed;
                    //initCli.IsEnabled = false;
                    //initSVCli.IsEnabled = false;
                    instancias.Add(sp);
                    instancias.Add(ed);
                    hayServer = true;
                }
                else
                {
                    string mensajeError = "Error: El cliente no pudo conectarse al servidor" + '\n';
                    MessageBox.Show(mensajeError, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    errconn = true;
                }
                if (errconn)
                {
                    sp.Servidor.eliminar();
                    sp = null;
                }
            }
            else
            {
                MessageBox.Show("Error: Solo puede haber un servidor en ejecución", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        void sp_Closed(object sender, EventArgs e)
        {
            instancias.Remove(sender as Window);
            hayServer = false;
        }

        void initCli_Click(object sender, RoutedEventArgs e)
        {
            bool pruebasOK = false;
            bool formatoOK = true;
            string mensajeError = "";
            System.Net.IPAddress IP = System.Net.IPAddress.Parse("127.0.0.1");
            int port = 0;
            Cliente cli = new Cliente();
            try
            {
                IP = System.Net.IPAddress.Parse(IPField.Text);
            }
            catch
            {
                mensajeError += "Error: IP inválida" + '\n';
                formatoOK = false;
            }
            try
            {
                port = int.Parse(puertoField.Text);
                if (port < 1 || port > 65535)
                {
                    throw new FormatException();
                }
            }
            catch
            {
                mensajeError += "Error: Puerto inválido" + '\n';
                formatoOK = false;
            }
            if (loginField.Text.Length > 32 || loginField.Text.Length == 0)
            {
                formatoOK = false;
                mensajeError += "Error: Login Ausente o muy largo";
            }
            if (formatoOK)
            {
                if (cli.conectar(IP, port, loginField.Text))
                {
                    pruebasOK = true;
                }
                else
                {
                    mensajeError += "Error: El cliente no pudo conectarse al servidor" + '\n';
                }
            }
            if (pruebasOK)
            {
                Editor ed = new Editor(cli);
                ed.Show();
                ed.Closed += ed_Closed;
                //initCli.IsEnabled = false;
                //initSVCli.IsEnabled = false;
            }
            else
            {
                MessageBox.Show(mensajeError,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        void ed_Closed(object sender, EventArgs e)
        {
            instancias.Remove(sender as Window);
        }
    }
}
