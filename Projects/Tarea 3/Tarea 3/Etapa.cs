using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea_3
{
    public static class counters
    {
        public static int chaincount = 0;
        public static int bloomcount = 0;
        public static int multibloomcount = 0;
        public static LibreriaT3.PetalColor? petalcreate;
        public static int chainobj = -1;
        public static int bloomobj = -1;
        public static int multobj = -1;
        public static int createobj = -1;
        public static string obj = "";
        public static void reset()
        {
            bloomcount=0;
            chaincount=0;
            multibloomcount=0;
            petalcreate=null;
            chainobj = -1;
            bloomobj = -1;
            multobj = -1;
            createobj = -1;
            obj = "";
        }
    }
    public class Etapa
    {

        private Celda Celdabase;
        public Lista Listanodos;
        private int radio;
        private string nombre;
        private string nobjetivo;
        public Celda portalO;
        public Celda portalB;
        public LibreriaT3.PetalColor[] pushes;
        private int indexpushactual;
        private int pushrestantes;
        public string objetivo
        {
            get { return this.nobjetivo; }
            set { ;}
        }
        public LibreriaT3.PetalColor[] cantPushes
        {
            get { return this.pushes; }
            set { ;}
        }
        public int pushesrestantes
        {
            get { return this.pushrestantes;}
            set { ;}
        }
        public Etapa(int radio, string name, string objetivo, LibreriaT3.PetalColor[] pushes)
        {
            this.pushes = pushes;
            pushrestantes = pushes.Length;
            this.nombre = name;
            this.radio = radio;
            Listanodos = new Lista();
            Celdabase = new Celda(0 ,0, Listanodos);
            Listanodos.Add(Celdabase);
            Celdabase.crecer(this.radio - 1);
            Celdabase.crearborde(this.radio);
            Celda dem = (Celda)Listanodos.buscar(1, 1);
            this.nobjetivo = objetivo; //Parsear esto a algo logico
            counters.obj = objetivo;
            this.name = nombre;
        }
        
        public LibreriaT3.PetalColor obtenerPush() //Obtiene el siguiente pétalo, imprime la lista con un petalo menos
        {
            LibreriaT3.PetalColor? push = null;
            try
            {
                LibreriaT3.PetalColor tpush = (LibreriaT3.PetalColor)pushes[0];
                Array.Reverse(this.pushes);
                LibreriaT3.PetalColor[] temp = new LibreriaT3.PetalColor[this.pushes.Length - 1];
                Array.Copy(this.pushes, temp, pushes.Length - 1);
                this.pushes = temp;
                Array.Reverse(this.pushes);
                opinterfaz.UI.DrawNextMoves(this.pushes);
                indexpushactual++;
                pushesrestantes--;
                push = tpush;
            }
            catch (IndexOutOfRangeException)
            {
                opinterfaz.UI.ShowMessage("");
            }
            return (LibreriaT3.PetalColor)push;
        }
        public string name
        {
            get { return this.nombre; }
            set { ; }
        }
        public int eradio
        {
            get { return this.radio ;}
            set { ;}
        }
        public void resetmfd()
        {
            for (int i = 0; i < this.Listanodos.Largo; i++)
            {
                Celda celda = (Celda)Listanodos.buscar(i);
                celda.MFD = false;
            }
        }
        public int checkblooms(bool repass)
        {
            Lista nodosMFD = new Lista(); //Centros que están marcados para ser eliminados
            for (int i = 0; i < this.Listanodos.Largo; i++)
            {
                Celda celda = (Celda)Listanodos.buscar(i);
                if(celda.rodeada() == true)
                {
                    nodosMFD.Add(celda);
                    if (!repass)
                    {
                        counters.bloomcount++;
                    }
                    else
                    {
                        counters.chaincount++;
                    }
                }
            }
            for (int i = 0; i < nodosMFD.Largo; i++)
            {
                Celda celda = (Celda)nodosMFD.buscar(i);
                celda.boom();
                celda.ptipo = null;
                celda.pcolor = null;
                opinterfaz.UI.DeleteHex(celda.py, celda.px);
            }
            return nodosMFD.Largo;
        }
        private int parseobjetivo()
        {
            int numobj = -1;
            if(this.objetivo.Contains("Multi"))
            {
            }
            else if(this.objetivo.Contains("Chain"))
            {
            }
            else if(this.objetivo.Contains("Empty"))
            {
            }
            else if(this.objetivo.Contains("push"))
            {
            }
            else if(this.objetivo.Contains("Clear"))
            {
            }
            else if (this.objetivo.Contains("Blooms"))
            {
            }
            else this.objetivo = "NOTIDENTIFIED";
            return numobj;
        }
        public bool objetivocumplido()
        {

            return false;
        }
    }
}
