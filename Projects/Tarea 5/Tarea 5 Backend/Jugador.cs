using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tarea_5_Backend
{
    /// <summary>
    /// Los jugadores no se manejaran mediante threads, sino que serán objetos solos
    /// </summary>
    public class Jugador : Entidad
    {
        private int radioEspantar = 4;
        public estado efectoOvejas; // El jugador tendra un efecto para sus ovejas, el que sea, y el otro jugador se lo podrá pasar. Despues de un tick, este desaparece
        public int[] cgravOvejas;
        public int antx = 0;
        public int anty = 0;
        public event Func<Jugador, int, int, bool> puedeMover;
        public ManualResetEvent[] swovejas;
        public int rEspantarOvejas
        {
            get { return this.radioEspantar; }
            set {;}
        }
        public Jugador(int pxi, int pyi, equipo eq, ManualResetEvent[] swovejas)
        {
            this.swovejas = swovejas;
            px = pxi;
            py = pyi;
            this.eq = eq;
        }
        public override void eliminar()
        {}
        public override void vivir()
        {
        } //No es necesario
        /// <summary>
        /// Mover un jugador
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public bool mover(int dx, int dy) // si no se puede realizar, entonces nada
        {
            bool resultado = false;
            if (intentarMovimiento(dx,dy))
            { 
                if (puedeMover(this,px+dx,py+dy))
                {
                    antx = this.px;
                    anty = this.py;
                    this.px+=dx;
                    this.py+=dy;
                    foreach (ManualResetEvent sw in swovejas)
                    {
                        sw.Set();
                    }
                    resultado = true;
                }
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }
        public void nuevoEstado(estado est)
        {
            this.efectoOvejas = est;
        }

    }
}
