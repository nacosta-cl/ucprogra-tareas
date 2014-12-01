using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Tarea_6_Backend
{
    public enum tipoMensaje
    {
        Chat, Pincel, AddCapa, ElimCapa, DescCapa, AscCapa, Linea, Rectangulo, Imagen,
        Ping
    }
    /// <summary>
    /// Objeto que se envía con diversos contenidos
    /// </summary>
    [Serializable]
    public class PaqueteEdit
    {
        /// <summary>
        /// Objeto numero 0 para enviar
        /// </summary>
        public object obj;
        /// <summary>
        /// Objeto numero 1 para enviar
        /// </summary>
        public object obj1;
        /// <summary>
        /// Objeto numero 2 para enviar
        /// </summary>
        public object obj2;
        /// <summary>
        /// Objeto numero 3 para enviar
        /// </summary>
        public object obj3;
        /// <summary>
        /// Objeto numero 4 para enviar
        /// </summary>
        public object obj4;
        /// <summary>
        /// Objeto numero 5 para enviar
        /// </summary>
        public object obj5;
        /// <summary>
        /// Objeto numero 6 para enviar
        /// </summary>
        public object obj6;
        //Colores
        public byte A;
        public byte B;
        public byte G;
        public byte R;
        private tipoMensaje tipo;
        string firma;
        /// <summary>
        /// Crea un paquete para enviar de un tipo en específico
        /// </summary>
        /// <param name="tipo">Tipo de paquete</param>
        public PaqueteEdit(tipoMensaje tipo)
        {
            this.tipo = tipo;
        }
        /// <summary>
        /// Recibe el tipo de paquete asignado por el remitente, revise enum para ver los tipos disponibles
        /// </summary>
        public tipoMensaje Tipo
        {
            get { return tipo; }
            set { ;}
        }
        /// <summary>
        /// Login del usuario que envió este paquete
        /// </summary>
        public string Firma
        {
            get { return firma; }
            set { firma=value;}
        }
    }
}
