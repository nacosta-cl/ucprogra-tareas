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
        public object obj;
        public object obj1;
        public object obj2;
        public object obj3;
        public object obj4;
        public object obj5;
        public object obj6;
        //Color
        public byte A;
        public byte B;
        public byte G;
        public byte R;
        public tipoMensaje tipo;
        string firma;
        public PaqueteEdit(tipoMensaje tipo)
        {
            this.tipo = tipo;
        }
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
