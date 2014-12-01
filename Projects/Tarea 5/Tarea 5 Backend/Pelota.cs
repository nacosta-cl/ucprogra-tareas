using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Tarea_5_Backend
{
    public class Pelota : Entidad
    {
        public bool op = true;
        public System.Timers.Timer tim = new System.Timers.Timer(500);
        public ManualResetEvent swpropio;
        int debx = 0;
        int deby = 0;
        public event Func<Pelota, int, int, int> puedeMover;
        public event Action<Pelota> moverseAutonomo;
        public event Action endmov;
        public equipo ultimoTirador = equipo.neutro;
        public int antx = 0;
        public int anty = 0;
        private const int dimensionX = 50; //Dimensiones estandar del mapa
        private const int dimensionY = 30;
        public override int PX
        {
            get { return this.px; }
            set { this.px = value; }
        }
        public override int PY
        {
            get { return this.py; }
            set { this.py = value; }
        }
        protected override bool intentarMovimiento(int dx, int dy)
        {
            bool resultado = false;
            if ((this.px + dx >= 0 + 1 && this.px + dx < dimensionX - 1) && ((this.py + dy >= 0) && (this.py + dy < dimensionY)))
            {
                resultado = true;
            }
            if ((this.px + dx == 0 || this.px + dx == dimensionX - 1) && ((this.py + dy > 13) && (this.py + dy < 18)))
            {
                resultado = true;
            }
            return resultado;
        }
        public Pelota(int pxi, int pyi, equipo eq)
        {
            swpropio = new ManualResetEvent(false);
            this.eq = eq;
            px = pxi;
            py = pyi;
            tpropio = new Thread(vivir);
            tpropio.IsBackground = true;
            tpropio.Start();
        }
        public override void vivir()
        {
            while (true)
            {
                this.swpropio.WaitOne();
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
                            if (res == 1)
                            {
                                antx = 0;
                                anty = 0;
                                antx = this.px;
                                anty = this.py;
                                this.px += dx;
                                this.py += dy;
                                if (debx != 0) debx -= dx;
                                if (deby != 0) deby -= dy;
                                moverseAutonomo(this);
                                endmov();
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                if (debx == 0 && deby == 0)
                    swpropio.Reset();
            }
        }
        public void patearPelota(Oveja ov)
        {
            ultimoTirador = ov.Equipo;
            debx = 3 * ov.debx;
            deby = 3 * ov.deby;
            swpropio.Set();
        }
    }
}
