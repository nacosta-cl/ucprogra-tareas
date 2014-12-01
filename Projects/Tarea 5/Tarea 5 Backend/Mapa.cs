using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Tarea_5_Backend
{
    /// <summary>
    /// Operador principal de una partida de sheepBall. 
    /// </summary>
    public class Mapa
    {
        public Random rnd = new Random();
        private const int largoX = 50;
        private const int largoY = 30;
        public Entidad[,] Cancha = new Entidad[largoX, largoY]; //Usar una cancha en forma de grilla
        Jugador Jugador1;
        public event Action<Jugador> J1movio; //Rojo, para las ovejas
        Oveja[] ovejas1 = new Oveja[8]; //Rojo

        ManualResetEvent[] switchesovs1 = new ManualResetEvent[10];

        Jugador Jugador2;
        public event Action<Jugador> J2movio;
        Oveja[] ovejas2 = new Oveja[8];

        ManualResetEvent[] switchesovs2 = new ManualResetEvent[10];

        Pelota pelota;

        Oveja[] ovejas; //10 ovejas por lado, 2 jugadores, 1 pelota... Principal recurso compartido
        public event Action<Oveja> ovejaEnMovimientoWPF;
        public event Action<Pelota> pelotaEnMovimientoWPF;
        public event Func<int[]> Coordsj;
        //campos para tick
        //Llaves para lock (O usar monitors?)
        object key0 = new object();
        ManualResetEvent ev = new ManualResetEvent(false);
        public Mapa()
        {
            for (int i = 0; i < 10; i++)
            {
                switchesovs1[i] = new ManualResetEvent(false);
                switchesovs2[i] = new ManualResetEvent(false);
            }
            pelota = new Pelota(25,15,equipo.neutro); //Solo existe una pelota
            pelota.moverseAutonomo += pelotaEnMovimiento;
            pelota.puedeMover += pelotaPuedeMoverse;
            Cancha[25, 15] = pelota;
            Jugador1 = new Jugador(5, 15, equipo.azul, switchesovs1);
            Jugador1.puedeMover += puedeMoverse;
            Cancha[5, 15] = Jugador1;
            Jugador2 = new Jugador(45, 15, equipo.rojo, switchesovs2);
            Jugador2.puedeMover += puedeMoverse;
            Cancha[45, 15] = Jugador2;
            for(int i = 0; i<8 ;i++)
            {
                ovejas1[i] = new Oveja(10, i + 12, equipo.azul, switchesovs1[i]); //Usa thread
                J1movio += ovejas1[i].adqComp; //Todas las ovejas están atentas al movimiento del jugador. Le pasamos el jugador para que analize
                ovejas1[i].moverseAutonomo += ovejaEnMovimiento; //Gatillado este evento, el mapa mueve la oveja, la oveja desacelera en su método
                ovejas1[i].puedeMover += ovejaPuedeMoverse;
                ovejas1[i].patearPelota += pelota.patearPelota;
                pelota.endmov += ovejas1[i].pelotaTermino;
                Cancha[10, i+12] = ovejas1[i];
            }
            for(int i = 0; i<8 ;i++)
            {
                ovejas2[i] = new Oveja(40, i + 12, equipo.rojo, switchesovs2[i]); //Usa thread
                J2movio += ovejas2[i].adqComp;
                ovejas2[i].moverseAutonomo += ovejaEnMovimiento;
                ovejas2[i].puedeMover += ovejaPuedeMoverse;
                ovejas2[i].patearPelota += pelota.patearPelota;
                pelota.endmov += ovejas2[i].pelotaTermino;
                Cancha[40, i+12] = ovejas2[i];
            }
            this.ovejas = new Oveja[ovejas1.Length + ovejas2.Length];
            this.ovejas1.CopyTo(ovejas, 0);
            this.ovejas2.CopyTo(ovejas, ovejas2.Length);
            
            //Array de ovejas
        }
        internal int pelotaPuedeMoverse(Entidad e, int nx, int ny) //Pregunta en la grilla si hay un null en esa posicion. de ser asi, la oveja se mueve
        {
            lock (key0)
            {
                int res = 0;
                if (Cancha[nx, ny] == null)
                {
                    res = 1;
                    Cancha[e.PX, e.PY] = null;
                    Cancha[nx, ny] = e;
                }
                return res;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <returns></returns>
        internal bool puedeMoverse(Entidad e, int nx, int ny) //Pregunta en la grilla si hay un null en esa posicion. de ser asi, la oveja se mueve
        {
            lock (key0)
            {
                bool res = false;
                if (Cancha[nx, ny] == null)
                {
                    res = true;
                    Cancha[e.PX, e.PY] = null;
                    Cancha[nx, ny] = e;
                }
                return res;
            }
        }
        public void eliminar()
        {
            foreach (Entidad ent in Cancha)
            {
                if (ent != null)
                {
                    ent.eliminar();
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <returns></returns>
        internal int ovejaPuedeMoverse(Entidad e, int nx, int ny) //Pregunta en la grilla si hay un null en esa posicion. de ser asi, la oveja se mueve
        {
            lock (key0)
            {
                int res = 0;
                if (Cancha[nx, ny] == null)
                {
                    res = 1;
                    Cancha[e.PX, e.PY] = null;
                    Cancha[nx, ny] = e;
                }
                if (Cancha[nx, ny] is Pelota)
                {
                    res = 2;
                    Cancha[e.PX, e.PY] = null;
                    Cancha[nx, ny] = e;
                }
                return res;
            }
        }
        //Sigue en lock
        void ovejaEnMovimiento(Oveja ov)
        {
            ovejaEnMovimientoWPF(ov);
        }
        void pelotaEnMovimiento(Pelota ov)
        {
            pelotaEnMovimientoWPF(ov);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="st">estado para activar</param>
        /// <param name="jug">Numero de jugador</param>
        public void ordenarOvejas(estado st, int jug)
        {
            Jugador j;
            if (jug==1)
            {
                int i = -4;
                j = Jugador1;
                j.efectoOvejas = st;
                if (st == estado.ordenlinea)
                {
                    foreach (Oveja ov in ovejas1)
                    {
                        ov.mover(j.PX + 5, j.PY+i);
                        i++;
                    }

                }
                
            }
            else if (jug == 2)
            {
                j = Jugador2;
                j.efectoOvejas = st;
            }
        }
        internal int[] calcularCentroGravedad(equipo eq)
        {
            int[] res = new int[2];
            if(eq == equipo.azul)
            foreach (Oveja ov in ovejas1)
            {
            }
            else if (eq == equipo.rojo)
            {
                foreach (Oveja ov in ovejas2)
                {
                }
            }
            return res;
        }
        /// <summary>
        /// Recibe un numero de jugador, y las diferencias de movimiento
        /// </summary>
        /// <param name="jugador"> id de jugador</param>
        /// <param name="dx"> dmov</param>
        /// <param name="dy"> dmov</param>
        public void moverJugador(int jugador, int dx, int dy)
        {
            if (jugador == 1)
            {
                if (Jugador1.mover(dx, dy))
                {
                    J1movio(Jugador1); //Notifica a las ovejas que se ha movido el jugador
                }
                
            }
            else if (jugador == 2)
            {   
                if (Jugador2.mover(dx, dy))
                {
                    J2movio(Jugador2);
                }
            }
        }
    }
}
//Using Syste.diagnostics 
//
//ctrl+w
