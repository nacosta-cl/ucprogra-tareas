using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
namespace Tarea_5_Backend
{
    public class Oveja : Entidad
    {
        estado estadoActual = estado.normal;
        internal int debx = 0;   //Movimiento que debe
        internal int deby = 0;
        public event Action<Oveja> moverseAutonomo; //Mueve la oveja en el mapa, sola.
        public event Action<Oveja> patearPelota;
        public event Action<PowerUp> atrapaPU;
        public event Func<Oveja, int, int, int> puedeMover;
        public ManualResetEvent swPropio;
        public ManualResetEvent swPelota;
        public int antx = 0;
        public int anty = 0;
        public Oveja(int pxi, int pyi, equipo eq, ManualResetEvent sw) //Creando una oveja, y dejandola vivir en libertad!
        {
            this.swPropio = sw;
            this.swPelota = new ManualResetEvent(false);
            this.eq = eq;
            px = pxi;
            py = pyi;
            this.tpropio = new Thread(this.vivir);
            tpropio.IsBackground = true;
            tpropio.Start();
        }
        object key = new object();
        /// <summary>
        /// Debemos movimiento, del que sea. Vivir lo ejecuta solamente, sin tratar de equivocarse, según el comportamiento que le dio el jugador
        /// </summary>
        public override void vivir()
        {
            while(true)
            {
                swPropio.WaitOne();
                if (debx != 0 || deby != 0)
                {
                    lock (this)
                    {
                        int dx = 0;
                        int dy = 0;
                        if (debx != 0)
                        dx = debx / Math.Abs(debx); //Vector unitario x
                        if (deby != 0)
                        dy = deby / Math.Abs(deby); //Vector unitario y
                        if (intentarMovimiento(dx, dy))
                        {
                            int res = puedeMover(this, px + dx, py + dy);
                            
                            if (res==1)
                            {
                                antx = 0;
                                anty = 0;
                                antx = this.px;
                                anty = this.py;
                                this.px += dx;
                                this.py += dy;
                                if (debx != 0) debx-= dx;
                                if (deby != 0) deby-= dy;
                                moverseAutonomo(this);
                                Thread.Sleep(500);
                            }
                            else if(res==2)
                            {
                                patearPelota(this);
                                swPelota.WaitOne();
                                Thread.Sleep(400);
                                swPelota.Reset();
                                antx = 0;
                                anty = 0;
                                antx = this.px;
                                anty = this.py;
                                this.px += dx;
                                this.py += dy;
                                if (debx != 0) debx-= dx;
                                if (deby != 0) deby-= dy;
                                moverseAutonomo(this);
                                Thread.Sleep(400);
                            }
                        }
                    }
                }
                if (debx == 0 && deby == 0)
                swPropio.Reset();
            }
        }
        public void pelotaTermino()
        {
            swPelota.Set();
        }
        public bool mover(int dx, int dy) // si no se puede realizar, entonces nada
        {
            bool resultado = false;
            if (intentarMovimiento(dx, dy))
            {
                int res = puedeMover(this, px + dx, py + dy);
                if (res == 1)
                {
                    antx = this.px;
                    anty = this.py;
                    debx = dx;
                    deby = dy;
                    resultado = true;
                }
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }
        public void adqComp(Jugador j) //Adquirir cierto comportamiento respecto del jugador
        {
            int xdif = this.px - j.PX;
            int ydif = this.py - j.PY;
            double dist = Math.Sqrt(xdif*xdif+ydif*ydif);
            estadoActual = j.efectoOvejas;
            if (dist < j.rEspantarOvejas)
            {
                if (estadoActual == estado.normal)
                {
                    debx = xdif;
                    deby = ydif;
                }
                else if (estadoActual == estado.aturdir)
                {
                    debx = 0;
                    deby = 0;
                }
                else if (estadoActual == estado.confundir)
                {
                    
                }
            }
            // Si no, se quedan quietas. Muy quietas
        }
        public void espantarse()
        {
        }
        public void confundirse()
        {
            
            
        }
        public void adquirirFormacion(Jugador j)
        {
            
            if (estadoActual == estado.ordenflecha)
            {
                
            }
            else if (estadoActual == estado.ordencirculo)
            {
                    
            }
            else if (estadoActual == estado.ordenlinea)
            {
                    
            }
        }
        
    }
}
