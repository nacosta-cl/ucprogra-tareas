using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tarea_5_Backend
{
    public enum equipo
    {
        rojo,azul,neutro
    }
    public enum estado //posibles para las ovejas
    {
        normal ,confundir, ordenflecha, ordenlinea, ordencirculo, aturdir
    }
    public abstract class Entidad
    {
        protected int desaceleracion = 1;
        private const int dimensionX = 50; //Dimensiones estandar del mapa
        private const int dimensionY = 30;
        protected int px;
        protected int py;
        protected equipo eq;
        protected Thread tpropio; //Cada entidad necesita un thread, menos los jugadores
        abstract public void vivir(); //Proceso que opera todas las instancias de entidades -> thread. Jugadores no lo requieren;
        protected int bufftime; //Tiempo que dura el efecto actual
        protected bool muerto = false;
        public virtual int PX
        {
            get { return this.px;}
            set { ; }
        }
        public virtual int PY
        {
            get { return this.py; }
            set { ; }
        }
        public equipo Equipo
        {
            get { return this.eq; }
            set { ; }
        }

        protected virtual bool intentarMovimiento(int dx, int dy)
        {
            bool resultado = false;
            if ((this.px + dx >= 0 + 1 && this.px + dx < dimensionX - 1) && (this.py + dy >= 0 && this.py + dy < dimensionY))
            {
                resultado = true;
            }
            return resultado;
        }
        public virtual void eliminar()
        {
            tpropio.Abort();
        }
    }
}
