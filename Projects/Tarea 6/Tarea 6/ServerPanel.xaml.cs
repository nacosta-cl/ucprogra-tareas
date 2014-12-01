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
using Tarea_6_Backend;

namespace Tarea_6
{
    /// <summary>
    /// Interaction logic for ServerPanel.xaml
    /// </summary>
    public partial class ServerPanel : Window
    {
        Server sv;
        string logmsg = "";
        public Server Servidor
        {
            get { return this.sv; }
            set { ;}
        }
        public ServerPanel()
        {
            InitializeComponent();
            sv = new Server();
            fieldIP.Text = sv.IPLocal;
            fieldPort.Text = sv.Puerto.ToString();
            fieldIP.IsReadOnly = true;
            fieldPort.IsReadOnly = true;
            sv.alertaHandler += sv_alerta;
            cerrarSv.Click += cerrarSv_Click;
            Heavylog.Click += Heavylog_Checked;
            this.Closed += ServerPanel_Closed;
        }

        void ServerPanel_Closed(object sender, EventArgs e)
        {
            sv.eliminar();
            sv = null;
        }

        void Heavylog_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)Heavylog.IsChecked)
            {
                sv.verbose = true;
            }
            else
            {
                sv.verbose = false;
            }
        }

        void sv_alerta(string obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                logmsg += obj;
                logServer.Items.Add(obj);
            }));
        }

        void cerrarSv_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
