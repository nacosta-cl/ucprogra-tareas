using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tarea_3
{
    public static class opinterfaz
    {
        public static LibreriaT3.UI UI = new LibreriaT3.UI();
        public static bool nextstage = false;
        public static Celda portalO;
        public static Celda portalB;

        public static void draw(Lista lista)
        {
            //h=x, d=y
            for (int i = 0; i < lista.Largo; i++)
            {
                Celda tmp = (Celda)lista.buscar(i);
                UI.UpdateCell(tmp.py, tmp.px, tmp.celdatipo);
                if (tmp.ptipo != null)
                if (tmp.ptipo == LibreriaT3.PetalType.Normal && tmp.pcolor != null)
                {
                    LibreriaT3.PetalColor tmpcolor = (LibreriaT3.PetalColor)tmp.pcolor;     //Evitar declaraciones null
                    LibreriaT3.PetalType tmpptipo = (LibreriaT3.PetalType)tmp.ptipo;
                    UI.CreateHex(tmp.py, tmp.px, tmpptipo, tmpcolor);
                }
                else if (tmp.ptipo != LibreriaT3.PetalType.Normal)
                {
                    LibreriaT3.PetalType tmpptipo = (LibreriaT3.PetalType)tmp.ptipo;
                    UI.CreateHex(tmp.py, tmp.px, tmpptipo, LibreriaT3.PetalColor.Blue);
                }

            }

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool endgame = false;
            Operator op = new Operator();
            opinterfaz.UI.Link(op);
            opinterfaz.UI.ShowMessage("Debido a la imposibilidad de cambiar de etapa cuandose cumple el objetivo, para pasar a otra etapa hay que pulsar redo");
            while (op.mapacargado == false)
            {
            }
            while (endgame == false)
            {
                while (op.mapacargado == false)
                {
                }
                Console.WriteLine("Se cargaron todos los mapas");
                Lista etapas = op.etapas;
                for (int i = 0; i < etapas.Largo; i++)
                {
                    Thread.Sleep(1000);
                    op.win=false;
                    op.defeat = false;
                    op.reload = false;
                    Etapa etapaActual = (Etapa)etapas.buscar(i);
                    opinterfaz.portalB = etapaActual.portalB;
                    opinterfaz.portalO = etapaActual.portalO;
                    op.etapaActual = etapaActual;
                    opinterfaz.UI.NewBoard(etapaActual.eradio);
                    opinterfaz.draw(etapaActual.Listanodos);
                    opinterfaz.UI.DrawNextMoves(etapaActual.pushes);
                    opinterfaz.UI.ShowMessage("Nombre de la etapa: "+etapaActual.name + "\n" + "Numero de movimientos máximos: " + etapaActual.pushes.Length + "\n" + "Objetivo :" + etapaActual.objetivo);
                    while (!op.win && !op.defeat && !op.reload)
                    {

                    }
                }
                op.reload = false;
                op = new Operator();
                opinterfaz.UI.Link(op);
            }
        }
    }
}
