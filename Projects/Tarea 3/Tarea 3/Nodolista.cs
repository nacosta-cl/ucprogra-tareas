using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_3
{
    public class Nodolista
    {
        public Nodolista next;
        public int px;
        public int py;
        public object content;
        public Nodolista(object contenido)
        {
            content = contenido;
        }
    }
}
