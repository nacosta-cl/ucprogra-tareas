using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    abstract class Jugador : Entidad
    {

        protected int HP;
        protected string nombre;
        public bool equipo;
        protected int agilidad;
        public Jugador(int posx, int posy, bool equipo, string nombre ) : base(posx, posy)
        {
            this.nombre = nombre;
            this.equipo = equipo;
        }
    }
}
