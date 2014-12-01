using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Bludger : Pelota
    {
        private bool equipo;
        int vision;
        bool persiguiendo = false;
        bool aturdida = false;
        int distaobj;
        public Bludger(int posx, int posy, bool equipo) : base(posx, posy)
        {
            if (equipo == true) this.forma = '☺';
            else this.forma = '☻';
            this.posx = posx;
            this.posy = posy;
            this.vel_lineal = 6;
            this.equipo = equipo;
        }
        public bool buscar()
        {
            if(aturdida == true)
            {
            }
            else if (persiguiendo==false)
            {
                return false;
            }
            return false;
        }

        public bool atacar()
        {
            return false;
        }
    }
}
