using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Timers;

//Ver app iNet

namespace Tarea_6_Backend
{
    public class Server
    {
        const int ClientPort = 5000;
        const int maxConn = 4;
        const int tout = 1000;
        int cp = ClientPort;
        private string IPlocal = GetLocalIP();
        public int Puerto
        {
            get { return this.cp;}
            set { ;}
        }
        public string IPLocal
        {
            get { return this.IPlocal;}
            set { ;}
        }

        public bool verbose = false;
        
        private List<clienteSV> HistoricoClientes = new List<clienteSV>();
        private List<clienteSV> Clientes = new List<clienteSV>();

        private List<PaqueteEdit> HistoricoMensajes = new List<PaqueteEdit>();

        private TcpListener listenerServer;
        //private clienteSV clienteDesignado;
        private BinaryFormatter formatbin = new BinaryFormatter();

        public event Action<string> alertaHandler;
        public event Action<PaqueteEdit> mensajeRecibido;

        private Thread ThreadEscucha;
        public Server()
        {
            Clientes.Capacity = 4;
            listenerServer = new TcpListener(IPAddress.Any, ClientPort);
            listenerServer.Start(maxConn); 
            alertaHandler += Server_alertaHandler;

            ThreadEscucha = new Thread(RecibirConexiones);
            ThreadEscucha.IsBackground = true;
            ThreadEscucha.Start();
            PaqueteEdit pq = new PaqueteEdit(tipoMensaje.AddCapa);
            pq.obj = "Capa base";
            HistoricoMensajes.Add(pq);
            mensajeRecibido += pushClientes;
            
            #region
            //NetworkStream ns = cli.GetStream();
            //BinaryFormatter bf = new BinaryFormatter();
            //Console.WriteLine("Esperando conn");
            //while (true)
            //{
            //    string ms = Console.ReadLine();
            //    var mensaje = new msg(ms);
            //    bf.Serialize(ns, mensaje);
            //    Console.WriteLine("enviado obj con meensaje");
            //}
            //IPEndPoint endPointServer = null;
            //try
            //{
            //    endPointServer = new IPEndPoint(IPAddress.Any, extClientPort);
            //    socketServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    socketServidor.Bind(endPointServer);
            //    socketServidor.Listen(4);
            //}
            //catch (SocketException)
            //{
            //    return false;
            //}
            //listenerServer = new TcpListener(IPAddress.Any, extClientPort);

            //listenerServer.Start(maxConn);

            //Socket Socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint Ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8000);
            //Socket1.Bind(Ep);
            //Socket1.Listen(8000);
            //Socket1.Accept();
            //Socket1.Send(new byte[8]);
            //TcpListener sv = new TcpListener(IPAddress.Parse("127.0.0.1"),800);
            //sv.Start();
            //TcpClient cliente = sv.AcceptTcpClient();
            //NetworkStream Flujo = cliente.GetStream();
            #endregion
        }

        public void eliminar()
        {
            lock (Clientes)
            {
                foreach (clienteSV cli in Clientes)
                {
                    cli.ClienteTCP.Close();
                }
            }
            ThreadEscucha.Abort();
            Clientes = null;
            this.listenerServer.Stop();
        }
        void Server_alertaHandler(string obj) { return; }
        private void alerta(string msg)
        {
            string tmp = msg;
            alertaHandler(msg);
        }
        private void RecibirConexiones()
        {
            alerta("[i] Servidor iniciado" + '\n');
            alerta("[i] Puerto de conexion abierto" + '\n');
            while (true)
            {
                if (Clientes.Count != maxConn && listenerServer.Pending())
                {
                    alerta("[i] Intento de conexion recibido" + '\n');
                    clienteSV clienteEspera = null;
                    bool identificado = false;
                    bool error = false;
                    bool nuevocli = true;
                    while (!identificado && !error)
	                {
	                    try
                        {
                            TcpClient TCPcli = listenerServer.AcceptTcpClient();
                            NetworkStream stream = TCPcli.GetStream();
                            alerta("[i] Esperando handshake" + '\n');
                            while (!identificado)
                            {
                                if (stream.DataAvailable)
                                {
                                    object msg = formatbin.Deserialize(stream);
                                    if(verbose) alerta("[vv] Desserializacion completada" + '\n');
                                    string login = msg as string;          //string id~nombre
                                    if (verbose) alerta("[vv] Parsing completado" + '\n');
                                    string id = login.Split('&')[0];
                                    string nombre = login.Split('&')[1];
                                    alerta("[i] Recibido handshake: ID = " + id + " Nombre = " + nombre + '\n');
                                    foreach (clienteSV clitmp in HistoricoClientes)
                                    {
                                        if (!clitmp.conectado && (clitmp.ID == id || clitmp.nombre == nombre))
                                        {
                                            alerta("[i] Cliente antiguo detectado" + '\n');
                                            clienteEspera = clitmp;
                                            clitmp.conectado = true;
                                            clitmp.ClienteTCP = TCPcli;
                                            nuevocli = false;
                                        }
                                    }
                                    if (nuevocli)
                                    {
                                        alerta("[i] Cliente nuevo detectado" + '\n');
                                        clienteEspera = new clienteSV(id,nombre, true, TCPcli);
                                    }
                                    if (verbose) alerta("[vv] Identificacion finalizada" + '\n');
                                    identificado = true;
                                }
                            }
                        }
                        catch
                        {
                            alerta("[!] Error 0001 - Error en la conexion"+ '\n');
                            error = true;
                        }
	                }
                    if (identificado)
	                {
                        lock (Clientes)
                        {
                            if (nuevocli) HistoricoClientes.Add(clienteEspera);
                            Clientes.Add(clienteEspera);
                            alerta("[i] Cliente con nombre "+'"'+clienteEspera.nombre+'"'+" y ID = "+clienteEspera.ID+" está conectado" + '\n');
                            Thread th = new Thread(escucharCliente);
                            th.IsBackground = true;
                            th.Start(clienteEspera);
                            if (verbose) alerta("[vv] Hilo de cliente iniciado" + '\n');
                        }
	                }
                }
            }
        }
        private void escucharCliente(object cli)
        {
            clienteSV cliente = cli as clienteSV;
            NetworkStream stream = cliente.ClienteTCP.GetStream();
            bool vivo = true;
            try
            {
                if (cliente.nuevo)
                {
                    alerta("[i] Sincronizando cliente nuevo "+ cliente.nombre + '\n');
                    syncCli(cliente);
                }
            }
            catch
            {
                vivo = false;
            }
            alerta("[i] Cliente de nombre " + cliente.nombre + " iniciado" + '\n');
            while (vivo)
            {
                try
                {
                    if (stream.DataAvailable)
                    {
                        object msg = formatbin.Deserialize(stream);
                        PaqueteEdit pack = msg as PaqueteEdit;
                        if(!(pack.Tipo== tipoMensaje.Ping))
                        {
                            mensajeRecibido(pack);
                        }
                        
                    }
                    if (!cliente.ClienteTCP.Connected)
                    {
                        vivo = false;
                    }
                }
                catch
                {
                    vivo = false;
                }
            }
            eliminarCliente(cliente);
            //Eliminar al cliente
        }
        /// <summary>
        /// Sincroniza un cliente atrasado, enviandole el histórico de mensajes
        /// </summary>
        /// <param name="clienteAtrasado"></param>
        private void syncCli(clienteSV clienteAtrasado)
        {
            NetworkStream stream = clienteAtrasado.ClienteTCP.GetStream();
            alerta("[i] Iniciando sincronizacion con cliente : " + clienteAtrasado.nombre + '\n');
            try
            {
                if (HistoricoMensajes.Count != 0)
                {
                    foreach (PaqueteEdit item in HistoricoMensajes)
                    {
                        formatbin.Serialize(stream, item);
                    }
                }
            }
            catch
            {
                alerta("[i] Error en el proceso de sincronizacion del cliente. Sincronizacion abortada : " + clienteAtrasado.nombre + '\n');
                clienteAtrasado.conectado = false;
                eliminarCliente(clienteAtrasado);
            }
            if(verbose) alerta("[vv]Sincronizacion completada");
        }
        private void pushClientes(PaqueteEdit msg)
        {
            try
            {
                lock (Clientes)
                {
                    foreach (clienteSV cli in Clientes)
                    {
                        if (msg.Firma != cli.nombre)
                        {
                            NetworkStream stream = cli.ClienteTCP.GetStream();
                            formatbin.Serialize(stream, msg);
                        }
                    }
                }
                HistoricoMensajes.Add(msg);
            }
            catch
            {
            }
        }
        public static String GetLocalIP()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        private void eliminarCliente(clienteSV MFDcli)
        {
            if (Clientes!=null)
            {
                try
                {
                    lock (Clientes)
                    {
                        MFDcli.desconectar();
                        Clientes.Remove(MFDcli);
                        alerta("[i] Cliente desconectado : " + MFDcli.nombre + '\n');
                        MFDcli.conectado = false;
                        MFDcli.ClienteTCP = null;
                    }
                }
                catch
                {
                }
            }
            
            
        }
    }
}
