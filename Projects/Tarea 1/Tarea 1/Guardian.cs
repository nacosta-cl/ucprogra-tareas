using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Guardian : Jugador
    {
        public int reflejo;
        private bool direccionarriba = true;
        public bool tienequaffle = false;
        public Guardian(int posx, int posy, bool equipo, string nombre, char forma) : base(posx, posy, equipo, nombre)
        {
            Random rnd = new Random();
            this.forma = forma;
            this.HP = rnd.Next(200,251);
            this.agilidad = rnd.Next(30,41);
            this.vel_lineal = 1;
            this.reflejo = rnd.Next(60, 91);
        }
        public Guardian(string dummy) : base(0,0,false," ")
        {
            dummy = " ";
        }
        public override bool Mover(int pfx, int pfy, long tics)
        {
            if (tienequaffle == true)
            {
                Mapa.atajada = true;
            }
            if (direccionarriba == true && posy == 7)
            {
                direccionarriba = false;
                posy++;
                return false;
            }
            else if (direccionarriba == false &&  posy == 13)
            {
                direccionarriba = true;
                posy--;
                return false;
            }
            else if(direccionarriba == true)
            {
                posy--;
                return true;
            }
            else if (direccionarriba == false)
            {
                posy++;
                return true;
            }
            return false;
        }
    }
}
