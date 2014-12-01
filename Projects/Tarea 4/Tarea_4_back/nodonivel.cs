using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_4_back
{
    [Serializable]
    internal class nodonivel
    {
        public bool sumable;
        public int valor;
        public int px;
        public int py;
        public nodonivel(int px,int py, int valor)
        {
            this.px = px;
            this.py = py;
            this.valor = valor;
            this.sumable = true;
        }
    }
}
