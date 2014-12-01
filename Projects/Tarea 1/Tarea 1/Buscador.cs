using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_1
{
    class Buscador : Jugador
    {
        private int habilidad;
        private int rangoVision;
        private int salto;
        public bool persiguiendo = false;
        public Buscador(int posx, int posy, bool equipo, string nombre, char forma) : base(posx, posy, equipo, nombre)
        {
            Random rnd = new Random();
            this.forma = forma;
            this.HP = rnd.Next(160, 191);
            this.agilidad = rnd.Next(50,71)/100;
            this.vel_lineal = rnd.Next(6, 9);
            this.habilidad = rnd.Next(10,41);
        }
        private bool puedePerseguir(int psnitchX, int psnitchY)
        {
            return false;
        }
        public void vagar()
        {

        }
        public void perseguir(int psnitchX, int psnitchY)
        { 

        }
        public void eliminar()
        {

        }
    }
}
