using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tarea_6_Backend
{
    internal class clienteSV
    {
        public bool conectado = false;
        public bool nuevo = true;
        private string id = "";
        public string nombre;
        internal TcpClient ClienteTCP;
        /// <summary>
        /// Recupera el id del cliente actual
        /// </summary>
        public string ID
        {
            get { return id; }
            set { ;}
        }

        public clienteSV(string ID, string nombre, bool conn, TcpClient clienteASOC)
        {
            this.nombre = nombre;
            this.id = ID;
            this.conectado = conn;
            this.ClienteTCP = clienteASOC;
        }
        public void desconectar()
        {
            this.ClienteTCP.Close();
            this.conectado = false;
        }
    }
}
