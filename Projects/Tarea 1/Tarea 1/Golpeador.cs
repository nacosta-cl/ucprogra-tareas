using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Golpeador : Jugador
    {
        private int protector;
        public Golpeador(int posx, int posy, bool equipo, string nombre, char forma) : base(posx, posy, equipo, nombre)
        {
            Random rnd  = new Random();
            this.forma = forma;
            this.protector = rnd.Next(30,60);
        }
        public void seguirCazador(Cazador c1, Cazador c2, Cazador c3)
        {
            
        }
        public bool golpearBludger(Bludger b1, Bludger b2)
        {
            bool resultado = false;
            return resultado;
        }
    }
}
