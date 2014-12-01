using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Snitch : Pelota
    {
        public bool enPosesion = false;
        public Snitch(int posx, int posy) : base(posx, posy)
        {
            this.forma = '♫';
        }
    }
}
