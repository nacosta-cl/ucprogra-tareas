using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Net.NetworkInformation;


namespace Tarea_6_Backend
{
    public class Cliente
    {
        int puerto;
        TcpClient clienteAsociado;
        IPEndPoint EPactual;

        public event Action<PaqueteEdit> pushRecibido;
        public event Action ConexionPerdida;

        ManualResetEvent paquetesPendientes = new ManualResetEvent(false);
        public AutoResetEvent editorIniciado = new AutoResetEvent(false);

        private Thread thOperador;
        private Thread thEnvio;

        private Queue<PaqueteEdit> paquetesEspera = new Queue<PaqueteEdit>();

        private string login;

        private BinaryFormatter formatbin = new BinaryFormatter();

        public string Login
        {
            get { return this.login; }
            set { ;}
        }

        public Cliente()
        {
            clienteAsociado = new TcpClient();
        }
        public void eliminar()
        {
            this.clienteAsociado.Close();
            this.thOperador.Abort();
            this.thEnvio.Abort();
        }
        public bool conectar(IPAddress ip, int puerto, string login)
        {
            this.login = login;
            this.puerto = puerto;
            string saludo = GetMACAddress() + '&' + login;
            try
            {
                EPactual = new IPEndPoint(ip, puerto);
                clienteAsociado.Connect(EPactual);
                NetworkStream stream = clienteAsociado.GetStream();
                formatbin.Serialize(stream, saludo);
                thOperador = new Thread(escucharServidor);
                thOperador.IsBackground = true;
                thOperador.Start();
                thEnvio = new Thread(enviarMensajeTh);
                thEnvio.IsBackground = true;
                thEnvio.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Retorna la primera direccion MAC que encuentra. Creditos a StackOverflow
        /// </summary>
        /// <returns></returns>
        private string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
        public void escucharServidor()
        {
            NetworkStream stream = clienteAsociado.GetStream();
            bool vivo = true;
            editorIniciado.WaitOne();
            while (vivo)
            {
                try
                {
                    if (stream.DataAvailable)
                    {
                        object msg = formatbin.Deserialize(stream);
                        PaqueteEdit pack = msg as PaqueteEdit;
                        pushRecibido(pack);
                    }
                    if(!clienteAsociado.Connected)
                    {
                        vivo = false;
                    }
                }
                catch
                {
                    vivo = false;
                }
            }
            //ConexionPerdida();
            this.eliminar();
        }
        public void enviarMensaje(PaqueteEdit msg)
        {
            paquetesEspera.Enqueue(msg);
            paquetesPendientes.Set();
        }
        private void enviarMensajeTh()
        {
            bool vivo = true;
            while (vivo)
            {
                paquetesPendientes.WaitOne();
                if (paquetesEspera.Count > 0)
                {
                    try
                    {
                        PaqueteEdit msg = paquetesEspera.Dequeue();
                        msg.Firma = this.login;
                        NetworkStream stream = clienteAsociado.GetStream();
                        formatbin.Serialize(stream, msg);
                    }
                    catch (System.Runtime.Serialization.SerializationException)
                    {
                        throw new Exception();
                    }
                    catch
                    {
                        vivo = false;
                    }
                }
                else paquetesPendientes.Reset();
            }
            ConexionPerdida();
            this.eliminar();
        }
    }
}
